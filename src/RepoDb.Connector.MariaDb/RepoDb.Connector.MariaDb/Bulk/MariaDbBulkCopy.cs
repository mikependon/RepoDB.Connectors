#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.MariaDb.Bulk
{
    /// <summary>
    /// Lets you efficiently bulk load a MariaDB table with data from another source.
    /// </summary>
    public class MariaDbBulkCopy : IDisposable
    {
        private readonly MariaDbConnection _connection;
        private const string _fieldTerminator = "\t";
        private const string _lineTerminator = "\n";

        // TODO:    Let us not support the MariaDbBulkCopyOptions for now.
        //          We can later extend this when there is a need.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkCopy"/> class using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the destination database.</param>
        public MariaDbBulkCopy(
            string connectionString)
            : this(new MariaDbConnection(connectionString))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkCopy"/> class using the specified <see cref="MariaDbConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="MariaDbConnection"/> to the destination database.</param>
        public MariaDbBulkCopy(
            MariaDbConnection connection)
        {
            ColumnMappings = new MariaDbBulkCopyColumnMappingCollection();
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of column mappings that determine which source columns are written to which destination columns.
        /// </summary>
        public MariaDbBulkCopyColumnMappingCollection ColumnMappings { get; private set; }

        /// <summary>
        /// Gets or sets the number of rows in each batch. At the end of each batch, the rows are sent to the server.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds for the operation to complete before it times out.
        /// </summary>
        public int BulkCopyTimeout { get; set; }

        /// <summary>
        /// Gets or sets the destination table name to where to bulk copy the data.
        /// </summary>
        public string DestinationTableName { get; set; }

        /// <summary>
        /// Gets the number of rows copied during the current bulk copy operation.
        /// </summary>
        public int RowsCopied { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbBulkCopy"/>.
        /// </summary>
        public void Dispose() => _connection.Dispose();

        /// <summary>
        /// Copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            IDataReader reader)
        {
            RowsCopied = WriteToServerInternal(reader);
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DbDataReader reader)
        {
            RowsCopied = WriteToServerInternal(reader);
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DataTable table)
        {
            var rows = new DataRow[table.Rows.Count];
            table.Rows.CopyTo(rows, 0);
            RowsCopied = WriteToServerInternal(rows, table.Columns.Count);
        }

        /// <summary>
        /// Copies only rows that match the supplied row state in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="rowState">A value from the <see cref="DataRowState"/> enumeration used to filter which rows are copied.</param>
        public void WriteToServer(
            DataTable table,
            DataRowState rowState)
        {
            var rows = SelectRows(table, rowState);
            RowsCopied = WriteToServerInternal(rows, table.Columns.Count);
        }

        /// <summary>
        /// Copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        public void WriteToServer(
            DataRow[] rows)
        {
            var columnCount = rows != null && rows.Length > 0 ? rows[0].Table.Columns.Count : 0;
            RowsCopied = WriteToServerInternal(rows, columnCount);
        }

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteToServerAsync(
            IDataReader reader,
            CancellationToken cancellationToken = default)
        {
            RowsCopied = await WriteToServerInternalAsync(reader, cancellationToken);
        }

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteToServerAsync(
            DbDataReader reader,
            CancellationToken cancellationToken = default)
        {
            RowsCopied = await WriteToServerInternalAsync(reader, cancellationToken);
        }

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteToServerAsync(
            DataTable table,
            CancellationToken cancellationToken = default)
        {
            var rows = new DataRow[table.Rows.Count];
            table.Rows.CopyTo(rows, 0);
            RowsCopied = await WriteToServerInternalAsync(rows, table.Columns.Count, cancellationToken);
        }

        /// <summary>
        /// Asynchronously copies only rows that match the supplied row state in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="rowState">A value from the <see cref="DataRowState"/> enumeration used to filter which rows are copied.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteToServerAsync(
            DataTable table,
            DataRowState rowState,
            CancellationToken cancellationToken = default)
        {
            var rows = SelectRows(table, rowState);
            RowsCopied = await WriteToServerInternalAsync(rows, table.Columns.Count, cancellationToken);
        }

        /// <summary>
        /// Asynchronously copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task WriteToServerAsync(
            DataRow[] rows,
            CancellationToken cancellationToken = default)
        {
            var columnCount = rows != null && rows.Length > 0 ? rows[0].Table.Columns.Count : 0;
            RowsCopied = await WriteToServerInternalAsync(rows, columnCount, cancellationToken);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Writes every remaining row of <paramref name="reader"/> to <see cref="DestinationTableName"/>.
        /// </summary>
        /// <param name="reader">
        /// The source reader. Read via each mapping's <see cref="MariaDbBulkColumnMapping.SourceOrdinal"/>, or by
        /// <see cref="MariaDbBulkColumnMapping.SourceColumn"/> name when the ordinal is unset.
        /// </param>
        /// <returns>The number of rows <see cref="MariaDbBulkLoader"/> reports having loaded.</returns>
        private int WriteToServerInternal(IDataReader reader) =>
            WriteToServerInternalAsync(reader, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronous counterpart of <see cref="WriteToServer(IDataReader)"/>.
        /// </summary>
        public async Task<int> WriteToServerInternalAsync(IDataReader reader,
            CancellationToken cancellationToken = default)
        {
            var filePath = GetTempFilePath();
            try
            {
                using (var writer = CreateFileWriter(filePath))
                {
                    while (reader.Read())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        WriteRow(writer, mapping =>
                        {
                            var ordinal = mapping.SourceOrdinal >= 0 ? mapping.SourceOrdinal : reader.GetOrdinal(mapping.SourceColumn);
                            return reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                        });
                    }
                }
                return await LoadAsync(filePath, cancellationToken);
            }
            finally
            {
                DeleteTempFile(filePath);
            }
        }

        /// <summary>
        /// Writes <paramref name="rows"/> to <see cref="DestinationTableName"/>.
        /// </summary>
        /// <param name="rows">The source rows, read via each mapping's <see cref="MariaDbBulkColumnMapping.SourceOrdinal"/> - i.e. the real <see cref="DataTable"/> column index, not the mapping's position in <see cref="ColumnMappings"/> (those can legitimately diverge) - or by <see cref="MariaDbBulkColumnMapping.SourceColumn"/> name when the ordinal is unset.</param>
        /// <param name="columnCount">Unused beyond a defensive bounds check - <see cref="ColumnMappings"/> alone already determines exactly which columns get written.</param>
        /// <returns>The number of rows <see cref="MariaDbBulkLoader"/> reports having loaded.</returns>
        public int WriteToServerInternal(DataRow[] rows,
            int columnCount) =>
            WriteToServerInternalAsync(rows, columnCount, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronous counterpart of <see cref="WriteToServer(DataRow[], int)"/>.
        /// </summary>
        public async Task<int> WriteToServerInternalAsync(DataRow[] rows,
            int columnCount,
            CancellationToken cancellationToken = default)
        {
            var filePath = GetTempFilePath();
            try
            {
                using (var writer = CreateFileWriter(filePath))
                {
                    foreach (var row in rows ?? Array.Empty<DataRow>())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        WriteRow(writer, mapping =>
                        {
                            var ordinal = mapping.SourceOrdinal >= 0 ? mapping.SourceOrdinal : row.Table.Columns.IndexOf(mapping.SourceColumn);
                            return ordinal >= 0 && ordinal < columnCount && ordinal < row.Table.Columns.Count && !row.IsNull(ordinal) ? row[ordinal] : null;
                        });
                    }
                }
                return await LoadAsync(filePath, cancellationToken);
            }
            finally
            {
                DeleteTempFile(filePath);
            }
        }

        /// <summary>
        /// Writes one row to <paramref name="writer"/> - one tab-separated field per entry in
        /// <see cref="ColumnMappings"/>, in that order, terminated by <see cref="LineTerminator"/>.
        /// </summary>
        /// <param name="writer">The temp file writer.</param>
        /// <param name="getValue">Resolves a column mapping - by <see cref="MariaDbBulkColumnMapping.SourceOrdinal"/> or, when unset, by <see cref="MariaDbBulkColumnMapping.SourceColumn"/> - to its raw CLR value (or <see langword="null"/> for a database <c>NULL</c>).</param>
        private void WriteRow(StreamWriter writer,
            Func<MariaDbBulkColumnMapping, object> getValue)
        {
            for (var i = 0; i < ColumnMappings.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(_fieldTerminator);
                }
                writer.Write(FormatValue(getValue((MariaDbBulkColumnMapping)ColumnMappings[i])));
            }
            writer.Write(_lineTerminator);
        }

        /// <summary>
        /// Formats a single CLR value for the temp file, using <c>LOAD DATA</c>'s own documented escape
        /// sequences (<c>\N</c> for <c>NULL</c>, <c>\\</c>/<c>\t</c>/<c>\n</c>/<c>\r</c> for the literal
        /// characters that would otherwise be misread as field/line structure).
        /// </summary>
        private static string FormatValue(object value)
        {
            if (value == null || value is DBNull)
            {
                return "\\N";
            }

            return value switch
            {
                Guid guid => guid.ToString("D", CultureInfo.InvariantCulture),
                DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture),
                bool boolean => boolean ? "1" : "0",
                byte[] bytes => BitConverter.ToString(bytes).Replace("-", string.Empty),
                IFormattable formattable => Escape(formattable.ToString(null, CultureInfo.InvariantCulture)),
                _ => Escape(value.ToString()),
            };
        }

        /// <summary>
        /// Backslash-escapes the handful of characters <c>LOAD DATA</c> would otherwise misinterpret as field
        /// structure: a literal backslash, tab (the field terminator), and CR/LF (the line terminator).
        /// </summary>
        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value ?? string.Empty;
            }

            StringBuilder builder = null;
            for (var i = 0; i < value.Length; i++)
            {
                var ch = value[i];
                var escaped = ch switch
                {
                    '\\' => "\\\\",
                    '\t' => "\\t",
                    '\n' => "\\n",
                    '\r' => "\\r",
                    _ => null,
                };
                if (escaped != null)
                {
                    builder ??= new StringBuilder(value, 0, i, value.Length + 8);
                    builder.Append(escaped);
                }
                else
                {
                    builder?.Append(ch);
                }
            }
            return builder?.ToString() ?? value;
        }

        /// <summary>
        /// Runs the configured <see cref="MariaDbBulkLoader"/> against the temp file built by <see cref="WriteRow"/>, back-tick-quoting each mapped destination column along the way - the
        /// counterpart to <see cref="DestinationTableName"/> already arriving pre-quoted (see its remarks).
        /// </summary>
        private async Task<int> LoadAsync(string filePath,
            CancellationToken cancellationToken)
        {
            await ResolveDestinationOrdinalMappingsAsync(cancellationToken);

            var bulkLoader = new MariaDbBulkLoader(_connection)
            {
                TableName = DestinationTableName,
                FileName = filePath,
                Local = true,
                FieldTerminator = _fieldTerminator,
                LineTerminator = _lineTerminator,
                NumberOfLinesToSkip = 0,
                CharacterSet = "utf8mb4",
                Timeout = BulkCopyTimeout,
            };
            foreach (var mapping in ColumnMappings)
            {
                bulkLoader.Columns.Add(QuoteIdentifier(((MariaDbBulkColumnMapping)mapping).DestinationColumn));
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await bulkLoader.LoadAsync();
        }

        /// <summary>
        /// Resolves every mapping whose destination is expressed as a <see cref="MariaDbBulkColumnMapping.DestinationOrdinal"/>
        /// (i.e. <see cref="MariaDbBulkColumnMapping.DestinationColumn"/> is unset) into the actual column name at
        /// that ordinal position within <see cref="DestinationTableName"/>, since <see cref="LoadAsync"/> only
        /// ever emits <see cref="MariaDbBulkColumnMapping.DestinationColumn"/> to the underlying loader.
        /// </summary>
        private async Task ResolveDestinationOrdinalMappingsAsync(CancellationToken cancellationToken)
        {
            var needsResolution = false;
            foreach (MariaDbBulkColumnMapping mapping in ColumnMappings)
            {
                if (string.IsNullOrEmpty(mapping.DestinationColumn) && mapping.DestinationOrdinal >= 0)
                {
                    needsResolution = true;
                    break;
                }
            }
            if (!needsResolution)
            {
                return;
            }

            var columns = await GetDestinationColumnNamesAsync(cancellationToken);
            foreach (MariaDbBulkColumnMapping mapping in ColumnMappings)
            {
                if (string.IsNullOrEmpty(mapping.DestinationColumn) && mapping.DestinationOrdinal >= 0)
                {
                    if (mapping.DestinationOrdinal >= columns.Count)
                    {
                        throw new IndexOutOfRangeException(
                            $"Destination ordinal {mapping.DestinationOrdinal} is out of range for table '{DestinationTableName}', which has {columns.Count} column(s).");
                    }
                    mapping.DestinationColumn = columns[mapping.DestinationOrdinal];
                }
            }
        }

        /// <summary>
        /// Retrieves the column names of <see cref="DestinationTableName"/>, in physical ordinal order, via
        /// <c>SHOW COLUMNS</c>.
        /// </summary>
        private async Task<List<string>> GetDestinationColumnNamesAsync(CancellationToken cancellationToken)
        {
            var wasClosed = _connection.State == ConnectionState.Closed;
            if (wasClosed)
            {
                _connection.Open();
            }
            try
            {
                using var command = _connection.CreateCommand();
                command.CommandText = $"SHOW COLUMNS FROM {QuoteIdentifier(DestinationTableName?.Trim('`'))};";

                cancellationToken.ThrowIfCancellationRequested();
                var columns = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
                return columns;
            }
            finally
            {
                if (wasClosed)
                {
                    _connection.Close();
                }
            }
        }

        /// <summary>
        /// Back-tick-quotes a raw identifier, doubling any embedded back-tick per MySQL's identifier-quoting
        /// escape rule.
        /// </summary>
        private static string QuoteIdentifier(string identifier) =>
            $"`{identifier?.Replace("`", "``")}`";

        /// <summary>
        /// Selects the rows of <paramref name="table"/> whose <see cref="DataRow.RowState"/> matches any of the
        /// flags set in <paramref name="rowState"/>, mirroring <see cref="DataRowState"/>'s flags semantics.
        /// </summary>
        /// <param name="table">The table to select rows from.</param>
        /// <param name="rowState">The row state flags to match.</param>
        private static DataRow[] SelectRows(DataTable table, DataRowState rowState)
        {
            var rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                if ((row.RowState & rowState) != 0)
                {
                    rows.Add(row);
                }
            }
            return rows.ToArray();
        }

        /// <summary>
        /// Creates a new, empty temp file and returns its path.
        /// </summary>
        /// <returns>The path of the newly created temp file.</returns>
        private static string GetTempFilePath() => Path.GetTempFileName();

        /// <summary>
        /// Creates a UTF-8 (no byte order mark) <see cref="StreamWriter"/> that overwrites the given file.
        /// </summary>
        /// <param name="filePath">The path of the file to write to.</param>
        /// <returns>The newly created <see cref="StreamWriter"/>.</returns>
        private static StreamWriter CreateFileWriter(string filePath) =>
            new(filePath, false, new UTF8Encoding(false));

        /// <summary>
        /// Deletes the temp file at <paramref name="filePath"/> on a best-effort basis.
        /// </summary>
        /// <param name="filePath">The path of the temp file to delete.</param>
        private static void DeleteTempFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (IOException)
            {
                // Best-effort cleanup - a lingering temp file is a minor annoyance, not worth failing an
                // otherwise-successful (or already-failed) bulk load over.
            }
        }

        #endregion
    }
}
