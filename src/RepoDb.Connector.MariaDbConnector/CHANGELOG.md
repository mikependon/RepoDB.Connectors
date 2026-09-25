# Changelog for RepoDb.Connector.MariaDbConnector

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 1.1.1

Date: 2026-09-25

#### Changed

- Bumped [Meziantou.Analyzer](https://www.nuget.org/packages/Meziantou.Analyzer) from `3.0.267` to `3.0.290`, the latest version.

## 1.1.0

Date: 2026-09-21

#### Changed

- Bumped [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) from `2.6.1` to `2.6.2`, the latest version.
- Fixed code smells reported by the Roslyn and Meziantou static code analyzers.

## 1.0.0

Date: 2026-09-08

Initial draft release of the MariaDB connector built on [MySqlConnector](https://www.nuget.org/packages/MySqlConnector). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](src/RepoDb.Connector.MariaDbConnector/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.MariaDbConnector` namespace), wrapping [MySqlConnector](https://www.nuget.org/packages/MySqlConnector) behind the standard `System.Data.Common` provider model:

- `MariaDbConnection` — extends `DbConnection`. Opens and manages a connection to a MariaDB server.
- `MariaDbCommand` — extends `DbCommand`. Executes SQL statements, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`.
- `MariaDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of a `MariaDbCommand`.
- `MariaDbParameter` / `MariaDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `MariaDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics.
- `MariaDbException` — extends `DbException`. Wraps the underlying `MySqlException`.
- `MariaDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Strongly typed connection-string properties (`Server`, `Port`, `Database`, `UserId`, `Password`, ...).
- `MariaDbProviderFactory` — extends `DbProviderFactory`.
- `MariaDbType` — enumeration of MariaDB-specific column types.
- `MariaDbTypeConverter` — converts between `MariaDbType` and `MySqlConnector.MySqlDbType`.

**Bulk operations** (`RepoDb.Connector.MariaDbConnector.Bulk` namespace), built directly on `MySqlConnector`'s `MySqlBulkCopy`:

- `MariaDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into a MariaDB table, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `MariaDbBulkColumnMapping` / `MariaDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `MariaDbBulkCopy`.
