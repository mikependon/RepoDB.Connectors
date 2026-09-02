<div align="center">
    <a href="https://www.nuget.org/packages/RepoDb.Connector.EnterpriseDb">
        <image src="logo.png" style="width:256px;" />
    </a>
    <br/>
    <span style="font-size:16px;font-weight:bold;">A lightweight, modern, and open-source ADO.NET data provider for EnterpriseDB, built for RepoDB.</span>
</div>

-----

> **Disclaimer**: This is an independent, unofficial .NET provider for EnterpriseDB. It is a thin ADO.NET wrapper and type-mapping layer built on top of [Npgsql](https://www.nuget.org/packages/Npgsql) and is not affiliated with or endorsed by EnterpriseDB Corporation.

The project aims to provide a dedicated EnterpriseDB connector based on the standard `System.Data.Common` abstractions, while exposing PostgreSQL/EnterpriseDB-specific data types, behaviors, and capabilities where applicable. All objects follow the `EDB`-prefixed naming convention used by EnterpriseDB's own [EnterpriseDB.EDBClient](https://www.nuget.org/packages/EnterpriseDB.EDBClient) driver, so that code written against this connector reads the same way it would against the official client.

It implements the `Async` equivalent of the [Npgsql](https://www.nuget.org/packages/Npgsql) library that is dedicated for EnterpriseDB/PostgreSQL. It also covers the full implementation of Bulk operations using Npgsql's binary `COPY` protocol (`NpgsqlBinaryImporter`).

> **Status:** Early development. The API and implementation are subject to change.

## Why is this exists?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated EnterpriseDB objects are required within its extension library so consumers who need `EDB`-prefixed types (mirroring EnterpriseDB's own client naming) can do so without taking a dependency on the commercially licensed `EnterpriseDB.EDBClient` package - this connector wraps the freely available, open-source [Npgsql](https://www.nuget.org/packages/Npgsql) driver instead.

This library will serve as the **official EnterpriseDB connector for RepoDB** and will be used internally by the [RepoDB project](https://github.com/mikependon/RepoDB).

## Goals of the library

RepoDb.Connector.EnterpriseDb aims to:

* Provide a dedicated ADO.NET data provider for EnterpriseDB.
* Follow the standard `System.Data.Common` provider architecture.
* Support modern synchronous and asynchronous .NET APIs.
* Provide EnterpriseDB-specific data type mappings and behaviors.
* Remain lightweight and suitable for use by ORMs and other data-access libraries.
* Support high-performance EnterpriseDB operations, including bulk operations, in future releases.
* Remain usable independently of any ORM.

## Core ADO.NET Objects

RepoDb.Connector.EnterpriseDb is built around the standard abstractions provided by `System.Data.Common`.

The following provider-specific objects form the core of the connector:

| RepoDb.Connector.EnterpriseDb    | ADO.NET Base Class          | Purpose                                       |
| --------------------------------- | --------------------------- | ---------------------------------------------- |
| `EDBConnection`                   | `DbConnection`              | Establishes and manages EnterpriseDB connections |
| `EDBCommand`                      | `DbCommand`                 | Executes SQL commands                          |
| `EDBDataReader`                   | `DbDataReader`               | Reads query results                            |
| `EDBParameter`                    | `DbParameter`                | Represents command parameters                  |
| `EDBParameterCollection`          | `DbParameterCollection`      | Manages command parameters                     |
| `EDBTransaction`                  | `DbTransaction`               | Manages database transactions                  |
| `EDBException`                    | `DbException`                | Represents EnterpriseDB errors                 |
| `EDBConnectionStringBuilder`      | `DbConnectionStringBuilder`   | Builds and parses connection strings           |
| `EDBFactory`                      | `DbProviderFactory`           | Creates provider-specific ADO.NET objects      |

The architecture follows the standard ADO.NET provider model:

```text
System.Data.Common
│
├── DbConnection
│     └── EDBConnection
│
├── DbCommand
│     └── EDBCommand
│
├── DbDataReader
│     └── EDBDataReader
│
├── DbParameter
│     └── EDBParameter
│
├── DbParameterCollection
│     └── EDBParameterCollection
│
├── DbTransaction
│     └── EDBTransaction
│
├── DbException
│     └── EDBException
│
├── DbConnectionStringBuilder
│     └── EDBConnectionStringBuilder
│
└── DbProviderFactory
      └── EDBFactory
```

## Basic Usage

RepoDb.Connector.EnterpriseDb is intended to provide the familiar ADO.NET programming model.

```csharp
using RepoDb.Connector.EnterpriseDb;

var connectionString =
    "Host=localhost;" +
    "Port=5432;" +
    "Database=TestDb;" +
    "Username=postgres;" +
    "Password=password;";

await using var connection =
    new EDBConnection(connectionString);

await connection.OpenAsync();

await using var command = connection.CreateCommand();

command.CommandText = """
    SELECT "Id", "Name", "Email"
    FROM "Customer"
    WHERE "Id" = @Id;
    """;

command.Parameters.AddWithValue("@Id", 100);

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    var id = reader.GetInt32(0);
    var name = reader.GetString(1);
    var email = reader.GetString(2);

    Console.WriteLine($"{id}: {name} ({email})");
}
```

## EDBConnection

`EDBConnection` extends `DbConnection` and represents a connection to an EnterpriseDB server.

```csharp
await using var connection =
    new EDBConnection(connectionString);

await connection.OpenAsync();

Console.WriteLine(connection.ServerVersion);
Console.WriteLine(connection.Database);
Console.WriteLine(connection.State);
```

Its responsibilities include:

* Connection establishment and termination
* Connection state management
* EnterpriseDB session management
* Command creation
* Transaction creation
* Connection string handling
* Synchronous and asynchronous operations

## EDBCommand

`EDBCommand` extends `DbCommand` and represents a SQL statement executed against EnterpriseDB.

```csharp
await using var command = new EDBCommand(
    "SELECT * FROM \"Customer\" WHERE \"Id\" = @Id",
    connection);

command.Parameters.AddWithValue("@Id", 100);

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

## EDBParameter

`EDBParameter` extends `DbParameter` and represents a parameter associated with a `EDBCommand`.

```csharp
var parameter = new EDBParameter
{
    ParameterName = "@Id",
    Value = 100
};

command.Parameters.Add(parameter);
```

An EnterpriseDB-specific type system is also planned:

```csharp
var parameter = new EDBParameter
{
    ParameterName = "@Id",
    EDBType = EDBType.Integer,
    Value = 100
};
```

## EnterpriseDB Data Types

RepoDb.Connector.EnterpriseDb aims to provide an `EDBType` enumeration in addition to the standard ADO.NET `DbType`.

The current set of types includes:

```csharp
public enum EDBType
{
    SmallInt,
    Integer,
    BigInt,
    Decimal,
    Real,
    Double,
    Money,
    Boolean,

    Char,
    VarChar,
    Text,
    Name,
    Citext,

    Bytea,

    Date,
    Time,
    TimeTz,
    Timestamp,
    TimestampTz,
    Interval,

    Inet,
    Cidr,
    MacAddr,
    MacAddr8,

    Bit,
    VarBit,

    Json,
    Jsonb,

    Uuid,
    Xml,
    Hstore,

    TsVector,
    TsQuery,

    Point,
    Line,
    LSeg,
    Box,
    Path,
    Polygon,
    Circle
}
```

The connector will provide mappings between:

```text
EDBType
     ↕
DbType
     ↕
.NET CLR Type
     ↕
EnterpriseDB Server Type
```

`EDBTypeConverter` provides the current leg of that mapping, converting between `EDBType` and the underlying `NpgsqlTypes.NpgsqlDbType`:

```csharp
var edbType = EDBTypeConverter.ToEDBType(NpgsqlDbType.Varchar);
var npgsqlDbType = EDBTypeConverter.ToNpgsqlDbType(EDBType.BigInt);
```

## Transactions

`EDBTransaction` extends `DbTransaction` and provides standard ADO.NET transaction semantics.

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
    command.Parameters.AddWithValue("@Id", 100);

    await command.ExecuteNonQueryAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

## Connection String Builder

`EDBConnectionStringBuilder` extends `DbConnectionStringBuilder` and provides a strongly typed way of creating EnterpriseDB connection strings.

```csharp
var builder = new EDBConnectionStringBuilder
{
    Host = "localhost",
    Port = 5432,
    Database = "TestDb",
    Username = "postgres",
    Password = "password"
};

await using var connection =
    new EDBConnection(builder.ConnectionString);

await connection.OpenAsync();
```

## Provider Factory

`EDBFactory` extends `DbProviderFactory` and enables provider-independent ADO.NET applications and libraries to create RepoDb.Connector.EnterpriseDb objects.

```csharp
var factory = EDBFactory.Instance;

using var connection = factory.CreateConnection();

connection.ConnectionString = connectionString;
connection.Open();
```

## Additional ADO.NET Support

Future releases may provide additional traditional ADO.NET components:

```text
EDBDataAdapter
    └── DbDataAdapter

EDBCommandBuilder
    └── DbCommandBuilder

EDBDataSource / EDBDataSourceBuilder
    └── DbDataSource
```

These components will provide support for `DataTable`, `DataSet`, connection pooling configuration, and other traditional ADO.NET workflows.

The initial development effort will prioritize the core connection, command, parameter, transaction, and data-reader infrastructure.

## Bulk Operations

RepoDb.Connector.EnterpriseDb provides bulk-loading support under the `RepoDb.Connector.EnterpriseDb.Bulk` namespace, built directly on top of Npgsql's binary `COPY` protocol (`NpgsqlBinaryImporter`, opened via `NpgsqlConnection.BeginBinaryImport`).

| RepoDb.Connector.EnterpriseDb.Bulk       | Purpose                                                                                                    |
| ------------------------------------------ | ----------------------------------------------------------------------------------------------------------- |
| `EDBBulkCopy`                              | Efficiently bulk-loads a `DbDataReader`/`IDataReader`, `DataTable`, or `DataRow[]` into an EnterpriseDB table |
| `EDBBulkColumnMapping`                     | Defines the mapping between a source column and a destination column                                        |
| `EDBBulkCopyColumnMappingCollection`       | The collection of `EDBBulkColumnMapping` objects exposed by `EDBBulkCopy.ColumnMappings`                     |

### EDBBulkCopy

`EDBBulkCopy` streams rows directly into a `COPY "table" (...) FROM STDIN (FORMAT BINARY)` operation via `NpgsqlBinaryImporter`, resolving any name-based column mapping (source column name, destination column ordinal) against the source schema and the destination table's `information_schema.columns` output before writing:

```csharp
await using var connection =
    new EDBConnection(connectionString);

await connection.OpenAsync();

using var bulkCopy = new EDBBulkCopy(connection)
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

> Unlike SQL Server's `SqlBulkCopy` or MariaDB Connector's `MySqlBulkCopy`, PostgreSQL/EnterpriseDB has no native bulk-copy class with the same shape - `COPY ... FROM STDIN (FORMAT BINARY)` is the closest equivalent high-performance loading mechanism, so `EDBBulkCopy` hand-rolls the row/column streaming over `NpgsqlBinaryImporter` while keeping the familiar `SqlBulkCopy`-style column-mapping API on the surface.

## Architecture

RepoDb.Connector.EnterpriseDb is more than a set of ADO.NET wrapper classes. The public ADO.NET API will sit on top of the EnterpriseDB communication and protocol infrastructure.

```text
Application / ORM
       │
       ▼
EDBConnection
       │
       ▼
EDBCommand
       │
       ▼
EnterpriseDB Session
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
        EnterpriseDB Server
```

## Roadmap

The initial development will focus on the essential ADO.NET provider infrastructure:

1. `EDBConnection`
2. `EDBCommand`
3. `EDBParameter`
4. `EDBParameterCollection`
5. `EDBDataReader`
6. `EDBTransaction`
7. `EDBException`
8. `EDBConnectionStringBuilder`
9. `EDBFactory`
10. `EDBType` and `EDBTypeConverter`
11. `EDBBulkCopy`, `EDBBulkColumnMapping`, and `EDBBulkCopyColumnMappingCollection`

Subsequent development may include:

* `EDBDataSource` / `EDBDataSourceBuilder`
* `EDBBatch` / `EDBBatchCommand`
* Connection pooling configuration
* Prepared statements
* TLS/SSL
* Authentication mechanisms
* `LISTEN`/`NOTIFY` support (`EDBNotificationEventArgs`)
* Advanced server metadata
* `EDBDataAdapter`
* `EDBCommandBuilder`
* Performance optimizations

## ORM and Library Integration

Although RepoDb.Connector.EnterpriseDb can be used directly through ADO.NET, it is designed to work naturally with libraries that operate against the standard `System.Data.Common` abstractions.

For example:

```text
RepoDB
Dapper
Custom Data Access Layers
ADO.NET Applications
Other DbConnection-based Libraries
          │
          ▼
   RepoDb.Connector.EnterpriseDb
          │
          ▼
      EnterpriseDB Server
```

The connector itself should remain independent of any ORM.

## Contributing

RepoDb.Connector.EnterpriseDb is in its early stages, and contributions are welcome.

Areas where contributions will be particularly valuable include:

* PostgreSQL/EnterpriseDB wire protocol implementation
* Authentication
* Type mappings
* Prepared statements
* Connection pooling
* TLS/SSL
* Async I/O
* EnterpriseDB version compatibility
* Performance benchmarking
* Bulk operations
* Integration and compatibility testing

When contributing, please keep the implementation aligned with the standard ADO.NET architecture and avoid unnecessary abstractions that could negatively affect performance.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the release history of every connector in this repository.

## License

RepoDb.Connector.EnterpriseDb is an independent open-source project. EnterpriseDB is a trademark of EnterpriseDB Corporation. This project is not affiliated with, sponsored by, or endorsed by EnterpriseDB Corporation.

[Apache License 2.0](https://apache.org/licenses/LICENSE-2.0.html) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon) 
