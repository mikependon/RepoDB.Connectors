# Worked example: RepoDb.Connector.AuroraDb.Npgsql

Condensed, verbatim-sourced walkthrough of a real, recently-built connector
(`src/RepoDb.Connector.AuroraDb.Npgsql` in `mikependon/RepoDB.Connectors`). It uses wrapping shape B
(wraps the AWS Advanced .NET Data Provider Wrapper, itself on top of Npgsql) — the more complex of the two
shapes described in `references/architecture.md`. For shape A (direct driver wrap), the sibling
`RepoDb.Connector.CockroachDb` connector in the same repo follows the identical pattern one layer
shallower (every field is a bare `NpgsqlConnection`/`NpgsqlCommand`/etc. instead of an
`AwsWrapperConnection<NpgsqlConnection>`/`AwsWrapperCommand`).

## Connection — delegation + one-time static setup + an `EnsureOpen` guard

```csharp
public class AuroraDbConnection : DbConnection
{
    private readonly AwsWrapperConnection<NpgsqlConnection> _connection;

    // One-time registration of whatever dialect/plugin the intermediate wrapper needs before it can
    // drive this specific underlying driver. Omit entirely for a direct wrap (shape A).
    static AuroraDbConnection()
    {
        NpgsqlDialectLoader.Load();
    }

    public AuroraDbConnection() : this(string.Empty) { }

    public AuroraDbConnection(string connectionString)
    {
        _connection = new AwsWrapperConnection<NpgsqlConnection>(connectionString);
    }

    // internal accessor so sibling classes (Command, Transaction) can reach the wrapped object.
    internal AwsWrapperConnection<NpgsqlConnection> InnerConnection => _connection;

    // public escape hatch for capabilities with no ADO.NET-standard representation.
    public AwsWrapperConnection WrappedConnection => _connection;

    public override string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }
    public override string Database => _connection.Database;
    public override string DataSource => _connection.DataSource;
    public override string ServerVersion => _connection.ServerVersion;
    public override ConnectionState State => _connection.State;

    public override void Open() => _connection.Open();
    public override Task OpenAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();  // the wrapper doesn't honor an already-cancelled token itself
        return _connection.OpenAsync(cancellationToken);
    }
    public override void Close() => _connection.Close();

    public new AuroraDbCommand CreateCommand() => (AuroraDbCommand)CreateDbCommand();
    protected override DbCommand CreateDbCommand() => new AuroraDbCommand((AwsWrapperCommand)_connection.CreateCommand(), this);
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        EnsureOpen();
        return new AuroraDbTransaction((AwsWrapperTransaction)_connection.BeginTransaction(isolationLevel), this);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _connection.Dispose();
        base.Dispose(disposing);
    }

    // A guard reused by Command before every execute call - throws a clear InvalidOperationException
    // instead of letting the underlying wrapper report a confusing failover attempt on a closed connection.
    internal void EnsureOpen()
    {
        if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            throw new InvalidOperationException("Connection is not open.");
    }
}
```

## Command — same delegation, plus a driver-exception-to-connector-exception boundary

```csharp
public class AuroraDbCommand : DbCommand
{
    private readonly AwsWrapperCommand _command;
    private readonly AuroraDbParameterCollection _parameters;
    private AuroraDbConnection _connection;
    private AuroraDbTransaction _transaction;

    public AuroraDbCommand(string commandText, AuroraDbConnection connection)
    {
        _command = new AwsWrapperCommand<NpgsqlCommand>(commandText, connection.InnerConnection);
        _parameters = new AuroraDbParameterCollection((NpgsqlParameterCollection)_command.Parameters);
        _connection = connection;
    }

    public new AuroraDbParameterCollection Parameters => _parameters;
    public override string CommandText { get => _command.CommandText; set => _command.CommandText = value; }
    protected override DbConnection DbConnection
    {
        get => _connection;
        set { _connection = (AuroraDbConnection)value; _command.Connection = _connection?.InnerConnection; }
    }
    protected override DbTransaction DbTransaction
    {
        get => _transaction;
        set { _transaction = (AuroraDbTransaction)value; _command.Transaction = _transaction?.InnerTransaction; }
    }

    // Every execute-shaped method follows this exact shape: guard the connection, delegate, translate
    // the driver's own exception type(s) into this connector's DbException subclass, let anything else
    // (including the intermediate wrapper's own failover-signaling exceptions, if any) pass through as-is.
    public override int ExecuteNonQuery()
    {
        EnsureConnection();
        try
        {
            return _command.ExecuteNonQuery();
        }
        catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
        {
            throw new AuroraDbException(exception);
        }
    }
    // ExecuteScalar, ExecuteReader/ExecuteDbDataReader, ExecuteNonQueryAsync, ExecuteScalarAsync,
    // ExecuteDbDataReaderAsync, Prepare, Cancel, CreateDbParameter all repeat this identical
    // guard/delegate/translate-exception shape - write it once, copy it for every method.

    protected override DbParameter CreateDbParameter() =>
        new AuroraDbParameter((NpgsqlParameter)_command.CreateParameter());

    private void EnsureConnection()
    {
        if (_connection is null)
            throw new InvalidOperationException("The Connection property has not been initialized.");
        _connection.EnsureOpen();
    }
}
```

