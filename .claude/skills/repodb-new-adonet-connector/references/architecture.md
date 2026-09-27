# Architecture

A dedicated connector (`RepoDb.Connector.<Provider>`) is the layer *below* a RepoDB provider
(`RepoDb.<Provider>`, built with the `repodb-new-db-provider` skill). It is not RepoDB-specific at all —
it's a plain `System.Data.Common`-based ADO.NET data provider that happens to be built *for* use by
RepoDB, but works with any ADO.NET-consuming library (Dapper, hand-rolled data access, other ORMs) or
standalone.

## Why build one instead of using the underlying driver directly?

The underlying driver (Npgsql, MySqlConnector, an AWS/other cloud vendor's own wrapper, ...) almost always
already works fine on its own. A dedicated connector exists for one or both of these reasons:

1. **Namespace collision avoidance.** Several RepoDB providers can be PostgreSQL-wire-compatible
   (PostgreSQL, CockroachDB, EnterpriseDB, Aurora PostgreSQL) and all built on Npgsql underneath. If two of
   them both used bare `NpgsqlConnection`/`NpgsqlParameter`/`NpgsqlDbType` directly, RepoDB (and any
   consuming application) couldn't tell them apart or register different behavior for each — RepoDB's own
   mapper classes (`DbSettingMapper`, `DbHelperMapper`, `StatementBuilderMapper`) key off the *type* of
   `DbConnection`. Giving each engine its own `<Provider>Connection`/`<Provider>Parameter`/`<Provider>Type`
   types, distinct from the underlying driver's own types, is what makes this possible.
2. **Exposing a capability the raw driver doesn't have on its own.** `RepoDb.Connector.AuroraDb.Npgsql`
   wraps not just Npgsql but the AWS Advanced .NET Data Provider Wrapper on top of it, to surface
   Aurora-specific cluster awareness (failover, enhanced failure monitoring, read/write splitting, IAM
   auth, Secrets Manager) through a single connector, driven entirely by the connection string.

If neither reason applies — you're building support for an engine with no existing RepoDB sibling that
shares its wire protocol, and the driver has no extra capability worth surfacing — a dedicated connector
may be unnecessary; the RepoDB provider can potentially reference the driver's own types directly. Confirm
this isn't the case before investing in a full connector.

## The standard ADO.NET object set

Every connector implements the same fixed set of `System.Data.Common` abstract classes, regardless of
which driver sits underneath:

| Class | Base | Responsibility |
|---|---|---|
| `<Provider>Connection` | `DbConnection` | Open/close, connection string, state, command/transaction creation |
| `<Provider>Command` | `DbCommand` | `ExecuteNonQuery`/`ExecuteScalar`/`ExecuteReader` (+ `Async`), `Prepare`, `Cancel` |
| `<Provider>Parameter` | `DbParameter` | One bound parameter: name, value, `DbType`, direction, size, ... |
| `<Provider>ParameterCollection` | `DbParameterCollection` | The command's parameter list |
| `<Provider>DataReader` | `DbDataReader` | Forward-only result set reading |
| `<Provider>Transaction` | `DbTransaction` | Commit/rollback |
| `<Provider>Exception` | `DbException` | Wraps the driver's own exception type(s) |
| `<Provider>ConnectionStringBuilder` | `DbConnectionStringBuilder` | Strongly-typed connection string properties |
| `<Provider>Factory` | `DbProviderFactory` | Provider-agnostic object creation (`Instance` singleton) |

Plus two types that aren't part of `System.Data.Common` but are conventional across every RepoDB
connector:

- **`<Provider>Type`** — an enum mirroring the driver's own native-type enum (`NpgsqlDbType`,
  `MySqlDbType`, ...), renamed under the `<Provider>` prefix for the same collision-avoidance reason as
  everything else. Exposed as a settable property on `<Provider>Parameter` alongside the standard
  `DbType`.
- **`<Provider>TypeConverter`** — a static class converting between `<Provider>Type` and the driver's
  native enum, both directions, via a `Dictionary` built once and a reverse map derived from it. Only
  covers *scalar* types the driver enum has a clean 1:1 equivalent for — composite/structural values
  (arrays, generic range/multirange flags, internal-only types) have no `<Provider>Type` member at all;
  converting one throws `NotSupportedException` rather than silently guessing.

