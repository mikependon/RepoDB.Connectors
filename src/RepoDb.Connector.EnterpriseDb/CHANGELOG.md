# Changelog for RepoDb.Connector.EnterpriseDb

All notable changes to the connectors in this repository are documented in this file. Each connector lives in its own directory under [`src/`](src) and ships as its own NuGet package with its own release cadence — this file tracks that history in one place, grouped by connector.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and each connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1

Date: 2026-09-02

> **Disclaimer**: `RepoDb.Connector.EnterpriseDb` started life as a 1:1 file copy of [RepoDb.Connector.MariaDbConnector](https://www.nuget.org/packages/RepoDb.Connector.MariaDbConnector) — same classes, same folder layout, same tests — and was then refactored to run on [Npgsql](https://www.nuget.org/packages/Npgsql) instead of MySqlConnector, with every `MariaDb`-prefixed type renamed to the `EDB`-prefixed naming convention used by EnterpriseDB's own [EnterpriseDB.EDBClient](https://www.nuget.org/packages/EnterpriseDB.EDBClient) driver. The "Refactor Notes" at the bottom of the `0.0.1` entry below detail exactly what that refactor touched.

Initial draft release of the EnterpriseDB connector built on [Npgsql](https://www.nuget.org/packages/Npgsql). Introduces the core ADO.NET provider objects and the bulk-copy support described in the connector's own [README](src/RepoDb.Connector.EnterpriseDb/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.EnterpriseDb` namespace), each wrapping the equivalent [Npgsql](https://www.nuget.org/packages/Npgsql) type behind the standard `System.Data.Common` provider model, using the same class names EnterpriseDB's own `EnterpriseDB.EDBClient` package uses for its Npgsql-derived types:

- `EDBConnection` — extends `DbConnection`. Establishes and manages a connection to an EnterpriseDB server, creates `EDBCommand` and `EDBTransaction` instances, and reports connection state, server version, and data source. Ships with both synchronous (`Open`) and asynchronous (`OpenAsync`) connection establishment.
- `EDBCommand` — extends `DbCommand`. Executes SQL statements against a `EDBConnection`. Implements `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`, each with an `Async` equivalent (`ExecuteNonQueryAsync`, `ExecuteScalarAsync`, `ExecuteReaderAsync`) that delegates directly to the underlying `NpgsqlCommand`, plus parameter creation and command preparation.
- `EDBDataReader` — extends `DbDataReader`. Reads the forward-only result set produced by a `EDBCommand`, exposing typed column accessors (`GetInt32`, `GetString`, `GetDateTime`, `GetGuid`, etc.). `Read`, `NextResult`, and `IsDBNull` each have an async counterpart (`ReadAsync`, `NextResultAsync`, `IsDBNullAsync`).
- `EDBParameter` — extends `DbParameter`. Represents a single named or positional parameter attached to a `EDBCommand`.
- `EDBParameterCollection` — extends `DbParameterCollection`. The strongly typed collection of `EDBParameter` objects exposed by `EDBCommand.Parameters`.
- `EDBTransaction` — extends `DbTransaction`. Wraps an EnterpriseDB transaction, providing `Commit` and `Rollback` semantics scoped to a `EDBConnection`.
- `EDBException` — extends `DbException`. Wraps the underlying `NpgsqlException` (and, transitively, `PostgresException` for server-raised errors) so consumers can catch a single, connector-specific exception type instead of depending on `Npgsql` directly.
- `EDBConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Provides strongly typed properties (`Host`, `Port`, `Database`, `Username`, `Password`, ...) for building and parsing EnterpriseDB connection strings.
- `EDBFactory` — extends `DbProviderFactory`. Lets provider-independent code construct `RepoDb.Connector.EnterpriseDb` ADO.NET objects (`CreateConnection`, `CreateCommand`, `CreateParameter`, ...) without a direct reference to the concrete types.
- `EDBType` — an enumeration of PostgreSQL/EnterpriseDB-specific column types spanning numeric, string, binary, date/time, network address, bit string, JSON, UUID/XML/HSTORE, text search, and geometric types.
- `EDBTypeConverter` — converts between `EDBType` and the underlying `NpgsqlTypes.NpgsqlDbType`, forming one leg of the `EDBType` ↔ `DbType` ↔ CLR type ↔ EnterpriseDB server type mapping.

**Bulk operations** (`RepoDb.Connector.EnterpriseDb.Bulk` namespace), built directly on Npgsql's binary `COPY` protocol:

- `EDBBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable` (optionally filtered by `DataRowState`), or `DataRow[]` into an EnterpriseDB table, with both synchronous (`WriteToServer`) and asynchronous (`WriteToServerAsync`) overloads for every source type. Internally opens a `NpgsqlBinaryImporter` via `COPY "table" (...) FROM STDIN (FORMAT BINARY)`, resolving any name-based column mapping against the source schema and the destination table's `information_schema.columns` output before streaming rows through `WriteRowAsync`.
- `EDBBulkColumnMapping` — defines the mapping between a single source column (by name or ordinal) and a destination column (by name or ordinal).
- `EDBBulkCopyColumnMappingCollection` — the collection of `EDBBulkColumnMapping` entries exposed by `EDBBulkCopy.ColumnMappings`.

#### Refactor Notes

Refactored `RepoDb.Connector.EnterpriseDb` (the copy under `src/`) into a real, independent Npgsql-based connector, following the same naming convention as EnterpriseDB's own `EnterpriseDB.EDBClient` package but wrapping the freely available Npgsql driver instead:

**Renaming (project, namespace, and class names)**
- Inner project/test folders, `.csproj`/`.slnx` files, `AssemblyName`/`Title`, and every `namespace`/`using` declaration renamed `RepoDb.Connector.MariaDbConnector` → `RepoDb.Connector.EnterpriseDb` (and `.Bulk`/`.UnitTests`/`.IntegrationTests` variants).
- Every `MariaDb*`-prefixed class renamed to its `EDB*` counterpart per EnterpriseDB.EDBClient's own naming convention: `MariaDbConnection` → `EDBConnection`, `MariaDbCommand` → `EDBCommand`, `MariaDbDataReader` → `EDBDataReader`, `MariaDbParameter`/`MariaDbParameterCollection` → `EDBParameter`/`EDBParameterCollection`, `MariaDbTransaction` → `EDBTransaction`, `MariaDbException` → `EDBException`, `MariaDbConnectionStringBuilder` → `EDBConnectionStringBuilder`, `MariaDbType`/`MariaDbTypeConverter` → `EDBType`/`EDBTypeConverter`, and `MariaDbBulkCopy`/`MariaDbBulkColumnMapping`/`MariaDbBulkCopyColumnMappingCollection` → `EDBBulkCopy`/`EDBBulkColumnMapping`/`EDBBulkCopyColumnMappingCollection`. The one deliberate naming exception: `MariaDbProviderFactory` became `EDBFactory` rather than `EDBProviderFactory`, matching the `EDBFactory` name EnterpriseDB.EDBClient itself uses for its `DbProviderFactory`.
- `MariaDbConnectionStringBuilder`'s `Server`/`UserId` properties became `Host`/`Username` on `EDBConnectionStringBuilder`, matching `NpgsqlConnectionStringBuilder`'s own property names (`Port` also changed from `uint` to `int`, again matching Npgsql).
- Renamed the integration-test database identifier (`RepoDb.Connector.MariaDbConnector` → `RepoDb.Connector.EnterpriseDb`) and switched the connection-string environment variables to `REPODB_ENTERPRISEDB_CONSTR`/`REPODB_ENTERPRISEDB_CONSTR_SYSTEM`.

**Library swap**
- `PackageReference` swapped from `MySqlConnector` 2.6.1 → `Npgsql` 10.0.3; every `using MySqlConnector;` → `using Npgsql;` (plus `using NpgsqlTypes;` for `EDBType`/`EDBTypeConverter`). Verified via reflection against the real Npgsql assembly that every `NpgsqlParameter`/`NpgsqlConnection`/`NpgsqlException`/`PostgresException`/etc. member this code touches has a match.
- **Target framework**: unlike the sibling connectors, `RepoDb.Connector.EnterpriseDb` targets `net8.0;net9.0;net10.0` only (no `netstandard2.0`) — Npgsql 10.x dropped `netstandard2.0` support (its last `netstandard2.0`-compatible major was 7.x), and the connector's bulk-copy implementation depends on `IAsyncDisposable`/`await using`, which `netstandard2.0` doesn't provide. This is a deliberate deviation, not an oversight.
- `EDBException.SqlState` no longer needs `MariaDbException`'s `#if NET8_0_OR_GREATER` conditional override, since every target framework here is already .NET 8+.

**Exception mapping**
- `EDBException` wraps `NpgsqlException` (the provider-level base type) rather than `PostgresException` (the server-raised subclass) directly, mirroring how `EnterpriseDB.EDBClient` itself layers `PostgresException : EDBException`. `ErrorCode` and `SqlState` both delegate straight to the wrapped `NpgsqlException`'s own same-named properties (Npgsql, unlike MySqlConnector, already exposes both as virtual members on the exception type itself — no `.Number`-style renaming or manual `PostgresException` type-checking was needed).

**Bulk copy: rebuilt on `NpgsqlBinaryImporter`, no `MySqlBulkCopy`-equivalent to wrap**
- Unlike MariaDB Connector's `MariaDbBulkCopy` (which wraps `MySqlConnector.MySqlBulkCopy` directly), Npgsql has no built-in bulk-copy class with a `WriteToServer(DataTable)`-style surface — the closest high-performance loading primitive is `COPY ... FROM STDIN (FORMAT BINARY)`, exposed as `NpgsqlConnection.BeginBinaryImport(string)` / `BeginBinaryImportAsync` returning a `NpgsqlBinaryImporter`. `EDBBulkCopy` hand-rolls the row/column streaming: it resolves the (source ordinal, destination column) pairs exactly as `MariaDbBulkCopy` did, builds a `COPY "table" (col1, col2, ...) FROM STDIN (FORMAT BINARY)` command, opens a `NpgsqlBinaryImporter`, and streams each source row through `WriteRowAsync` (translating `DBNull`/`null` values appropriately), finishing with `CompleteAsync` — whose `ulong` return value is the authoritative row count, used directly for `RowsCopied`.
- Destination-ordinal column-name resolution (used when a mapping refers to the destination column only by ordinal) queries `information_schema.columns` ordered by `ordinal_position`, replacing MariaDB Connector's `SHOW COLUMNS FROM` lookup. Identifier quoting switched from MySQL back-tick escaping to PostgreSQL double-quote escaping (doubling embedded `"` instead of `` ` ``).
- `BulkCopyTimeout` (int seconds) is applied to the importer's `Timeout` (a `TimeSpan`) only when set to a positive value, leaving Npgsql's own default in place otherwise.

**Data type notes**
- PostgreSQL has no direct equivalent of MariaDB's unsigned `TINYINT`; the `InsertModel.ColumnBit` test column became `SMALLINT` (CLR `short`) instead of `TINYINT UNSIGNED` (CLR `byte`), and the corresponding test/model code reads it via `GetInt16` rather than `GetByte`.
- The `Id` identity column uses `BIGINT GENERATED BY DEFAULT AS IDENTITY` (standard SQL identity) rather than MySQL's `AUTO_INCREMENT`; inserting an explicit row that should auto-generate `Id` uses the `DEFAULT` keyword in the `VALUES` list, since — unlike `AUTO_INCREMENT`, which treats an explicit `NULL` as "auto-assign" — a PostgreSQL identity column declared `NOT NULL` rejects an explicit `NULL` outright.
- Row GUIDs are generated with `gen_random_uuid()` (built into PostgreSQL 13+) in place of MySQL's `UUID()`, and stored as a native `UUID` column instead of `CHAR(36)`.

**Verification**
- Full solution builds clean (0 errors) targeting `net8.0`/`net9.0`/`net10.0`, and `dotnet pack` succeeds.
- All 70 unit tests pass consistently under MSTest's method-level parallelization, across all three target frameworks. One test (`ResetDbType` behavior) was adjusted from asserting an exact post-reset `DbType` to asserting the type is no longer the explicitly-set one — every API signature and behavioral difference here (default `CommandTimeout` of `30` vs. MySqlConnector's `0`; `DataSource` formatted as `tcp://host:port` vs. a bare host; `ResetDbType()` clearing to `DbType.Object`/`NpgsqlDbType.Unknown` rather than re-inferring from `Value`; a cancelled `OpenAsync` throwing `OperationCanceledException`; `Connection`-less command execution throwing `InvalidOperationException`) was confirmed via reflection and small throwaway probes against the real Npgsql 10.0.3 assembly, not assumed.
- Ran the full 58-test integration suite against the `enterprisedb` service added to the root `docker-compose.yml` (the official `postgres` image — see below): 58/58 pass, across all three target frameworks, including the full `WriteToServerTest` suite exercising the `EDBBulkCopy`/`NpgsqlBinaryImporter` COPY-based path end-to-end.

**Docs**
- Updated the connector's own `README.md` and `CHANGELOG.md` (renamed throughout, `MySqlConnector`→`Npgsql`, `MariaDb`→`EDB`, rewrote the Bulk Operations section to describe the `COPY`-based approach, and updated the license attribution to reference EnterpriseDB Corporation instead of MariaDB).

**Not touched (out of scope for this refactor)**
- The root `README.md`'s "Supported Connectors" table and root `CHANGELOG.md` don't yet list this new package, there's no CI workflow for it, and `docker-compose.yml` has no EnterpriseDB/PostgreSQL service defined.