## Parameter, ParameterCollection, DataReader, Transaction — pure delegation

```csharp
public class AuroraDbParameter : DbParameter
{
    private readonly NpgsqlParameter _parameter;
    internal AuroraDbParameter(NpgsqlParameter parameter) { _parameter = parameter; }
    internal NpgsqlParameter InnerParameter => _parameter;

    public override DbType DbType { get => _parameter.DbType; set => _parameter.DbType = value; }
    // The provider-native type property, alongside the standard DbType - this is the pattern every
    // connector's <Provider>Parameter follows for its own <Provider>Type property.
    public AuroraDbType AuroraDbType
    {
        get => AuroraDbTypeConverter.ToAuroraDbType(_parameter.NpgsqlDbType);
        set => _parameter.NpgsqlDbType = AuroraDbTypeConverter.ToNpgsqlDbType(value);
    }
    public override object Value { get => _parameter.Value; set => _parameter.Value = value; }
    public override void ResetDbType() => _parameter.ResetDbType();
    // ParameterName, Direction, IsNullable, Size, SourceColumn, SourceColumnNullMapping, SourceVersion:
    // same one-line get/set-forward pattern.
}

public class AuroraDbParameterCollection : DbParameterCollection
{
    private readonly NpgsqlParameterCollection _parameters;
    internal AuroraDbParameterCollection(NpgsqlParameterCollection parameters) { _parameters = parameters; }
    public override int Count => _parameters.Count;
    public AuroraDbParameter AddWithValue(string parameterName, object value) =>
        new AuroraDbParameter(_parameters.AddWithValue(parameterName, value));
    // Every DbParameterCollection abstract member (Add, Clear, Contains, IndexOf, Insert, Remove,
    // RemoveAt, this[int]/this[string], CopyTo, GetEnumerator, ...) forwards the same way, wrapping/
    // unwrapping AuroraDbParameter <-> NpgsqlParameter at the boundary.
}

public class AuroraDbDataReader : DbDataReader
{
    private readonly DbDataReader _reader;
    internal AuroraDbDataReader(DbDataReader reader) { _reader = reader; }
    public override bool Read() => _reader.Read();
    public override int FieldCount => _reader.FieldCount;
    public override bool GetBoolean(int ordinal) => _reader.GetBoolean(ordinal);
    // Every Get*(int ordinal) accessor, this[int]/this[string], GetSchemaTable, NextResult, IsDBNull,
    // GetEnumerator, and their Async counterparts: one-line forward, no exceptions here (a reader
    // failure surfaces from the Command call that produced it, not from reading rows afterward).
}

public class AuroraDbTransaction : DbTransaction
{
    private readonly AwsWrapperTransaction _transaction;
    private readonly AuroraDbConnection _connection;
    private bool _completed;   // guards against double commit/rollback - not tracked by the driver itself

    internal AuroraDbTransaction(AwsWrapperTransaction transaction, AuroraDbConnection connection)
    {
        _transaction = transaction;
        _connection = connection;
    }
    internal AwsWrapperTransaction InnerTransaction => _transaction;
    protected override DbConnection DbConnection => _connection;

    public override void Commit()
    {
        EnsureNotCompleted();
        _transaction.Commit();
        _completed = true;
    }
    public override void Rollback()
    {
        EnsureNotCompleted();
        _transaction.Rollback();
        _completed = true;
    }
    private void EnsureNotCompleted()
    {
        if (_completed)
            throw new InvalidOperationException("This transaction has completed; it is no longer usable.");
    }
}
```

## Exception — wraps the driver's exception, lets unrelated ones pass through