And, if the engine has (or can be given) a native bulk-load API, a `Bulk/` sub-namespace:

- **`<Provider>BulkCopy`** — the actual bulk-load implementation (see "Bulk copy" below).
- **`<Provider>BulkColumnMapping`** — one source→destination column mapping, by name and/or ordinal (four
  constructor overloads: name/name, name/ordinal, ordinal/name, ordinal/ordinal).
- **`<Provider>BulkCopyColumnMappingCollection`** — a `CollectionBase`-derived collection of the above.

This `<Provider>BulkCopy` class is what a sibling `RepoDb.<Provider>.BulkOperations` package (built with
the `repodb-new-db-provider-bulk-operations` skill) later orchestrates — the connector owns the actual
wire-level bulk-load mechanism, the RepoDB bulk-operations package only owns the pseudo-table staging
strategy built on top of it.

## Two wrapping shapes

**A. Direct wrap** (the common case — e.g. `RepoDb.Connector.CockroachDb` wrapping `NpgsqlConnection`
directly). Every `<Provider>Xxx` class holds one field of the driver's own type and every member is a
one-line forward:
```csharp
public class <Provider>Connection : DbConnection
{
    private readonly NpgsqlConnection _connection;
    public override string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }
    // ... every other member is the same one-line pattern
}
```

**B. Wrap-an-intermediate-wrapper** (e.g. `RepoDb.Connector.AuroraDb.Npgsql` wrapping
`AwsWrapperConnection<NpgsqlConnection>`, itself sitting on Npgsql). Same delegation pattern, one layer
further removed from the driver:
```csharp
public class <Provider>Connection : DbConnection
{
    private readonly AwsWrapperConnection<NpgsqlConnection> _connection;
    static <Provider>Connection() { NpgsqlDialectLoader.Load(); }   // one-time dialect/plugin registration
    // ... same one-line-forward pattern against _connection
    internal AwsWrapperConnection<NpgsqlConnection> InnerConnection => _connection;      // for sibling classes
    public AwsWrapperConnection WrappedConnection => _connection;                         // public escape hatch
}
```
Use shape B only when the intermediate wrapper genuinely adds capability (as with the AWS wrapper's
failover/EFM/IAM plugins) — it's extra indirection with no benefit otherwise. Either shape needs an
`internal` accessor to the wrapped object so sibling classes (`<Provider>Command`, `<Provider>Transaction`)
can pass it around, and shape B additionally exposes a `public` escape hatch (`WrappedConnection`,
`Unwrap<T>()`) for capabilities that have no ADO.NET-standard representation at all.

## Bulk copy: the one class that isn't pure delegation

Everywhere else in the connector, "implementation" means one-line forwarding. `<Provider>BulkCopy` is the
exception — it's a genuine implementation of a bulk-load protocol, because `WriteToServer`/
`WriteToServerAsync` isn't a method the driver already exposes in this shape; you're building it from
whatever lower-level primitive the driver *does* expose (Npgsql's `NpgsqlBinaryImporter` via
`BeginBinaryImportAsync`, MySqlConnector's `MySqlBulkCopy`, or — if the driver has nothing at all — raw
`INSERT` batching as a fallback). See `references/worked-example.md` for a complete real implementation
built on Npgsql's binary `COPY` protocol, and `references/checklist.md` for the design questions (source
type support, column-mapping resolution, connection ownership) it needs to answer regardless of which
underlying primitive you're building on.

Note that when using wrapping shape B, the bulk-load primitive is often driver-specific in a way the
intermediate wrapper doesn't expose at all — `AuroraDbBulkCopy` reaches straight through
`_connection.InnerConnection.Unwrap<NpgsqlConnection>()` to get the raw Npgsql connection needed to open a
binary importer, bypassing the AWS wrapper entirely for that one operation. This is exactly what the
public `Unwrap<T>()`/`WrappedConnection` escape hatch exists for.
