<div align="center">
    <a href="https://www.nuget.org/packages/RepoDb.Connector.AuroraDb">
        <image src="logo.png" style="width:256px;" />
    </a>
    <br/>
    <span style="font-size:16px;font-weight:bold;">A lightweight, modern, and open-source ADO.NET data provider for Amazon Aurora PostgreSQL, built for RepoDB on top of the AWS Advanced .NET Data Provider Wrapper.</span>
</div>

-----

> **Disclaimer**: This is an independent, unofficial .NET provider for Amazon Aurora. It is a thin ADO.NET wrapper and type-mapping layer built on top of the [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) and [Npgsql](https://www.nuget.org/packages/Npgsql), and is not affiliated with or endorsed by Amazon Web Services, Inc.

The project aims to provide a dedicated Aurora connector based on the standard `System.Data.Common` abstractions. All objects follow the `AuroraDb`-prefixed naming convention, so that they never collide with the `Npgsql`-prefixed objects of the underlying driver (or of any other PostgreSQL-compatible connector) within the same application.

## Powered by the AWS Advanced .NET Data Provider Wrapper

This connector **does not implement a new wire protocol and does not replace the AWS wrapper - it leverages it**. Every `AuroraDbConnection` is backed by an `AwsWrapperConnection<NpgsqlConnection>` from the [AWS.AdvancedDotnetDataProviderWrapper.Core](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Core) package, using the [AWS.AdvancedDotnetDataProviderWrapper.Dialect.Npgsql](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Dialect.Npgsql) dialect (registered automatically by the connector), on top of [Npgsql](https://www.nuget.org/packages/Npgsql).

That means all the capabilities implemented by the AWS wrapper are available to `RepoDb.Connector.AuroraDb` consumers, driven by the connection string:

* **Failover** - awareness of the Aurora cluster topology, with fast recovery to the newly elected primary instance.
* **Enhanced Failure Monitoring (EFM)** - faster detection of an unhealthy database instance than a network/connection timeout.
* **Read/Write Splitting** - routing of read workloads to the Aurora Replicas.
* **IAM Authentication** - connecting with short-lived IAM tokens instead of a password.
* **AWS Secrets Manager** - fetching the database credentials from Secrets Manager.
* **Other AWS wrapper plugins** - custom endpoints, Blue/Green deployments, Aurora Limitless, telemetry, and so on.

It also works with plain RDS PostgreSQL and self-managed PostgreSQL databases.

```text
Application / RepoDB
        │
        ▼
RepoDb.Connector.AuroraDb
        │   AuroraDbConnection, AuroraDbCommand, AuroraDbParameter, ...
        ▼
AWS Advanced .NET Data Provider Wrapper (AwsWrapperConnection<NpgsqlConnection>)
        │   Plugins: Failover, EFM, Read/Write Splitting, IAM, Secrets Manager, ...
        ▼
Npgsql (Dialect.Npgsql)
        │
        ▼
Aurora PostgreSQL cluster
```

> **Status:** Early development. The API and implementation are subject to change.

## Why is this exists?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated Aurora objects are required within its extension library to avoid class collisions with the other PostgreSQL-compatible providers (PostgreSQL, EnterpriseDB, CockroachDB, ...) and to expose the AWS wrapper capabilities to RepoDB through the standard ADO.NET surface.

This library will serve as the **official Aurora connector for RepoDB** and will be used internally by the [RepoDB project](https://github.com/mikependon/RepoDB).

## Goals of the library

RepoDb.Connector.AuroraDb aims to:

* Provide a dedicated ADO.NET data provider for Amazon Aurora PostgreSQL.
* Leverage the AWS Advanced .NET Data Provider Wrapper, preserving its capabilities rather than replacing them.
* Follow the standard `System.Data.Common` provider architecture.
* Support modern synchronous and asynchronous .NET APIs.
* Provide Aurora PostgreSQL-specific data type mappings and behaviors.
* Remain lightweight and suitable for use by ORMs and other data-access libraries.
* Support high-performance operations, including bulk operations.
* Remain usable independently of any ORM.

## Core ADO.NET Objects

RepoDb.Connector.AuroraDb is built around the standard abstractions provided by `System.Data.Common`.

The following provider-specific objects form the core of the connector:

| RepoDb.Connector.AuroraDb        | ADO.NET Base Class          | Purpose                                        |
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

RepoDb.Connector.AuroraDb is intended to provide the familiar ADO.NET programming model.

```csharp
using RepoDb.Connector.AuroraDb;

var connectionString =
    "Host=my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com;" +
    "Port=5432;" +
    "Database=postgres;" +
    "Username=postgres;" +
    "Password=<password>;";

await using var connection =
    new AuroraDbConnection(connectionString);

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

## Using the AWS wrapper capabilities

The AWS wrapper plugins are enabled through the connection string. Both the [AWS wrapper properties](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper/blob/main/docs/using-the-dotnet-driver/UsingTheDotNetDriver.md) and the Npgsql properties can be mixed in the same connection string; the wrapper consumes its own and passes the remaining ones to Npgsql.

```csharp
var builder = new AuroraDbConnectionStringBuilder
{
    Host = "my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com",
    Port = 5432,
    Database = "postgres",
    Username = "postgres",
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

NpgsqlConnection npgsql = wrapper.Unwrap<NpgsqlConnection>();
```

### Failover exceptions

When a failover happens, the AWS wrapper raises its own exceptions (e.g. `FailoverSuccessException`, `FailoverFailedException`, `TransactionStateUnknownException`) so that the application can decide how to proceed (typically by retrying the operation or the transaction). These are **not** wrapped in `AuroraDbException`; they are surfaced as-is so that they can be handled exactly as documented by the AWS wrapper.

Errors raised by the underlying `Npgsql` provider and by the wrapper (`AwsWrapperDbException`) are surfaced as `AuroraDbException`, carrying the original exception as `InnerException` and exposing its `SqlState`.

## AuroraDbConnection

`AuroraDbConnection` extends `DbConnection` and represents a connection to an Aurora PostgreSQL cluster.

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
    "SELECT * FROM \"Customer\" WHERE \"Id\" = @Id",
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

The native Aurora PostgreSQL type of a parameter can be set explicitly via the `AuroraDbType` property:

```csharp
var parameter = new AuroraDbParameter
{
    ParameterName = "@Document",
    AuroraDbType = AuroraDbType.Jsonb,
    Value = "{\"name\": \"RepoDB\"}"
};
```

## Aurora PostgreSQL Data Types

RepoDb.Connector.AuroraDb provides an `AuroraDbType` enumeration in addition to the standard ADO.NET `DbType`. Aurora PostgreSQL is PostgreSQL-compatible, so it mirrors the PostgreSQL type system:

| Category        | AuroraDbType                                                                                      |
| --------------- | ------------------------------------------------------------------------------------------------- |
| Numeric         | `SmallInt`, `Integer`, `BigInt`, `Decimal`, `Real`, `Double`, `Money`, `Boolean`                  |
| String          | `Char`, `VarChar`, `Text`, `Name`, `Citext`                                                       |
| Binary          | `Bytea`                                                                                           |
| Date and Time   | `Date`, `Time`, `TimeTz`, `Timestamp`, `TimestampTz`, `Interval`                                 |
| Network Address | `Inet`, `Cidr`, `MacAddr`, `MacAddr8`                                                             |
| Bit String      | `Bit`, `VarBit`                                                                                   |
| JSON and XML    | `Json`, `Jsonb`, `JsonPath`, `Xml`                                                                |
| Geometric       | `Point`, `Line`, `LSeg`, `Box`, `Path`, `Polygon`, `Circle`                                       |
| Range           | `IntegerRange`, `BigIntRange`, `NumericRange`, `TimestampRange`, `TimestampTzRange`, `DateRange` |
| Other           | `Uuid`, `Oid`, `Hstore`, `LTree`                                                                  |
| Spatial         | `Geometry`, `Geography` (PostGIS)                                                                 |
| Text Search     | `TsVector`, `TsQuery`                                                                             |

`AuroraDbTypeConverter` converts between `AuroraDbType` and the underlying `NpgsqlTypes.NpgsqlDbType`:

```csharp
var auroraDbType = AuroraDbTypeConverter.ToAuroraDbType(NpgsqlDbType.Varchar);
var npgsqlDbType = AuroraDbTypeConverter.ToNpgsqlDbType(AuroraDbType.BigInt);
```

Notes:

* Composite `NpgsqlDbType` values (arrays, the generic `Range`/`Multirange` flags, ...) have no scalar `AuroraDbType`; converting them throws a `NotSupportedException`. Arrays and `ENUM`s work through Npgsql's default (value-inferred) type handling.
* `Citext`, `Hstore`, `LTree`, `Geometry` and `Geography` require the corresponding PostgreSQL extensions (`citext`, `hstore`, `ltree`, `postgis`) to be installed in the database.
* Reading and writing `Geometry`/`Geography` values requires the [Npgsql.NetTopologySuite](https://www.nuget.org/packages/Npgsql.NetTopologySuite) (or [Npgsql.GeoJSON](https://www.nuget.org/packages/Npgsql.GeoJSON)) plugin.

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

A transaction that has already been committed or rolled back throws an `InvalidOperationException` when it is completed again, and beginning a transaction on a closed connection throws an `InvalidOperationException`.

## Connection String Builder

`AuroraDbConnectionStringBuilder` extends `DbConnectionStringBuilder` and provides a strongly typed way of creating Aurora connection strings. It is backed by the AWS wrapper's connection string builder, so it understands the AWS wrapper properties (e.g. `Plugins`) besides the usual `Host`, `Port`, `Database`, `Username` and `Password`.

```csharp
var builder = new AuroraDbConnectionStringBuilder
{
    Host = "my-cluster.cluster-abc123.eu-west-1.rds.amazonaws.com",
    Port = 5432,
    Database = "postgres",
    Username = "postgres",
    Plugins = "failover"
};

await using var connection =
    new AuroraDbConnection(builder.ConnectionString);

await connection.OpenAsync();
```

## Provider Factory

`AuroraDbFactory` extends `DbProviderFactory` and enables provider-independent ADO.NET applications and libraries to create RepoDb.Connector.AuroraDb objects.

```csharp
var factory = AuroraDbFactory.Instance;

using var connection = factory.CreateConnection();

connection.ConnectionString = connectionString;
connection.Open();
```

## Bulk Operations

RepoDb.Connector.AuroraDb provides bulk-loading support under the `RepoDb.Connector.AuroraDb.Bulk` namespace, built directly on top of Npgsql's binary `COPY` protocol (`NpgsqlBinaryImporter`, opened via `NpgsqlConnection.BeginBinaryImport`), which PostgreSQL supports.

| RepoDb.Connector.AuroraDb.Bulk          | Purpose                                                                                                       |
| ------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| `AuroraDbBulkCopy`                      | Efficiently bulk-loads a `DbDataReader`/`IDataReader`, `DataTable`, or `DataRow[]` into an Aurora table    |
| `AuroraDbBulkColumnMapping`             | Defines the mapping between a source column and a destination column                                          |
| `AuroraDbBulkCopyColumnMappingCollection` | The collection of `AuroraDbBulkColumnMapping` objects exposed by `AuroraDbBulkCopy.ColumnMappings`  |

### AuroraDbBulkCopy

`AuroraDbBulkCopy` streams rows directly into a `COPY "table" (...) FROM STDIN (FORMAT BINARY)` operation via `NpgsqlBinaryImporter`, resolving any name-based column mapping (source column name, destination column ordinal) against the source schema and the destination table's `information_schema.columns` output before writing:

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

> The binary `COPY` protocol is Npgsql specific, so `AuroraDbBulkCopy` unwraps the currently active `NpgsqlConnection` of the AWS wrapper to open the importer. The bulk-copy stream itself therefore runs directly on the Npgsql connection.

## Architecture

RepoDb.Connector.AuroraDb is more than a set of ADO.NET wrapper classes. The public ADO.NET API sits on top of the AWS Advanced .NET Data Provider Wrapper, which in turn sits on top of the communication and protocol infrastructure provided by Npgsql.

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
        Npgsql (NpgsqlConnection / NpgsqlCommand)
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
             Aurora PostgreSQL Cluster
```

## Local Development

A PostgreSQL-compatible server (with the `postgis` extension, used by the spatial type tests) for local development and the integration tests is defined in the repository's `docker-compose.yml` (using the `postgis/postgis` image):

```bash
docker compose up -d auroradb
```

The server is then available at `127.0.0.1:5435` (user `postgres`, password `RepoDB2026`). The integration tests read their connection strings from the `REPODB_AURORADB_CONSTR_SYSTEM` and `REPODB_AURORADB_CONSTR` environment variables, falling back to that local server.

> The local server is a plain PostgreSQL, not an Aurora cluster, so the Aurora-specific AWS wrapper features (topology-based failover, read/write splitting, ...) are not exercised by the integration tests.

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
* Aurora MySQL support (through the AWS wrapper's MySqlConnector dialect)
* Array, `ENUM` and `VECTOR` type mappings
* Performance optimizations

## ORM and Library Integration

Although RepoDb.Connector.AuroraDb can be used directly through ADO.NET, it is designed to work naturally with libraries that operate against the standard `System.Data.Common` abstractions.

For example:

```text
RepoDB
Dapper
Custom Data Access Layers
ADO.NET Applications
Other DbConnection-based Libraries
          │
          ▼
   RepoDb.Connector.AuroraDb
          │
          ▼
AWS Advanced .NET Data Provider Wrapper
          │
          ▼
  Aurora PostgreSQL Cluster
```

The connector itself should remain independent of any ORM.

## Contributing

RepoDb.Connector.AuroraDb is in its early stages, and contributions are welcome.

Areas where contributions will be particularly valuable include:

* Type mappings
* AWS wrapper plugin integration and testing (failover, EFM, IAM, Secrets Manager, ...)
* Authentication and TLS/SSL
* Connection pooling
* Async I/O
* Aurora version compatibility
* Performance benchmarking
* Bulk operations
* Integration and compatibility testing

When contributing, please keep the implementation aligned with the standard ADO.NET architecture and avoid unnecessary abstractions that could negatively affect performance.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the release history of this connector.

## License

RepoDb.Connector.AuroraDb is an independent open-source project. Amazon Aurora and AWS are trademarks of Amazon.com, Inc. or its affiliates. This project is not affiliated with, sponsored by, or endorsed by Amazon Web Services, Inc.

The [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) is licensed under the Apache License 2.0.

[Apache License 2.0](https://apache.org/licenses/LICENSE-2.0.html) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
