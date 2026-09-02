#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.EnterpriseDb.Bulk
{
    /// <summary>
    /// Lets you efficiently bulk load an EnterpriseDB table with data from another source.
    /// </summary>
    public class EDBBulkCopy : IDisposable
    {
        private readonly EDBConnection _connection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBBulkCopy"/> class using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the destination database.</param>
        public EDBBulkCopy(
            string connectionString)
            : this(new EDBConnection(connectionString))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBBulkCopy"/> class using the specified <see cref="EDBConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="EDBConnection"/> to the destination database.</param>
        public EDBBulkCopy(
            EDBConnection connection)
        {
            ColumnMappings = new EDBBulkCopyColumnMappingCollection();
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of column mappings that determine which source columns are written to which destination columns.
        /// </summary>
        public EDBBulkCopyColumnMappingCollection ColumnMappings { get; private set; }

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
        /// Releases the resources used by the <see cref="EDBBulkCopy"/>.
        /// </summary>
        public void Dispose() => _connection.Dispose();

        /// <summary>
        /// Copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        public int WriteToServer(
            IDataReader reader) =>
            WriteToServerAsync(reader, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        public int WriteToServer(
            DbDataReader reader) =>
            WriteToServerAsync(reader, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        public int WriteToServer(
            DataTable table) =>
            WriteToServerAsync(table, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Copies only rows that match the supplied row state in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="rowState">A value from the <see cref="DataRowState"/> enumeration used to filter which rows are copied.</param>
        public int WriteToServer(
            DataTable table,
            DataRowState rowState) =>
            WriteToServerAsync(table, rowState, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        public int WriteToServer(
            DataRow[] rows) =>
            WriteToServerAsync(rows, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task<int> WriteToServerAsync(
            IDataReader reader,
            CancellationToken cancellationToken = default)
        {
            int ResolveSourceOrdinal(EDBBulkColumnMapping mapping) => reader.GetOrdinal(mapping.SourceColumn);

            async Task WriteRowsAsync(List<(int SourceOrdinal, string DestinationColumn)> mappings, NpgsqlBinaryImporter importer, CancellationToken ct)
            {
                while (reader.Read())
                {
                    var values = new object[mappings.Count];
                    for (var i = 0; i < mappings.Count; i++)
                    {
                        var value = reader.GetValue(mappings[i].SourceOrdinal);
                        values[i] = value is DBNull ? null : value;
                    }
                    await importer.WriteRowAsync(ct, values).ConfigureAwait(false);
                }
            }

            RowsCopied = await ExecuteAsync(ResolveSourceOrdinal, WriteRowsAsync, cancellationToken);
            return RowsCopied;
        }

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public Task<int> WriteToServerAsync(
            DbDataReader reader,
            CancellationToken cancellationToken = default) =>
            WriteToServerAsync((IDataReader)reader, cancellationToken);

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task<int> WriteToServerAsync(
            DataTable table,
            CancellationToken cancellationToken = default)
        {
            int ResolveSourceOrdinal(EDBBulkColumnMapping mapping) => table.Columns.IndexOf(mapping.SourceColumn);

            async Task WriteRowsAsync(List<(int SourceOrdinal, string DestinationColumn)> mappings, NpgsqlBinaryImporter importer, CancellationToken ct)
            {
                foreach (DataRow row in table.Rows)
                {
                    await WriteDataRowAsync(importer, mappings, row, ct).ConfigureAwait(false);
                }
            }

            RowsCopied = await ExecuteAsync(ResolveSourceOrdinal, WriteRowsAsync, cancellationToken);
            return RowsCopied;
        }

        /// <summary>
        /// Asynchronously copies only rows that match the supplied row state in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="rowState">A value from the <see cref="DataRowState"/> enumeration used to filter which rows are copied.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public Task<int> WriteToServerAsync(
            DataTable table,
            DataRowState rowState,
            CancellationToken cancellationToken = default) =>
            WriteToServerAsync(SelectRows(table, rowState), cancellationToken);

        /// <summary>
        /// Asynchronously copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task<int> WriteToServerAsync(
            DataRow[] rows,
            CancellationToken cancellationToken = default)
        {
            if (rows == null || rows.Length == 0)
            {
                RowsCopied = 0;
                return RowsCopied;
            }

            int ResolveSourceOrdinal(EDBBulkColumnMapping mapping) =>
                rows[0].Table.Columns.IndexOf(mapping.SourceColumn);

            async Task WriteRowsAsync(List<(int SourceOrdinal, string DestinationColumn)> mappings, NpgsqlBinaryImporter importer, CancellationToken ct)
            {
                foreach (var row in rows)
                {
                    await WriteDataRowAsync(importer, mappings, row, ct).ConfigureAwait(false);
                }
            }

            RowsCopied = await ExecuteAsync(ResolveSourceOrdinal, WriteRowsAsync, cancellationToken);
            return RowsCopied;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures the connection is open, builds the (source ordinal, destination column) pairs from
        /// <see cref="ColumnMappings"/>, streams rows into a <see cref="NpgsqlBinaryImporter"/> opened via
        /// <c>COPY ... FROM STDIN (FORMAT BINARY)</c> using <paramref name="writeRowsAsync"/>, and returns
        /// the number of rows inserted.
        /// </summary>
        private async Task<int> ExecuteAsync(
            Func<EDBBulkColumnMapping, int> resolveSourceOrdinal,
            Func<List<(int SourceOrdinal, string DestinationColumn)>, NpgsqlBinaryImporter, CancellationToken, Task> writeRowsAsync,
            CancellationToken cancellationToken)
        {
            var wasClosed = _connection.State == ConnectionState.Closed;
            if (wasClosed)
            {
                await _connection.OpenAsync(cancellationToken);
            }
            try
            {
                var columnMappings = await BuildColumnMappingsAsync(resolveSourceOrdinal, cancellationToken);

                var columnList = string.Join(", ", columnMappings.Select(mapping => QuoteIdentifier(mapping.DestinationColumn)));
                var copyCommand = $"COPY {QuoteIdentifier(DestinationTableName)} ({columnList}) FROM STDIN (FORMAT BINARY)";

                await using var importer = await _connection.InnerConnection.BeginBinaryImportAsync(copyCommand, cancellationToken).ConfigureAwait(false);

                if (BulkCopyTimeout > 0)
                {
                    importer.Timeout = TimeSpan.FromSeconds(BulkCopyTimeout);
                }

                await writeRowsAsync(columnMappings, importer, cancellationToken).ConfigureAwait(false);

                return (int)await importer.CompleteAsync(cancellationToken).ConfigureAwait(false);
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
        /// Writes a single <see cref="DataRow"/> to the <paramref name="importer"/>, translating <see cref="DBNull"/> values to <c>null</c>.
        /// </summary>
        private static Task WriteDataRowAsync(
            NpgsqlBinaryImporter importer,
            List<(int SourceOrdinal, string DestinationColumn)> mappings,
            DataRow row,
            CancellationToken cancellationToken)
        {
            var values = new object[mappings.Count];
            for (var i = 0; i < mappings.Count; i++)
            {
                var value = row[mappings[i].SourceOrdinal];
                values[i] = value is DBNull ? null : value;
            }
            return importer.WriteRowAsync(cancellationToken, values);
        }

        /// <summary>
        /// Translates <see cref="ColumnMappings"/> - which may refer to source/destination columns by name or by
        /// ordinal - into the concrete (source ordinal, destination column name) pairs that the underlying
        /// <c>COPY</c> command requires, resolving destination ordinals via <c>information_schema.columns</c>
        /// only if at least one mapping needs it.
        /// </summary>
        private async Task<List<(int SourceOrdinal, string DestinationColumn)>> BuildColumnMappingsAsync(
            Func<EDBBulkColumnMapping, int> resolveSourceOrdinal,
            CancellationToken cancellationToken)
        {
            List<string> destinationColumns = null;
            var mappings = new List<(int SourceOrdinal, string DestinationColumn)>(ColumnMappings.Count);

            foreach (EDBBulkColumnMapping mapping in ColumnMappings)
            {
                var sourceOrdinal = mapping.SourceOrdinal >= 0 ? mapping.SourceOrdinal : resolveSourceOrdinal(mapping);

                var destinationColumn = mapping.DestinationColumn;
                if (string.IsNullOrEmpty(destinationColumn))
                {
                    destinationColumns ??= await GetDestinationColumnNamesAsync(cancellationToken);
                    if (mapping.DestinationOrdinal < 0 || mapping.DestinationOrdinal >= destinationColumns.Count)
                    {
                        throw new IndexOutOfRangeException(
                            $"Destination ordinal {mapping.DestinationOrdinal} is out of range for table '{DestinationTableName}', which has {destinationColumns.Count} column(s).");
                    }
                    destinationColumn = destinationColumns[mapping.DestinationOrdinal];
                }

                mappings.Add((sourceOrdinal, destinationColumn));
            }

            return mappings;
        }

        /// <summary>
        /// Retrieves the column names of <see cref="DestinationTableName"/>, in physical ordinal order, via <c>information_schema.columns</c>.
        /// </summary>
        private async Task<List<string>> GetDestinationColumnNamesAsync(CancellationToken cancellationToken)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT column_name FROM information_schema.columns WHERE table_name = @tableName ORDER BY ordinal_position;";
            command.Parameters.AddWithValue("@tableName", UnquoteIdentifier(DestinationTableName));

            var columns = new List<string>();
            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    columns.Add(reader.GetString(0));
                }
            }
            return columns;
        }

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
        /// Double-quotes a raw identifier, doubling any embedded double-quote per PostgreSQL's identifier-quoting escape rule.
        /// </summary>
        private static string QuoteIdentifier(string identifier) =>
            $"\"{UnquoteIdentifier(identifier)?.Replace("\"", "\"\"")}\"";

        /// <summary>
        /// Strips a leading/trailing pair of double-quotes from an identifier, if present.
        /// </summary>
        private static string UnquoteIdentifier(string identifier) =>
            identifier?.Trim('"');

        #endregion
    }
}
