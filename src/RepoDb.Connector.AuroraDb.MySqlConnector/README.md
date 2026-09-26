<div align="center">
    <a href="https://www.nuget.org/packages/RepoDb.Connector.AuroraDb.MySqlConnector">
        <image src="logo.png" style="width:256px;" />
    </a>
    <br/>
    <span style="font-size:16px;font-weight:bold;">A lightweight, modern, and open-source ADO.NET data provider for Amazon Aurora MySQL, built for RepoDB on top of the AWS Advanced .NET Data Provider Wrapper.</span>
</div>

-----

> **Note**: This is the Aurora **MySQL** flavor of the connector. The Aurora PostgreSQL flavor lives in [RepoDb.Connector.AuroraDb.Npgsql](../RepoDb.Connector.AuroraDb.Npgsql).

> **Disclaimer**: This is an independent, unofficial .NET provider for Amazon Aurora. It is a thin ADO.NET wrapper and type-mapping layer built on top of the [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) and [MySqlConnector](https://www.nuget.org/packages/MySqlConnector), and is not affiliated with or endorsed by Amazon Web Services, Inc.

The project aims to provide a dedicated Aurora connector based on the standard `System.Data.Common` abstractions. All objects follow the `AuroraDb`-prefixed naming convention, so that they never collide with the `MySql`-prefixed objects of the underlying driver (or of any other MySQL-compatible connector) within the same application.

## Powered by the AWS Advanced .NET Data Provider Wrapper

This connector **does not implement a new wire protocol and does not replace the AWS wrapper - it leverages it**. Every `AuroraDbConnection` is backed by an `AwsWrapperConnection<MySqlConnection>` from the [AWS.AdvancedDotnetDataProviderWrapper.Core](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Core) package, using the [AWS.AdvancedDotnetDataProviderWrapper.Dialect.MySqlConnector](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Dialect.MySqlConnector) dialect (registered automatically by the connector), on top of [MySqlConnector](https://www.nuget.org/packages/MySqlConnector).

That means all the capabilities implemented by the AWS wrapper are available to `RepoDb.Connector.AuroraDb.MySqlConnector` consumers, driven by the connection string:

* **Failover** - awareness of the Aurora cluster topology, with fast recovery to the newly elected primary instance.
* **Enhanced Failure Monitoring (EFM)** - faster detection of an unhealthy database instance than a network/connection timeout.
* **Read/Write Splitting** - routing of read workloads to the Aurora Replicas.
* **IAM Authentication** - connecting with short-lived IAM tokens instead of a password.
* **AWS Secrets Manager** - fetching the database credentials from Secrets Manager.
* **Other AWS wrapper plugins** - custom endpoints, Blue/Green deployments, Aurora Limitless, telemetry, and so on.

It also works with plain RDS MySQL and self-managed MySQL databases.

```text
Application / RepoDB
        │
        ▼
RepoDb.Connector.AuroraDb.MySqlConnector
        │   AuroraDbConnection, AuroraDbCommand, AuroraDbParameter, ...
        ▼
AWS Advanced .NET Data Provider Wrapper (AwsWrapperConnection<MySqlConnection>)
        │   Plugins: Failover, EFM, Read/Write Splitting, IAM, Secrets Manager, ...
        ▼
MySqlConnector (Dialect.MySqlConnector)
        │
        ▼
Aurora MySQL cluster
```

> **Status:** Early development. The API and implementation are subject to change.

## Why is this exists?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated Aurora objects are required within its extension library to avoid class collisions with the other MySQL-compatible providers (MySQL, MariaDB, ...) and to expose the AWS wrapper capabilities to RepoDB through the standard ADO.NET surface.

This library will serve as the **official Aurora connector for RepoDB** and will be used internally by the [RepoDB project](https://github.com/mikependon/RepoDB).

## Goals of the library

RepoDb.Connector.AuroraDb.MySqlConnector aims to:

* Provide a dedicated ADO.NET data provider for Amazon Aurora MySQL.
* Leverage the AWS Advanced .NET Data Provider Wrapper, preserving its capabilities rather than replacing them.
* Follow the standard `System.Data.Common` provider architecture.
* Support modern synchronous and asynchronous .NET APIs.
* Provide Aurora MySQL-specific data type mappings and behaviors.
* Remain lightweight and suitable for use by ORMs and other data-access libraries.
* Support high-performance operations, including bulk operations.
* Remain usable independently of any ORM.

## Core ADO.NET Objects

RepoDb.Connector.AuroraDb.MySqlConnector is built around the standard abstractions provided by `System.Data.Common`.

The following provider-specific objects form the core of the connector:

| RepoDb.Connector.AuroraDb.MySqlConnector        | ADO.NET Base Class          | Purpose                                        |
| ------------------------------------ | --------------------------- | ---------------------------------------------- |
| `AuroraDbConnection`             | `DbConnection`              | Establishes and manages Aurora connections (backed by `AwsWrapperConnection`) |
| `AuroraDbCommand`                | `DbCommand`                 | Executes SQL commands                          |
| `AuroraDbDataReader`             | `DbDataReader`              | Reads query results                            |
| `AuroraDbParameter`              | `DbParameter`               | Represents command parameters                  |
| `AuroraDbParameterCollection`    | `DbParameterCollection`     | Manages command parameters                     |
| `AuroraDbTransaction`            | `DbTransaction`             | Manages database transactions                  |
| `AuroraDbException`              | `DbException`               | Represents Aurora errors                       |
| `AuroraDbConnectionStringBuilder`| `DbConnectionStringBuilder` | Builds and parses connection strings (including the AWS wrapper properties) |
| `AuroraDbFactory`                | `DbProviderFactory`         | Creates provider-specific ADO.NET objects      |

The architecture follows the standard ADO.NET provider model:

```text
System.Data.Common
│
├── DbConnection
│     └── AuroraDbConnection
│
├── DbCommand
│     └── AuroraDbCommand
│
├── DbDataReader
│     └── AuroraDbDataReader
│
├── DbParameter
│     └── AuroraDbParameter
│
├── DbParameterCollection
│     └── AuroraDbParameterCollection
│
├── DbTransaction
│     └── AuroraDbTransaction
│
├── DbException
│     └── AuroraDbException
│
├── DbConnectionStringBuilder
│     └── AuroraDbConnectionStringBuilder
│
└── DbProviderFactory
      └── AuroraDbFactory
```

## Basic Usage

RepoDb.Connector.AuroraDb.MySqlConnector is intended to provide the familiar ADO.NET programming model.

```csharp
using RepoDb.Connector.AuroraDb.MySqlConnector;

var connectionString =
    "Server=my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com;" +
    "Port=3306;" +
    "Database=mysql;" +
    "User ID=admin;" +
    "Password=<password>;";

await using var connection =
    new AuroraDbConnection(connectionString);

await connection.OpenAsync();

await using var command = connection.CreateCommand();

command.CommandText = """
    SELECT `Id`, `Name`, `Email`
    FROM `Customer`
    WHERE `Id` = @Id;
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

## Using the AWS wrapper capabilities

The AWS wrapper plugins are enabled through the connection string. Both the [AWS wrapper properties](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper/blob/main/docs/using-the-dotnet-driver/UsingTheDotNetDriver.md) and the MySqlConnector properties can be mixed in the same connection string; the wrapper consumes its own and passes the remaining ones to MySqlConnector.

```csharp
var builder = new AuroraDbConnectionStringBuilder
{
    Server = "my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com",
    Port = 3306,
    Database = "mysql",
    UserId = "admin",
    Password = "<password>",
    Plugins = "failover,efm"   // AWS wrapper plugins
};

await using var connection =
    new AuroraDbConnection(builder.ConnectionString);

await connection.OpenAsync();
```

For scenarios that cannot be represented by the standard ADO.NET surface, the underlying `AwsWrapperConnection` is exposed through `AuroraDbConnection.WrappedConnection`:

```csharp
AwsWrapperConnection wrapper = connection.WrappedConnection;

MySqlConnection mySql = wrapper.Unwrap<MySqlConnection>();
```

### Failover exceptions

When a failover happens, the AWS wrapper raises its own exceptions (e.g. `FailoverSuccessException`, `FailoverFailedException`, `TransactionStateUnknownException`) so that the application can decide how to proceed (typically by retrying the operation or the transaction). These are **not** wrapped in `AuroraDbException`; they are surfaced as-is so that they can be handled exactly as documented by the AWS wrapper.

Errors raised by the underlying `MySqlConnector` provider and by the wrapper (`AwsWrapperDbException`) are surfaced as `AuroraDbException`, carrying the original exception as `InnerException` and exposing its `SqlState`.

## AuroraDbConnection

`AuroraDbConnection` extends `DbConnection` and represents a connection to an Aurora MySQL cluster.

```csharp
await using var connection =
    new AuroraDbConnection(connectionString);

await connection.OpenAsync();

Console.WriteLine(connection.ServerVersion);
Console.WriteLine(connection.Database);
Console.WriteLine(connection.State);
```

Its responsibilities include:

* Connection establishment and termination
* Connection state management
* Command creation
* Transaction creation
* Connection string handling
* Synchronous and asynchronous operations
* Access to the underlying AWS wrapper (`WrappedConnection`)

## AuroraDbCommand

`AuroraDbCommand` extends `DbCommand` and represents a SQL statement executed against Aurora. It delegates the execution to the command of the wrapped AWS connection.

```csharp
await using var command = new AuroraDbCommand(
    "SELECT * FROM `Customer` WHERE `Id` = @Id",
    connection);

command.Parameters.AddWithValue("@Id", 100L);

await using var reader =
    await command.ExecuteReaderAsync();
```

The implementation supports:

* `ExecuteNonQuery`
* `ExecuteScalar`
* `ExecuteReader`
* Async equivalents
* Parameterized SQL
* Prepared statements
* Command timeout
* Cancellation

Executing a command that has no connection, or whose connection is not open, throws an `InvalidOperationException`.

## AuroraDbParameter

`AuroraDbParameter` extends `DbParameter` and represents a parameter associated with a `AuroraDbCommand`.

```csharp
var parameter = new AuroraDbParameter
{
    ParameterName = "@Id",
    Value = 100L
};

command.Parameters.Add(parameter);
```

The native Aurora MySQL type of a parameter can be set explicitly via the `AuroraDbType` property:

```csharp
var parameter = new AuroraDbParameter
{
    ParameterName = "@Document",
    AuroraDbType = AuroraDbType.Jsonb,
    Value = "{\"name\": \"RepoDB\"}"
};
```

## Aurora MySQL Data Types

RepoDb.Connector.AuroraDb.MySqlConnector provides an `AuroraDbType` enumeration in addition to the standard ADO.NET `DbType`. It mirrors the MySQL type system:

| Category        | AuroraDbType                                                                                                             |
| --------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Numeric         | `TinyInt`, `SmallInt`, `MediumInt`, `Int`, `BigInt`, `Decimal`, `Float`, `Double`, `Bit`                                 |
| String          | `Char`, `VarChar`, `TinyText`, `Text`, `MediumText`, `LongText`, `Enum`, `Set`                                           |
| Binary          | `Binary`, `VarBinary`, `TinyBlob`, `Blob`, `MediumBlob`, `LongBlob`                                                      |
| Date and Time   | `Date`, `Time`, `DateTime`, `Timestamp`, `Year`                                                                          |
| JSON            | `Json`                                                                                                                   |
| Spatial         | `Geometry`, `Point`, `LineString`, `Polygon`, `MultiPoint`, `MultiLineString`, `MultiPolygon`, `GeometryCollection`      |

`AuroraDbTypeConverter` converts between `AuroraDbType` and the underlying `MySqlConnector.MySqlDbType`:

```csharp
var auroraDbType = AuroraDbTypeConverter.ToAuroraDbType(MySqlDbType.VarChar);
var mySqlDbType = AuroraDbTypeConverter.ToMySqlDbType(AuroraDbType.BigInt);
```

Notes:

* Converting a `MySqlDbType` that has no `AuroraDbType` counterpart (e.g. `Guid`, `UByte`, `VarString`, ...) throws a `NotSupportedException`.
* Unlike MariaDB, `JSON` is a native type in Aurora MySQL, and there is no native `UUID` type (use `CHAR(36)` or `BINARY(16)`).
* The MySQL protocol reports coarse data type names when reading results: the `TEXT` family is reported as `VARCHAR`, the `BINARY`/`BLOB` families as `BLOB`, and every spatial type as `GEOMETRY`.

## Transactions

`AuroraDbTransaction` extends `DbTransaction` and provides standard ADO.NET transaction semantics.

```csharp
await using var transaction =
    await connection.BeginTransactionAsync();

try
{
    await using var command = connection.CreateCommand();

    command.Transaction = (AuroraDbTransaction)transaction;

    command.CommandText =
        "UPDATE `Customer` SET `Name` = @Name WHERE `Id` = @Id";

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

A transaction that has already been committed or rolled back throws an `InvalidOperationException` when it is completed again, and beginning a transaction on a closed connection throws an `InvalidOperationException`.

## Connection String Builder

`AuroraDbConnectionStringBuilder` extends `DbConnectionStringBuilder` and provides a strongly typed way of creating Aurora connection strings. It is backed by the AWS wrapper's connection string builder, so it understands the AWS wrapper properties (e.g. `Plugins`) besides the usual `Server`, `Port`, `Database`, `UserId` and `Password`.

```csharp
var builder = new AuroraDbConnectionStringBuilder
{
    Server = "my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com",
    Port = 3306,
    Database = "mysql",
    UserId = "admin",
    Plugins = "failover"
};

await using var connection =
    new AuroraDbConnection(builder.ConnectionString);

await connection.OpenAsync();
```

## Provider Factory

`AuroraDbFactory` extends `DbProviderFactory` and enables provider-independent ADO.NET applications and libraries to create RepoDb.Connector.AuroraDb.MySqlConnector objects.

```csharp
var factory = AuroraDbFactory.Instance;

using var connection = factory.CreateConnection();

connection.ConnectionString = connectionString;
connection.Open();
```

## Bulk Operations

RepoDb.Connector.AuroraDb.MySqlConnector provides bulk-loading support under the `RepoDb.Connector.AuroraDb.MySqlConnector.Bulk` namespace, built on top of MySqlConnector's `MySqlBulkCopy`.

| RepoDb.Connector.AuroraDb.MySqlConnector.Bulk | Purpose                                                                                                     |
| --------------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `AuroraDbBulkCopy`                            | Efficiently bulk-loads a `DbDataReader`/`IDataReader`, `DataTable`, or `DataRow[]` into an Aurora MySQL table |
| `AuroraDbBulkColumnMapping`                   | Defines the mapping between a source column and a destination column                                        |
| `AuroraDbBulkCopyColumnMappingCollection`     | The collection of `AuroraDbBulkColumnMapping` objects exposed by `AuroraDbBulkCopy.ColumnMappings`          |

### AuroraDbBulkCopy

`AuroraDbBulkCopy` wraps a `MySqlConnector.MySqlBulkCopy` internally, resolving any name-based column mapping (source column name, destination column ordinal) against the source schema and the destination table's `SHOW COLUMNS` output before delegating the actual write:

```csharp
await using var connection =
    new AuroraDbConnection(connectionString);

await connection.OpenAsync();

using var bulkCopy = new AuroraDbBulkCopy(connection)
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

> `MySqlBulkCopy` is MySqlConnector specific, so `AuroraDbBulkCopy` unwraps the currently active `MySqlConnection` of the AWS wrapper. The bulk-copy stream itself therefore runs directly on the MySqlConnector connection. It relies on `LOAD DATA LOCAL INFILE`, which requires `AllowLoadLocalInfile=True` in the connection string and `local_infile` enabled on the server (on Aurora MySQL, through the DB cluster parameter group).

## Architecture

RepoDb.Connector.AuroraDb.MySqlConnector is more than a set of ADO.NET wrapper classes. The public ADO.NET API sits on top of the AWS Advanced .NET Data Provider Wrapper, which in turn sits on top of the communication and protocol infrastructure provided by MySqlConnector.

```text
Application / ORM
       │
       ▼
AuroraDbConnection
       │
       ▼
AuroraDbCommand
       │
       ▼
AwsWrapperConnection / AwsWrapperCommand (AWS Advanced .NET Data Provider Wrapper)
       │
       ├── Failover
       ├── Enhanced Failure Monitoring
       ├── Read/Write Splitting
       ├── IAM Authentication
       └── Secrets Manager
               │
               ▼
        MySqlConnector (MySqlConnection / MySqlCommand)
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
                MySQL Wire Protocol
                       │
                       ▼
                      TCP
                       │
                       ▼
             Aurora MySQL Cluster
```

## Local Development

A MySQL 8.0 server (the engine Aurora MySQL 3 is compatible with) for local development and the integration tests is defined in the repository's `docker-compose.yml`:

```bash
docker compose up -d auroradbmysql
```

The server is then available at `127.0.0.1:3308` (user `root`, password `RepoDB2026`). The integration tests read their connection strings from the `REPODB_AURORADB_MYSQL_CONSTR_SYSTEM` and `REPODB_AURORADB_MYSQL_CONSTR` environment variables, falling back to that local server.

> The local server is a plain MySQL, not an Aurora cluster, so the Aurora-specific AWS wrapper features (topology-based failover, read/write splitting, ...) are not exercised by the integration tests.

## Roadmap

The initial development focuses on the essential ADO.NET provider infrastructure:

1. `AuroraDbConnection`
2. `AuroraDbCommand`
3. `AuroraDbParameter`
4. `AuroraDbParameterCollection`
5. `AuroraDbDataReader`
6. `AuroraDbTransaction`
7. `AuroraDbException`
8. `AuroraDbConnectionStringBuilder`
9. `AuroraDbFactory`
10. `AuroraDbType` and `AuroraDbTypeConverter`
11. `AuroraDbBulkCopy`, `AuroraDbBulkColumnMapping`, and `AuroraDbBulkCopyColumnMappingCollection`

Subsequent development may include:

* `AuroraDbCommandBuilder`
* `AuroraDbDataAdapter`
* `AuroraDbDataSource` / `AuroraDbDataSourceBuilder`
* `AuroraDbBatch` / `AuroraDbBatchCommand`
* Transaction savepoints
* Performance optimizations

## ORM and Library Integration

Although RepoDb.Connector.AuroraDb.MySqlConnector can be used directly through ADO.NET, it is designed to work naturally with libraries that operate against the standard `System.Data.Common` abstractions.

For example:

```text
RepoDB
Dapper
Custom Data Access Layers
ADO.NET Applications
Other DbConnection-based Libraries
          │
          ▼
   RepoDb.Connector.AuroraDb.MySqlConnector
          │
          ▼
AWS Advanced .NET Data Provider Wrapper
          │
          ▼
  Aurora MySQL Cluster
```

The connector itself should remain independent of any ORM.

## Contributing

RepoDb.Connector.AuroraDb.MySqlConnector is in its early stages, and contributions are welcome.

Areas where contributions will be particularly valuable include:

* Type mappings
* AWS wrapper plugin integration and testing (failover, EFM, IAM, Secrets Manager, ...)
* Authentication and TLS/SSL
* Connection pooling
* Async I/O
* Aurora MySQL version compatibility
* Performance benchmarking
* Bulk operations
* Integration and compatibility testing

When contributing, please keep the implementation aligned with the standard ADO.NET architecture and avoid unnecessary abstractions that could negatively affect performance.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the release history of this connector.

## License

RepoDb.Connector.AuroraDb.MySqlConnector is an independent open-source project. Amazon Aurora and AWS are trademarks of Amazon.com, Inc. or its affiliates. This project is not affiliated with, sponsored by, or endorsed by Amazon Web Services, Inc.

The [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) is licensed under the Apache License 2.0.

[Apache License 2.0](https://apache.org/licenses/LICENSE-2.0.html) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
