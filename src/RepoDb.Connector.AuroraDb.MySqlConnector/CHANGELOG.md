# Changelog for RepoDb.Connector.AuroraDb.MySqlConnector

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1-alpha1

Date: 2026-09-26

Initial draft release of the Amazon Aurora MySQL connector, built on the [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) (with its MySqlConnector dialect) and [MySqlConnector](https://www.nuget.org/packages/MySqlConnector). It is the MySQL counterpart of the Aurora PostgreSQL connector (`RepoDb.Connector.AuroraDb.Npgsql`). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](README.md). Ref: [#7](https://github.com/mikependon/RepoDB.Connectors/issues/7).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.AuroraDb.MySqlConnector` namespace), wrapping the AWS wrapper (which wraps [MySqlConnector](https://www.nuget.org/packages/MySqlConnector)) behind the standard `System.Data.Common` provider model, using the `AuroraDb`-prefixed naming convention:

- `AuroraDbConnection` — extends `DbConnection`. Backed by `AwsWrapperConnection<MySqlConnection>`; registers the `AWS.AdvancedDotnetDataProviderWrapper.Dialect.MySqlConnector` dialect automatically, so the AWS wrapper capabilities (failover, enhanced failure monitoring, read/write splitting, IAM authentication, Secrets Manager, ...) are available through the connection string. Exposes the underlying wrapper through `WrappedConnection` for advanced scenarios. Opening with an already-cancelled token throws `OperationCanceledException` without attempting to connect, and beginning a transaction on a closed connection throws `InvalidOperationException`.
- `AuroraDbCommand` — extends `DbCommand`. Delegates to the command of the wrapped AWS connection, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`. Executing without a connection, or with a closed connection, throws `InvalidOperationException`.
- `AuroraDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of an `AuroraDbCommand`.
- `AuroraDbParameter` / `AuroraDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `AuroraDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics, including the async overloads. Completing a transaction twice throws `InvalidOperationException`.
- `AuroraDbException` — extends `DbException`. Wraps the underlying `MySqlException` or `AwsWrapperDbException`, exposing its `Number` and `SqlState`. The AWS wrapper failover exceptions (e.g. `FailoverSuccessException`) are surfaced as-is.
- `AuroraDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Backed by the AWS wrapper's connection string builder; strongly typed connection-string properties (`Server`, `Port`, `Database`, `UserId`, `Password`, `Plugins`).
- `AuroraDbFactory` — extends `DbProviderFactory`.
- `AuroraDbType` — enumeration of the Aurora MySQL column types (numeric, string, `ENUM`/`SET`, binary/blob, date/time, `JSON`, spatial).
- `AuroraDbTypeConverter` — converts between `AuroraDbType` and `MySqlConnector.MySqlDbType`. Unmapped values throw a `NotSupportedException`.

**Bulk operations** (`RepoDb.Connector.AuroraDb.MySqlConnector.Bulk` namespace), built on MySqlConnector's `MySqlBulkCopy` (through the `MySqlConnection` unwrapped from the AWS wrapper):

- `AuroraDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into an Aurora MySQL table, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `AuroraDbBulkColumnMapping` / `AuroraDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `AuroraDbBulkCopy`.

**Tests and tooling**

- Unit tests and integration tests, covering both the positive and the negative scenarios (invalid credentials, unknown database, unreachable host, invalid SQL, closed connections, completed transactions, out-of-range and invalid values, unsupported types, ...).
- An `auroradbmysql` service (`mysql:8.0`) in the repository's `docker-compose.yml` for the local development and the integration tests.
