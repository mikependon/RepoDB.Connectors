# Changelog for RepoDb.Connector.MariaDb

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1

Date: 2026-08-18

Initial draft release of the MariaDB connector. Introduces the core ADO.NET provider objects and bulk-loading support described in the [README](src/RepoDb.Connector.MariaDb/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.MariaDb` namespace), wrapping [MySql.Data](https://www.nuget.org/packages/mysql.data) behind the standard `System.Data.Common` provider model:

- `MariaDbConnection` — extends `DbConnection`. Opens and manages a connection to a MariaDB server.
- `MariaDbCommand` — extends `DbCommand`. Executes SQL statements, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`.
- `MariaDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of a `MariaDbCommand`.
- `MariaDbParameter` / `MariaDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `MariaDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics.
- `MariaDbException` — extends `DbException`. Wraps the underlying `MySqlException`.
- `MariaDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Strongly typed connection-string properties (`Server`, `Port`, `Database`, `UserId`, `Password`, ...).
- `MariaDbProviderFactory` — extends `DbProviderFactory`.
- `MariaDbType` — enumeration of MariaDB-specific column types.
- `MariaDbTypeConverter` — converts between `MariaDbType` and `MySqlDbType`.

**Bulk operations** (`RepoDb.Connector.MariaDb.Bulk` namespace), built on `LOAD DATA LOCAL INFILE`:

- `MariaDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into a MariaDB table, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `MariaDbBulkColumnMapping` / `MariaDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `MariaDbBulkCopy`.
- `MariaDbBulkLoader` — wraps `MySqlBulkLoader` for loading directly from a file or stream, with `Load`/`LoadAsync` overloads.
- `MariaDbBulkLoaderConflictOption` — controls key-conflict behavior during a load.
- `MariaDbBulkLoaderPriority` — controls load priority (`None`, `Low`, `Concurrent`).