```csharp
public class AuroraDbException : DbException
{
    private readonly DbException _exception;

    internal AuroraDbException(DbException exception) : base(exception.Message, exception)
    {
        _exception = exception;
    }

    public override int ErrorCode => _exception.ErrorCode;
    public override string SqlState => _exception is AwsWrapperDbException wrapperException ? wrapperException.SqlState : _exception.SqlState;
}
```
The real connector deliberately does **not** wrap every exception type — the AWS wrapper's own
failover-signaling exceptions (`FailoverSuccessException`, `FailoverFailedException`, ...) are left
unwrapped so callers can catch and handle them exactly as the wrapper's own docs describe. Decide
per-connector which exception types genuinely need normalizing into `<Provider>Exception` and which carry
meaning specific to whatever's underneath that a caller needs to see undisguised.

## Factory, ConnectionStringBuilder

```csharp
public class AuroraDbFactory : DbProviderFactory
{
    public static readonly AuroraDbFactory Instance = new AuroraDbFactory();
    public override DbConnection CreateConnection() => new AuroraDbConnection();
    public override DbCommand CreateCommand() => new AuroraDbCommand();
    public override DbParameter CreateParameter() => new AuroraDbParameter();
    public override DbConnectionStringBuilder CreateConnectionStringBuilder() => new AuroraDbConnectionStringBuilder();
}

public class AuroraDbConnectionStringBuilder : DbConnectionStringBuilder
{
    private readonly AwsWrapperConnectionStringBuilder _builder;
    public const int DefaultPort = 5432;

    public AuroraDbConnectionStringBuilder() { _builder = new AwsWrapperConnectionStringBuilder(); }

    public new string ConnectionString { get => _builder.ConnectionString; set => _builder.ConnectionString = value; }
    public string Host { get => _builder.Host; set => _builder.Host = value; }
    public int Port { get => _builder.Port ?? DefaultPort; set => _builder.Port = value; }
    public string Database { get => GetValue("Database"); set => _builder["Database"] = value; }
    // Every connection property the driver/wrapper supports gets its own strongly-typed property here,
    // plus any wrapper-specific one worth surfacing (here, "Plugins" for the AWS wrapper's plugin list).
}
```

## Type + TypeConverter — mirror the driver's own type enum

```csharp
public enum AuroraDbType
{
    SmallInt, Integer, BigInt, Decimal, Real, Double, Money, Boolean,   // Numeric
    Char, VarChar, Text, Name, Citext,                                  // String
    Bytea,                                                              // Binary
    Date, Time, TimeTz, Timestamp, TimestampTz, Interval,               // Date/Time
    // ... one member per scalar type the driver's own enum has and this engine actually supports
}

public static class AuroraDbTypeConverter
{
    private static readonly Dictionary<NpgsqlDbType, AuroraDbType> NpgsqlToAurora = new()
    {
        { NpgsqlDbType.Smallint, AuroraDbType.SmallInt },
        { NpgsqlDbType.Integer, AuroraDbType.Integer },
        // ... one entry per scalar mapping
    };
    private static readonly Dictionary<AuroraDbType, NpgsqlDbType> AuroraToNpgsql = BuildReverseMap();

    public static AuroraDbType ToAuroraDbType(NpgsqlDbType npgsqlDbType) =>
        NpgsqlToAurora.TryGetValue(npgsqlDbType, out var value)
            ? value
            : throw new NotSupportedException($"'{npgsqlDbType}' has no scalar {nameof(AuroraDbType)} equivalent.");

    public static NpgsqlDbType ToNpgsqlDbType(AuroraDbType auroraDbType) => AuroraToNpgsql[auroraDbType];

    private static Dictionary<AuroraDbType, NpgsqlDbType> BuildReverseMap() =>
        NpgsqlToAurora.ToDictionary(kv => kv.Value, kv => kv.Key);
}
```
Composite/structural driver-enum values (arrays, generic `Range`/`Multirange` flags, internal-only types)
are deliberately left out of the map entirely — converting one throws `NotSupportedException` rather than
guessing at a lossy equivalent. Document this explicitly (see the README excerpt in
`references/project-layout.md`) so consumers know arrays/enums fall back to the driver's own
value-inferred type handling instead of an explicit `<Provider>Type`.

## Bulk copy — a genuine protocol implementation, not delegation

