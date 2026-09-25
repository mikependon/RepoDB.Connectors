<div align="center">
    <a href="https://www.nuget.org/packages/RepoDb.Connector.CockroachDb">
        <image src="logo.png" style="width:256px;" />
    </a>
    <br/>
    <span style="font-size:16px;font-weight:bold;">A lightweight, modern, and open-source ADO.NET data provider for CockroachDB, built for RepoDB.</span>
</div>

-----

> **Disclaimer**: This is an independent, unofficial .NET provider for CockroachDB. It is a thin ADO.NET wrapper and type-mapping layer built on top of [Npgsql](https://www.nuget.org/packages/Npgsql) and is not affiliated with or endorsed by Cockroach Labs, Inc.

The project aims to provide a dedicated CockroachDB connector based on the standard `System.Data.Common` abstractions, while exposing CockroachDB-specific data types, behaviors, and capabilities where applicable. All objects follow the `CockroachDb`-prefixed naming convention, so that they never collide with the `Npgsql`-prefixed objects of the underlying driver (or of any other PostgreSQL-compatible connector) within the same application.

It implements the `Async` equivalent of the [Npgsql](https://www.nuget.org/packages/Npgsql) library, which is the .NET driver recommended by Cockroach Labs, as CockroachDB speaks the PostgreSQL wire protocol. It also covers the full implementation of Bulk operations using Npgsql's binary `COPY` protocol (`NpgsqlBinaryImporter`).

> **Status:** Early development. The API and implementation are subject to change.

## Why is this exists?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated CockroachDB objects are required within its extension library to avoid class collisions with the other PostgreSQL-compatible providers (PostgreSQL, EnterpriseDB, ...) and to expose the CockroachDB-specific type system - which is a subset of PostgreSQL's, with a few differences of its own.

This library will serve as the **official CockroachDB connector for RepoDB** and will be used internally by the [RepoDB project](https://github.com/mikependon/RepoDB).

## Goals of the library

RepoDb.Connector.CockroachDb aims to:

* Provide a dedicated ADO.NET data provider for CockroachDB.
* Follow the standard `System.Data.Common` provider architecture.
* Support modern synchronous and asynchronous .NET APIs.
* Provide CockroachDB-specific data type mappings and behaviors.
* Remain lightweight and suitable for use by ORMs and other data-access libraries.
* Support high-performance CockroachDB operations, including bulk operations.
* Remain usable independently of any ORM.

## Core ADO.NET Objects

RepoDb.Connector.CockroachDb is built around the standard abstractions provided by `System.Data.Common`.

The following provider-specific objects form the core of the connector:

| RepoDb.Connector.CockroachDb        | ADO.NET Base Class          | Purpose                                        |
| ------------------------------------ | --------------------------- | ---------------------------------------------- |
| `CockroachDbConnection`             | `DbConnection`              | Establishes and manages CockroachDB connections |
| `CockroachDbCommand`                | `DbCommand`                 | Executes SQL commands                          |
| `CockroachDbDataReader`             | `DbDataReader`              | Reads query results                            |
| `CockroachDbParameter`              | `DbParameter`               | Represents command parameters                  |
| `CockroachDbParameterCollection`    | `DbParameterCollection`     | Manages command parameters                     |
| `CockroachDbTransaction`            | `DbTransaction`             | Manages database transactions                  |
| `CockroachDbException`              | `DbException`               | Represents CockroachDB errors                  |
| `CockroachDbConnectionStringBuilder`| `DbConnectionStringBuilder` | Builds and parses connection strings           |
| `CockroachDbFactory`                | `DbProviderFactory`         | Creates provider-specific ADO.NET objects      |

The architecture follows the standard ADO.NET provider model:

```text
System.Data.Common
│
├── DbConnection
│     └── CockroachDbConnection
│
├── DbCommand
│     └── CockroachDbCommand
│
├── DbDataReader
│     └── CockroachDbDataReader
│
├── DbParameter
│     └── CockroachDbParameter
│
├── DbParameterCollection
│     └── CockroachDbParameterCollection
│
├── DbTransaction
│     └── CockroachDbTransaction
│
├── DbException
│     └── CockroachDbException
│
├── DbConnectionStringBuilder
│     └── CockroachDbConnectionStringBuilder
│
└── DbProviderFactory
      └── CockroachDbFactory
```

## Basic Usage

RepoDb.Connector.CockroachDb is intended to provide the familiar ADO.NET programming model.

```csharp
using RepoDb.Connector.CockroachDb;

var connectionString =
    "Host=localhost;" +
    "Port=26257;" +
    "Database=defaultdb;" +
    "Username=root;" +
    "SSL Mode=Disable;";

await using var connection =
    new CockroachDbConnection(connectionString);

await connection.OpenAsync();

await using var command = connection.CreateCommand();

command.CommandText = """
    SELECT "Id", "Name", "Email"
    FROM "Customer"
    WHERE "Id" = @Id;
    """;

command.Parameters.AddWithValue("@Id", 100L);

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    var id = reader.GetInt64(0);
    var name = reader.GetString(1);
    var email = reader.GetString(2);

    Console.WriteLine($"{id}: {name} ({email})");
}
```

> The connection string above targets an **insecure** local single-node cluster (e.g. `cockroach start-single-node --insecure`). A secure cluster, including CockroachDB Cloud, requires `SSL Mode=VerifyFull` together with a password (or a client certificate).

## CockroachDbConnection

`CockroachDbConnection` extends `DbConnection` and represents a connection to a CockroachDB cluster.

```csharp
await using var connection =
    new CockroachDbConnection(connectionString);

await connection.OpenAsync();

Console.WriteLine(connection.ServerVersion);
Console.WriteLine(connection.Database);
Console.WriteLine(connection.State);
```

Its responsibilities include:

* Connection establishment and termination
* Connection state management
* CockroachDB session management
* Command creation
* Transaction creation
* Connection string handling
* Synchronous and asynchronous operations

## CockroachDbCommand

`CockroachDbCommand` extends `DbCommand` and represents a SQL statement executed against CockroachDB.

```csharp
await using var command = new CockroachDbCommand(
    "SELECT * FROM \"Customer\" WHERE \"Id\" = @Id",
    connection);

command.Parameters.AddWithValue("@Id", 100L);

await using var reader =
    await command.ExecuteReaderAsync();
```

The implementation is intended to support:

* `ExecuteNonQuery`
* `ExecuteScalar`
* `ExecuteReader`
* Async equivalents
* Parameterized SQL
* Prepared statements
* Command timeout
* Cancellation
* Multiple result sets

## CockroachDbParameter

`CockroachDbParameter` extends `DbParameter` and represents a parameter associated with a `CockroachDbCommand`.

```csharp
var parameter = new CockroachDbParameter
{
    ParameterName = "@Id",
    Value = 100L
};

command.Parameters.Add(parameter);
```

The native CockroachDB type of a parameter can be set explicitly via the `CockroachDbType` property:

```csharp
var parameter = new CockroachDbParameter
{
    ParameterName = "@Document",
    CockroachDbType = CockroachDbType.Jsonb,
    Value = "{\"name\": \"RepoDB\"}"
};
```

## CockroachDB Data Types

RepoDb.Connector.CockroachDb provides a `CockroachDbType` enumeration in addition to the standard ADO.NET `DbType`. It only lists the types that CockroachDB actually implements:

```csharp
public enum CockroachDbType
{
    // Numeric
    SmallInt,      // INT2
    Integer,       // INT4
    BigInt,        // INT8 (INT, INTEGER, BIGINT)
    Decimal,       // DECIMAL (NUMERIC)
    Real,          // FLOAT4 (REAL)
    Double,        // FLOAT8 (FLOAT, DOUBLE PRECISION)
    Boolean,       // BOOL

    // String
    Char,          // CHAR(n)
    VarChar,       // VARCHAR(n)
    Text,          // STRING (TEXT)
    Name,          // NAME
    Citext,        // CITEXT

    // Binary
    Bytea,         // BYTES (BYTEA)

    // Date and Time
    Date,
    Time,
    TimeTz,
    Timestamp,
    TimestampTz,
    Interval,

    // Network Address
    Inet,

    // Bit String
    Bit,
    VarBit,

    // JSON
    Jsonb,         // JSONB (JSON is an alias)

    // Other
    Uuid,
    Oid,
    LTree,

    // Text Search
    TsVector,
    TsQuery,

    // Spatial
    Geometry,
    Geography
}
```

The connector provides mappings between:

```text
CockroachDbType
     ↕
DbType
     ↕
.NET CLR Type
     ↕
CockroachDB Server Type
```

`CockroachDbTypeConverter` provides the current leg of that mapping, converting between `CockroachDbType` and the underlying `NpgsqlTypes.NpgsqlDbType`:

```csharp
var cockroachDbType = CockroachDbTypeConverter.ToCockroachDbType(NpgsqlDbType.Varchar);
var npgsqlDbType = CockroachDbTypeConverter.ToNpgsqlDbType(CockroachDbType.BigInt);
```

### Differences from PostgreSQL

Although CockroachDB is wire-compatible with PostgreSQL, its type system differs in a few important ways that the connector reflects:

* **`INT` is 64-bit.** A column declared as `INT`/`INTEGER` is an `INT8` in CockroachDB, so it is read back as a `long`. Declare the column as `INT4` explicitly if a 32-bit `int` is desired.
* **`JSON` is an alias of `JSONB`.** There is no separate textual `JSON` type - `NpgsqlDbType.Json` is therefore converted to `CockroachDbType.Jsonb`.
* **Unsupported PostgreSQL types.** `MONEY`, `CIDR`, `MACADDR`/`MACADDR8`, `XML`, `HSTORE`, `JSONPATH`, the geometric types (`POINT`, `LINE`, `LSEG`, `BOX`, `PATH`, `POLYGON`, `CIRCLE`) and the range types (`INT4RANGE`, `TSTZRANGE`, ...) are not implemented by CockroachDB. They are intentionally absent from `CockroachDbType`, and converting them throws a `NotSupportedException`.
* **Spatial types.** `GEOMETRY` and `GEOGRAPHY` are mapped, but reading and writing their values requires the [Npgsql.NetTopologySuite](https://www.nuget.org/packages/Npgsql.NetTopologySuite) (or [Npgsql.GeoJSON](https://www.nuget.org/packages/Npgsql.GeoJSON)) plugin.
* **Arrays, `ENUM` and `VECTOR`.** These are supported by CockroachDB and work through Npgsql's default (value-inferred) type handling, but they have no dedicated `CockroachDbType` member.

## Transactions

`CockroachDbTransaction` extends `DbTransaction` and provides standard ADO.NET transaction semantics.

```csharp
await using var transaction =
    await connection.BeginTransactionAsync();

try
{
    await using var command = connection.CreateCommand();

    command.Transaction = transaction;

    command.CommandText =
        "UPDATE \"Customer\" SET \"Name\" = @Name WHERE \"Id\" = @Id";

    command.Parameters.AddWithValue("@Name", "John Doe");
    command.Parameters.AddWithValue("@Id", 100L);

    await command.ExecuteNonQueryAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

> CockroachDB runs at `SERIALIZABLE` isolation by default and may ask the client to retry a transaction under contention (SQLSTATE `40001`). Such errors surface as a `CockroachDbException` and should be handled with a client-side retry loop.

## Connection String Builder

`CockroachDbConnectionStringBuilder` extends `DbConnectionStringBuilder` and provides a strongly typed way of creating CockroachDB connection strings.

```csharp
var builder = new CockroachDbConnectionStringBuilder
{
    Host = "localhost",
    Port = 26257,
    Database = "defaultdb",
    Username = "root"
};

await using var connection =
    new CockroachDbConnection(builder.ConnectionString);

await connection.OpenAsync();
```

## Provider Factory

`CockroachDbFactory` extends `DbProviderFactory` and enables provider-independent ADO.NET applications and libraries to create RepoDb.Connector.CockroachDb objects.

```csharp
var factory = CockroachDbFactory.Instance;

using var connection = factory.CreateConnection();

connection.ConnectionString = connectionString;
connection.Open();
```

## Additional ADO.NET Support

Future releases may provide additional traditional ADO.NET components:

```text
CockroachDbDataAdapter
    └── DbDataAdapter

CockroachDbCommandBuilder
    └── DbCommandBuilder

CockroachDbDataSource / CockroachDbDataSourceBuilder
    └── DbDataSource
```

These components will provide support for `DataTable`, `DataSet`, connection pooling configuration, and other traditional ADO.NET workflows.

## Bulk Operations

RepoDb.Connector.CockroachDb provides bulk-loading support under the `RepoDb.Connector.CockroachDb.Bulk` namespace, built directly on top of Npgsql's binary `COPY` protocol (`NpgsqlBinaryImporter`, opened via `NpgsqlConnection.BeginBinaryImport`), which CockroachDB supports.

| RepoDb.Connector.CockroachDb.Bulk          | Purpose                                                                                                       |
| ------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| `CockroachDbBulkCopy`                      | Efficiently bulk-loads a `DbDataReader`/`IDataReader`, `DataTable`, or `DataRow[]` into a CockroachDB table    |
| `CockroachDbBulkColumnMapping`             | Defines the mapping between a source column and a destination column                                          |
| `CockroachDbBulkCopyColumnMappingCollection` | The collection of `CockroachDbBulkColumnMapping` objects exposed by `CockroachDbBulkCopy.ColumnMappings`  |

### CockroachDbBulkCopy

`CockroachDbBulkCopy` streams rows directly into a `COPY "table" (...) FROM STDIN (FORMAT BINARY)` operation via `NpgsqlBinaryImporter`, resolving any name-based column mapping (source column name, destination column ordinal) against the source schema and the destination table's `information_schema.columns` output before writing:

```csharp
await using var connection =
    new CockroachDbConnection(connectionString);

await connection.OpenAsync();

using var bulkCopy = new CockroachDbBulkCopy(connection)
{
    DestinationTableName = "Customer",
    BulkCopyTimeout = 60
};

bulkCopy.ColumnMappings.Add("Id", "Id");
bulkCopy.ColumnMappings.Add("Name", "Name");
bulkCopy.ColumnMappings.Add("Email", "Email");

await bulkCopy.WriteToServerAsync(customersDataTable);

Console.WriteLine(bulkCopy.RowsCopied);
```

`WriteToServer`/`WriteToServerAsync` are overloaded to accept an `IDataReader`, a `DbDataReader`, a `DataTable` (optionally filtered by `DataRowState`), or a `DataRow[]`.

> Unlike SQL Server's `SqlBulkCopy` or MariaDB Connector's `MySqlBulkCopy`, CockroachDB has no native bulk-copy class with the same shape - `COPY ... FROM STDIN (FORMAT BINARY)` is the closest equivalent high-performance loading mechanism, so `CockroachDbBulkCopy` hand-rolls the row/column streaming over `NpgsqlBinaryImporter` while keeping the familiar `SqlBulkCopy`-style column-mapping API on the surface.

## Architecture

RepoDb.Connector.CockroachDb is more than a set of ADO.NET wrapper classes. The public ADO.NET API sits on top of the CockroachDB communication and protocol infrastructure provided by Npgsql.

```text
Application / ORM
       │
       ▼
CockroachDbConnection
       │
       ▼
CockroachDbCommand
       │
       ▼
CockroachDB Session
       │
       ├── Authentication
       ├── TLS
       ├── Prepared Statements
       ├── Parameter Encoding
       ├── Result Set Parsing
       ├── Type Encoding/Decoding
       └── Cancellation
               │
               ▼
        PostgreSQL Wire Protocol
               │
               ▼
              TCP
               │
               ▼
        CockroachDB Cluster
```

## Local Development

A single-node, insecure CockroachDB cluster for local development and the integration tests is defined in the repository's `docker-compose.yml` (using the official `cockroachdb/cockroach` image):

```bash
docker compose up -d cockroachdb
```

The SQL endpoint is then available at `127.0.0.1:26257` (user `root`, no password) and the DB Console at `http://127.0.0.1:8081`. The integration tests read their connection strings from the `REPODB_COCKROACHDB_CONSTR_SYSTEM` and `REPODB_COCKROACHDB_CONSTR` environment variables, falling back to that local cluster.

## Roadmap

The initial development focuses on the essential ADO.NET provider infrastructure:

1. `CockroachDbConnection`
2. `CockroachDbCommand`
3. `CockroachDbParameter`
4. `CockroachDbParameterCollection`
5. `CockroachDbDataReader`
6. `CockroachDbTransaction`
7. `CockroachDbException`
8. `CockroachDbConnectionStringBuilder`
9. `CockroachDbFactory`
10. `CockroachDbType` and `CockroachDbTypeConverter`
11. `CockroachDbBulkCopy`, `CockroachDbBulkColumnMapping`, and `CockroachDbBulkCopyColumnMappingCollection`

Subsequent development may include:

* `CockroachDbDataSource` / `CockroachDbDataSourceBuilder`
* `CockroachDbBatch` / `CockroachDbBatchCommand`
* Built-in transaction retry helpers (SQLSTATE `40001`)
* Array, `ENUM` and `VECTOR` type mappings
* Connection pooling configuration
* Advanced server metadata
* `CockroachDbDataAdapter`
* `CockroachDbCommandBuilder`
* Performance optimizations

## ORM and Library Integration

Although RepoDb.Connector.CockroachDb can be used directly through ADO.NET, it is designed to work naturally with libraries that operate against the standard `System.Data.Common` abstractions.

For example:

```text
RepoDB
Dapper
Custom Data Access Layers
ADO.NET Applications
Other DbConnection-based Libraries
          │
          ▼
   RepoDb.Connector.CockroachDb
          │
          ▼
      CockroachDB Cluster
```

The connector itself should remain independent of any ORM.

## Contributing

RepoDb.Connector.CockroachDb is in its early stages, and contributions are welcome.

Areas where contributions will be particularly valuable include:

* Type mappings
* Transaction retry handling
* Authentication and TLS/SSL
* Connection pooling
* Async I/O
* CockroachDB version compatibility
* Performance benchmarking
* Bulk operations
* Integration and compatibility testing

When contributing, please keep the implementation aligned with the standard ADO.NET architecture and avoid unnecessary abstractions that could negatively affect performance.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the release history of this connector.

## License

RepoDb.Connector.CockroachDb is an independent open-source project. CockroachDB is a trademark of Cockroach Labs, Inc. This project is not affiliated with, sponsored by, or endorsed by Cockroach Labs, Inc.

[Apache License 2.0](https://apache.org/licenses/LICENSE-2.0.html) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
