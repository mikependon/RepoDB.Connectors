# Design questions and implementation checklist

## Decide these before writing code

**Is a dedicated connector actually warranted?**
- [ ] Does another RepoDB provider already use the same underlying driver directly? If yes, a dedicated
      connector avoids the type-collision problem described in `references/architecture.md`. If no other
      provider shares this driver, and the driver exposes no extra capability worth surfacing, consider
      whether the RepoDB provider can reference the driver's types directly instead of building this layer.

**Which underlying library to wrap, and how deep?**
- [ ] Wrap the raw driver directly (shape A), or wrap an intermediate capability-adding wrapper library
      (shape B)? Only choose B if that intermediate library adds something genuinely worth exposing
      (failover, connection pooling behavior, credential management, ...) — extra indirection with no
      benefit just adds a class of bugs (see the `Unwrap<T>()` need below) for nothing.
- [ ] If shape B, does the intermediate wrapper need any one-time static registration/dialect loading
      before it can drive this specific underlying driver (mirror `AuroraDbConnection`'s static
      constructor calling `NpgsqlDialectLoader.Load()`)?
- [ ] Does *anything* the connector needs to expose live only on the raw driver, invisible through the
      intermediate wrapper (a binary bulk-load protocol, a driver-specific connection property)? If so,
      plan the public escape hatch (`WrappedConnection`, `Unwrap<T>()`) from the start rather than bolting
      it on later.

**Exception handling**
- [ ] Which of the underlying driver's/wrapper's exception types represent a genuine database error that
      should be normalized into `<Provider>Exception` (so callers can catch one exception type regardless
      of what's underneath)? Which represent something a caller needs to see undisguised — a
      failover-in-progress signal, a cancellation, anything documented by the underlying library as
      "catch this specific type and react accordingly"? Wrapping *everything* indiscriminately can hide
      exactly the type-based dispatch a caller is supposed to do on the underlying library's own
      documented exception hierarchy.

**Type system**
- [ ] What's the driver's own native-type enum? Enumerate every *scalar* value from it — one `<Provider>Type`
      member each. Deliberately exclude composite/structural values (arrays, generic range/multirange
      flags, internal-only types) rather than inventing a member that doesn't cleanly correspond; document
      that converting one throws `NotSupportedException` and that arrays/similar fall back to the driver's
      own value-inferred type handling.
- [ ] Does the connection string need a strongly-typed builder property for anything beyond the basics
      (host/port/database/username/password) — a wrapper-specific setting, a driver-specific performance
      flag worth surfacing?

**Bulk copy (if the engine has, or can be given, a native bulk-load primitive)**
- [ ] What's the actual lower-level primitive — a binary import protocol (Npgsql's `NpgsqlBinaryImporter`),
      a purpose-built bulk-copy class the driver already ships (`MySqlBulkCopy`), something else?
- [ ] Which source shapes need `WriteToServer`/`WriteToServerAsync` overloads — at minimum `IDataReader`
      (the shared core, everything else can funnel through it), `DataTable` (+ a `DataRowState`-filtered
      variant), `DataRow[]`. Build all the others as thin adapters over one shared engine method rather than
      duplicating the streaming loop per source type.
- [ ] How does a column mapping resolve when it names a column by ordinal instead of by name, on either
      side? If the destination is given by ordinal, does resolving it require a metadata round-trip
      (`information_schema.columns` or equivalent), and can that round-trip be skipped when every mapping
      already names its destination column directly?
- [ ] Does the bulk-load primitive need the connection opened first, and if the caller handed in an
      already-open connection, does `<Provider>BulkCopy` need to leave it open afterward (only close what
      it itself opened)?
- [ ] How should `null` values be represented to the underlying primitive — does it want a real
      language-level `null`, or does it accept `DBNull.Value` directly? (Npgsql's binary importer wants a
      real `null`; check your specific driver rather than assuming.)

## Implementation order

1. `<Provider>Connection` — get open/close, connection string, and state working first; everything else
   needs a working connection to construct against.
2. `<Provider>Command` + `<Provider>Parameter` + `<Provider>ParameterCollection` — enough to run a
   parameterized query.
3. `<Provider>DataReader` — enough to read results back.
4. `<Provider>Transaction`.
5. `<Provider>Exception`, `<Provider>ConnectionStringBuilder`, `<Provider>Factory` — the remaining
   `System.Data.Common` surface.
6. `<Provider>Type` + `<Provider>TypeConverter`.
7. `Bulk/<Provider>BulkCopy` (+ the two column-mapping support classes), if applicable — this is
   substantially more work than everything above combined, since it's genuine protocol implementation
   rather than delegation; budget accordingly.

## Tests

- **Unit tests** (no live database) — one test class roughly per connector class:
  - Connection/ConnectionStringBuilder: does a given connection string parse into the right
    `DataSource`/`Database`/etc.?
  - Command/CommandConnection: does assigning a connection wire up the underlying command's connection
    correctly? Does executing without a connection throw `InvalidOperationException`?
  - Parameter/ParameterCollection: do property get/set pairs round-trip through to the wrapped object?
  - ParameterType: does setting `<Provider>Type` update the underlying driver enum property and vice
    versa?
  - TypeConverter: does every mapped value convert correctly both directions, and does an unmapped
    (composite/structural) value throw `NotSupportedException`?
  - Exception: does wrapping preserve `Message`/`InnerException`/`ErrorCode`/`SqlState` correctly?
  - Factory: does each `Create*` method return the right concrete type?
  - Bulk (if applicable): column-mapping construction (all four name/ordinal combinations), the
    mapping collection's `Add` overloads.
- **Integration tests** (against a real local instance, via `docker-compose.yml`):
  - Basic connect/open/close, `ExecuteNonQuery`/`ExecuteReader`/`ExecuteScalar`, transactions
    (commit and rollback), a `DataTable`-based round trip.
  - **A dedicated type round-trip test per mapped `<Provider>Type` value** — write a value through a
    parameter with that type explicitly set, read it back, assert equality. This is the test most likely
    to catch a wrong entry in the type-converter map.
  - A parallel test round-tripping via the driver's own native type (bypassing `<Provider>Type`
    entirely) for at least a representative subset — confirms the mapping matches what the driver
    actually does, not just what the enum claims.
  - `WriteToServer`/`WriteToServerAsync` for each supported source shape, including at least one
    explicit-column-mapping case and one relying on the metadata-driven ordinal-to-name resolution.