```csharp
public class AuroraDbBulkCopy : IDisposable
{
    private readonly AuroraDbConnection _connection;

    public AuroraDbBulkCopy(AuroraDbConnection connection)
    {
        ColumnMappings = new AuroraDbBulkCopyColumnMappingCollection();
        _connection = connection;
    }

    public AuroraDbBulkCopyColumnMappingCollection ColumnMappings { get; }
    public int BulkCopyTimeout { get; set; }
    public string DestinationTableName { get; set; }
    public int RowsCopied { get; private set; }

    // Every WriteToServer overload (IDataReader, DbDataReader, DataTable, DataTable+DataRowState,
    // DataRow[]) is a thin sync-over-async or filter-then-delegate wrapper around one shared engine:
    public async Task<int> WriteToServerAsync(IDataReader reader, CancellationToken cancellationToken = default)
    {
        int ResolveSourceOrdinal(AuroraDbBulkColumnMapping mapping) => reader.GetOrdinal(mapping.SourceColumn);

        async Task WriteRowsAsync(List<(int SourceOrdinal, string DestinationColumn)> mappings, NpgsqlBinaryImporter importer, CancellationToken ct)
        {
            while (reader.Read())
            {
                var values = new object[mappings.Count];
                for (var i = 0; i < mappings.Count; i++)
                {
                    var value = reader.GetValue(mappings[i].SourceOrdinal);
                    values[i] = value is DBNull ? null : value;   // the driver's binary importer wants a
                                                                    // real null, not DBNull.Value
                }
                await importer.WriteRowAsync(ct, values).ConfigureAwait(false);
            }
        }

        RowsCopied = await ExecuteAsync(ResolveSourceOrdinal, WriteRowsAsync, cancellationToken).ConfigureAwait(false);
        return RowsCopied;
    }

    // The shared engine: ensure open, resolve column mappings (by name or ordinal, on both sides),
    // build the actual COPY command text, open the binary importer, stream rows, complete, restore
    // connection state.
    private async Task<int> ExecuteAsync(
        Func<AuroraDbBulkColumnMapping, int> resolveSourceOrdinal,
        Func<List<(int SourceOrdinal, string DestinationColumn)>, NpgsqlBinaryImporter, CancellationToken, Task> writeRowsAsync,
        CancellationToken cancellationToken)
    {
        var wasClosed = _connection.State == ConnectionState.Closed;
        if (wasClosed) await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var columnMappings = await BuildColumnMappingsAsync(resolveSourceOrdinal, cancellationToken).ConfigureAwait(false);
            var columnList = string.Join(", ", columnMappings.Select(m => QuoteIdentifier(m.DestinationColumn)));
            var copyCommand = $"COPY {QuoteIdentifier(DestinationTableName)} ({columnList}) FROM STDIN (FORMAT BINARY)";

            // The binary COPY protocol is Npgsql-specific and has no AWS-wrapper equivalent, so this
            // reaches straight through to the underlying NpgsqlConnection for this one operation - the
            // exact reason the public Unwrap<T>()/WrappedConnection escape hatch exists.
            var importer = await _connection.InnerConnection.Unwrap<NpgsqlConnection>().BeginBinaryImportAsync(copyCommand, cancellationToken).ConfigureAwait(false);
            await using (importer.ConfigureAwait(false))
            {
                if (BulkCopyTimeout > 0) importer.Timeout = TimeSpan.FromSeconds(BulkCopyTimeout);
                await writeRowsAsync(columnMappings, importer, cancellationToken).ConfigureAwait(false);
                return (int)await importer.CompleteAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            if (wasClosed) _connection.Close();   // only close what this call itself opened
        }
    }

    // A mapping may name its destination column, or only give an ordinal - resolving an ordinal to a
    // name requires one metadata query, done at most once per call (lazily, only if actually needed).
    private async Task<List<(int SourceOrdinal, string DestinationColumn)>> BuildColumnMappingsAsync(
        Func<AuroraDbBulkColumnMapping, int> resolveSourceOrdinal, CancellationToken cancellationToken)
    {
        List<string> destinationColumns = null;
        var mappings = new List<(int, string)>(ColumnMappings.Count);
        foreach (AuroraDbBulkColumnMapping mapping in ColumnMappings)
        {
            var sourceOrdinal = mapping.SourceOrdinal >= 0 ? mapping.SourceOrdinal : resolveSourceOrdinal(mapping);
            var destinationColumn = mapping.DestinationColumn;
            if (string.IsNullOrEmpty(destinationColumn))
            {
                destinationColumns ??= await GetDestinationColumnNamesAsync(cancellationToken).ConfigureAwait(false);
                destinationColumn = destinationColumns[mapping.DestinationOrdinal];
            }
            mappings.Add((sourceOrdinal, destinationColumn));
        }
        return mappings;
    }
}
```

`AuroraDbBulkColumnMapping` and `AuroraDbBulkCopyColumnMappingCollection` are pure data-holding boilerplate
(four constructor overloads for name/ordinal × source/destination, a `CollectionBase`-derived collection
with an `Add` overload per constructor shape) — copy their shape essentially unchanged.
