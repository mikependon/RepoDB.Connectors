# Changelog for RepoDb.Connector.CockroachDb

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 0.0.1-alpha2

Date: 2026-09-25

#### Changed

- Bumped [Meziantou.Analyzer](https://www.nuget.org/packages/Meziantou.Analyzer) from `3.0.267` to `3.0.290`, the latest version.

## 0.0.1-alpha1

Date: 2026-09-25

Initial draft release of the CockroachDB connector built on [Npgsql](https://www.nuget.org/packages/Npgsql). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.CockroachDb` namespace), wrapping [Npgsql](https://www.nuget.org/packages/Npgsql) behind the standard `System.Data.Common` provider model, using the `CockroachDb`-prefixed naming convention:

- `CockroachDbConnection` — extends `DbConnection`. Opens and manages a connection to a CockroachDB cluster.
- `CockroachDbCommand` — extends `DbCommand`. Executes SQL statements, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`.
- `CockroachDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of a `CockroachDbCommand`.
- `CockroachDbParameter` / `CockroachDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `CockroachDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics.
- `CockroachDbException` — extends `DbException`. Wraps the underlying `NpgsqlException`.
- `CockroachDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Strongly typed connection-string properties (`Host`, `Port`, `Database`, `Username`, `Password`, ...).
- `CockroachDbFactory` — extends `DbProviderFactory`.
- `CockroachDbType` — enumeration of the column types supported by CockroachDB (numeric, string, `CITEXT`, `BYTES`, date/time, `INET`, bit string, `JSONB`, `UUID`, `OID`, `LTREE`, `TSVECTOR`/`TSQUERY`, `GEOMETRY`/`GEOGRAPHY`). PostgreSQL-only types (`MONEY`, `CIDR`, `MACADDR`, `XML`, `HSTORE`, geometric and range types) are intentionally excluded.
- `CockroachDbTypeConverter` — converts between `CockroachDbType` and `NpgsqlTypes.NpgsqlDbType`, mapping `NpgsqlDbType.Json` to `CockroachDbType.Jsonb` (JSON is an alias of JSONB in CockroachDB).

**Bulk operations** (`RepoDb.Connector.CockroachDb.Bulk` namespace), built directly on Npgsql's binary `COPY` protocol:

- `CockroachDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into a CockroachDB table via `NpgsqlBinaryImporter`, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `CockroachDbBulkColumnMapping` / `CockroachDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `CockroachDbBulkCopy`.
