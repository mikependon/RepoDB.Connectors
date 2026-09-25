# Changelog for RepoDb.Connector.CockcroachDb

All notable changes to this connector are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this connector follows [Semantic Versioning](https://semver.org/).

## 1.0.0

Date: Unreleased

Initial draft release of the CockroachDB connector built on [Npgsql](https://www.nuget.org/packages/Npgsql). Introduces the core ADO.NET provider objects and bulk-copy support described in the [README](README.md).

#### Added

**Core ADO.NET objects** (`RepoDb.Connector.CockcroachDb` namespace), wrapping [Npgsql](https://www.nuget.org/packages/Npgsql) behind the standard `System.Data.Common` provider model, using the `CockcroachDb`-prefixed naming convention:

- `CockcroachDbConnection` — extends `DbConnection`. Opens and manages a connection to a CockroachDB cluster.
- `CockcroachDbCommand` — extends `DbCommand`. Executes SQL statements, with sync and async overloads for `ExecuteNonQuery`, `ExecuteScalar`, and `ExecuteReader`.
- `CockcroachDbDataReader` — extends `DbDataReader`. Reads the forward-only result set of a `CockcroachDbCommand`.
- `CockcroachDbParameter` / `CockcroachDbParameterCollection` — extend `DbParameter` / `DbParameterCollection`.
- `CockcroachDbTransaction` — extends `DbTransaction`. Provides `Commit`/`Rollback` semantics.
- `CockcroachDbException` — extends `DbException`. Wraps the underlying `NpgsqlException`.
- `CockcroachDbConnectionStringBuilder` — extends `DbConnectionStringBuilder`. Strongly typed connection-string properties (`Host`, `Port`, `Database`, `Username`, `Password`, ...).
- `CockcroachDbFactory` — extends `DbProviderFactory`.
- `CockcroachDbType` — enumeration of the column types supported by CockroachDB (numeric, string, `CITEXT`, `BYTES`, date/time, `INET`, bit string, `JSONB`, `UUID`, `OID`, `LTREE`, `TSVECTOR`/`TSQUERY`, `GEOMETRY`/`GEOGRAPHY`). PostgreSQL-only types (`MONEY`, `CIDR`, `MACADDR`, `XML`, `HSTORE`, geometric and range types) are intentionally excluded.
- `CockcroachDbTypeConverter` — converts between `CockcroachDbType` and `NpgsqlTypes.NpgsqlDbType`, mapping `NpgsqlDbType.Json` to `CockcroachDbType.Jsonb` (JSON is an alias of JSONB in CockroachDB).

**Bulk operations** (`RepoDb.Connector.CockcroachDb.Bulk` namespace), built directly on Npgsql's binary `COPY` protocol:

- `CockcroachDbBulkCopy` — bulk-loads an `IDataReader`/`DbDataReader`, `DataTable`, or `DataRow[]` into a CockroachDB table via `NpgsqlBinaryImporter`, with sync (`WriteToServer`) and async (`WriteToServerAsync`) overloads.
- `CockcroachDbBulkColumnMapping` / `CockcroachDbBulkCopyColumnMappingCollection` — source-to-destination column mapping for `CockcroachDbBulkCopy`.
