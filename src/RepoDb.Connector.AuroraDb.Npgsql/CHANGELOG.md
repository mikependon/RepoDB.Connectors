# Changelog for RepoDb.Connector.AuroraDb.Npgsql

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 1.2.0

Date: 2026-10-23

#### Changed

- Bumped [Meziantou.Analyzer](https://www.nuget.org/packages/Meziantou.Analyzer) from `3.0.296` to `3.0.316`, the latest version.
- Bumped [Microsoft.CodeAnalysis.NetAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.NetAnalyzers) from `10.0.401` to `10.0.402`, the latest version.

## 0.0.1-alpha2

Date: 2026-10-10

#### Changed

- Bumped [AWS.AdvancedDotnetDataProviderWrapper.Core](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Core) and [AWS.AdvancedDotnetDataProviderWrapper.Dialect.Npgsql](https://www.nuget.org/packages/AWS.AdvancedDotnetDataProviderWrapper.Dialect.Npgsql) from `2.2.0` to `3.0.0`, the latest version.
- Bumped [Meziantou.Analyzer](https://www.nuget.org/packages/Meziantou.Analyzer) from `3.0.290` to `3.0.296`, the latest version.
- Bumped [Microsoft.Testing.Extensions.CodeCoverage](https://www.nuget.org/packages/Microsoft.Testing.Extensions.CodeCoverage) from `18.11.2` to `18.12.0`, the latest version.
- Bumped [Microsoft.Testing.Extensions.TrxReport](https://www.nuget.org/packages/Microsoft.Testing.Extensions.TrxReport) from `2.4.1` to `2.5.1`, the latest version.
- Bumped [MSTest.TestAdapter](https://www.nuget.org/packages/MSTest.TestAdapter) and [MSTest.TestFramework](https://www.nuget.org/packages/MSTest.TestFramework) from `4.4.1` to `4.5.1`, the latest version.
- Bumped the `MSTest.Sdk` test project SDK from `4.0.2` to `4.5.1`, the latest version.

## 0.0.1-alpha1

Date: TBA

Initial draft release of the Amazon Aurora PostgreSQL connector, built on the [AWS Advanced .NET Data Provider Wrapper](https://github.com/aws/aws-advanced-dotnet-data-provider-wrapper) and [Npgsql](https://www.nuget.org/packages/Npgsql). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](README.md). Ref: [#7](https://github.com/mikependon/RepoDB.Connectors/issues/7).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.AuroraDb.Npgsql` namespace), wrapping the AWS wrapper (which wraps [Npgsql](https://www.nuget.org/packages/Npgsql)) behind the standard `System.Data.Common` provider model, using the `AuroraDb`-prefixed naming convention:

- `AuroraDbConnection` — extends `DbConnection`. Backed by `AwsWrapperConnection<NpgsqlConnection>`; registers the `AWS.AdvancedDotnetDataProviderWrapper.Dialect.Npgsql` dialect automatically, so the AWS wrapper capabilities (failover, enhanced failure monitoring, read/write splitting, IAM authentication, Secrets Manager, ...) are available through the connection string. Exposes the underlying wrapper through `WrappedConnection` for advanced scenarios. Opening an already-cancelled token throws `OperationCanceledException` without attempting to connect, and beginning a transaction on a closed connection throws `InvalidOperationException`.
- `AuroraDbCommand` — extends `DbCommand`. Delegates to the command of the wrapped AWS connection, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`. Executing without a connection, or with a closed connection, throws `InvalidOperationException`.
- `AuroraDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of an `AuroraDbCommand`.
- `AuroraDbParameter` / `AuroraDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `AuroraDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics, including the async overloads. Completing a transaction twice throws `InvalidOperationException`.
- `AuroraDbException` — extends `DbException`. Wraps the underlying `NpgsqlException` or `AwsWrapperDbException`, exposing its `SqlState`. The AWS wrapper failover exceptions (e.g. `FailoverSuccessException`) are surfaced as-is.
- `AuroraDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Backed by the AWS wrapper's connection string builder; strongly typed connection-string properties (`Host`, `Port`, `Database`, `Username`, `Password`, `Plugins`).
- `AuroraDbFactory` — extends `DbProviderFactory`.
- `AuroraDbType` — enumeration of the Aurora PostgreSQL column types (numeric, `MONEY`, string, `CITEXT`, `BYTEA`, date/time, network address, bit string, `JSON`/`JSONB`/`JSONPATH`, `XML`, geometric, range, `UUID`, `OID`, `HSTORE`, `LTREE`, `TSVECTOR`/`TSQUERY`, `GEOMETRY`/`GEOGRAPHY`).
- `AuroraDbTypeConverter` — converts between `AuroraDbType` and `NpgsqlTypes.NpgsqlDbType`. Unlike CockroachDB, `JSON` and `JSONB` are distinct types. Composite values (arrays, generic range flags) throw a `NotSupportedException`.

**Bulk operations** (`RepoDb.Connector.AuroraDb.Npgsql.Bulk` namespace), built directly on Npgsql's binary `COPY` protocol (through the `NpgsqlConnection` unwrapped from the AWS wrapper):

- `AuroraDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into an Aurora table via `NpgsqlBinaryImporter`, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `AuroraDbBulkColumnMapping` / `AuroraDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `AuroraDbBulkCopy`.

**Tests and tooling**

- Unit tests and integration tests, covering both the positive and the negative scenarios (invalid credentials, unknown database, unreachable host, invalid SQL, closed connections, completed transactions, unsupported types, ...).
- An `auroradb` service (`postgis/postgis`) in the repository's `docker-compose.yml` for the local development and the integration tests.
