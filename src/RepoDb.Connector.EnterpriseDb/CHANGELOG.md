# Changelog for RepoDb.Connector.EnterpriseDb

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1

Date: 2026-09-02

Initial draft release of the EnterpriseDB connector built on [Npgsql](https://www.nuget.org/packages/Npgsql). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](src/RepoDb.Connector.EnterpriseDb/README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.EnterpriseDb` namespace), wrapping [Npgsql](https://www.nuget.org/packages/Npgsql) behind the standard `System.Data.Common` provider model, using the `EDB`-prefixed naming convention of EnterpriseDB's own [EnterpriseDB.EDBClient](https://www.nuget.org/packages/EnterpriseDB.EDBClient) driver:

- `EDBConnection` — extends `DbConnection`. Opens and manages a connection to an EnterpriseDB server.
- `EDBCommand` — extends `DbCommand`. Executes SQL statements, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`.
- `EDBDataReader` — extends `DbDataReader`. Reads the forward-only result set of an `EDBCommand`.
- `EDBParameter` / `EDBParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `EDBTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics.
- `EDBException` — extends `DbException`. Wraps the underlying `NpgsqlException`.
- `EDBConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Strongly typed connection-string properties (`Host`, `Port`, `Database`, `Username`, `Password`, ...).
- `EDBFactory` — extends `DbProviderFactory`.
- `EDBType` — enumeration of PostgreSQL/EnterpriseDB-specific column types.
- `EDBTypeConverter` — converts between `EDBType` and `NpgsqlTypes.NpgsqlDbType`.

**Bulk operations** (`RepoDb.Connector.EnterpriseDb.Bulk` namespace), built directly on Npgsql's binary `COPY` protocol:

- `EDBBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into an EnterpriseDB table via `NpgsqlBinaryImporter`, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `EDBBulkColumnMapping` / `EDBBulkCopyColumnMappingCollection` — source-to-destination column mapping for `EDBBulkCopy`.
