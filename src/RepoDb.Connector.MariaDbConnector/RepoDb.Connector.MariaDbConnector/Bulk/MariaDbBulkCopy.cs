#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.MariaDbConnector.Bulk
{
    /// <summary>
    /// Lets you efficiently bulk load a MariaDB table with data from another source.
    /// </summary>
    public class MariaDbBulkCopy : IDisposable
    {
        private readonly MariaDbConnection _connection;

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
            int ResolveSourceOrdinal(MariaDbBulkColumnMapping mapping) => reader.GetOrdinal(mapping.SourceColumn);
            RowsCopied = await ExecuteAsync(
                ResolveSourceOrdinal,
                bulkCopy => bulkCopy.WriteToServerAsync(reader, cancellationToken),
                cancellationToken);
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
            int ResolveSourceOrdinal(MariaDbBulkColumnMapping mapping) => table.Columns.IndexOf(mapping.SourceColumn);

            RowsCopied = await ExecuteAsync(
                ResolveSourceOrdinal,
                bulkCopy => bulkCopy.WriteToServerAsync(table, cancellationToken),
                cancellationToken);
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

            var columnCount = rows[0].Table.Columns.Count;

            int ResolveSourceOrdinal(MariaDbBulkColumnMapping mapping) =>
                rows[0].Table.Columns.IndexOf(mapping.SourceColumn);

            RowsCopied = await ExecuteAsync(
                ResolveSourceOrdinal,
                bulkCopy => bulkCopy.WriteToServerAsync(rows, columnCount, cancellationToken),
                cancellationToken);
            return RowsCopied;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures the connection is open, builds the <see cref="MySqlBulkCopyColumnMapping"/> list from
        /// <see cref="ColumnMappings"/>, runs <paramref name="writeAsync"/> against a freshly configured
        /// <see cref="MySqlBulkCopy"/>, and returns the number of rows inserted.
        /// </summary>
        private async Task<int> ExecuteAsync(
            Func<MariaDbBulkColumnMapping, int> resolveSourceOrdinal,
            Func<MySqlBulkCopy, ValueTask<MySqlBulkCopyResult>> writeAsync,
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

                var bulkCopy = new MySqlBulkCopy(_connection.InnerConnection, null)
                {
                    DestinationTableName = DestinationTableName,
                    BulkCopyTimeout = BulkCopyTimeout,
                };
                bulkCopy.ColumnMappings.AddRange(columnMappings);

                var result = await writeAsync(bulkCopy);
                return result.RowsInserted;
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
        /// Translates <see cref="ColumnMappings"/> - which may refer to source/destination columns by name or by
        /// ordinal - into the concrete (source ordinal, destination column name) pairs that
        /// <see cref="MySqlBulkCopyColumnMapping"/> requires, resolving destination ordinals via <c>SHOW COLUMNS</c>
        /// only if at least one mapping needs it.
        /// </summary>
        private async Task<List<MySqlBulkCopyColumnMapping>> BuildColumnMappingsAsync(
            Func<MariaDbBulkColumnMapping, int> resolveSourceOrdinal,
            CancellationToken cancellationToken)
        {
            List<string> destinationColumns = null;
            var mappings = new List<MySqlBulkCopyColumnMapping>(ColumnMappings.Count);

            foreach (MariaDbBulkColumnMapping mapping in ColumnMappings)
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

                mappings.Add(new MySqlBulkCopyColumnMapping
                {
                    SourceOrdinal = sourceOrdinal,
                    DestinationColumn = destinationColumn,
                });
            }

            return mappings;
        }

        /// <summary>
        /// Retrieves the column names of <see cref="DestinationTableName"/>, in physical ordinal order, via <c>SHOW COLUMNS</c>.
        /// </summary>
        private async Task<List<string>> GetDestinationColumnNamesAsync(CancellationToken cancellationToken)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SHOW COLUMNS FROM {QuoteIdentifier(DestinationTableName?.Trim('`'))};";

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
        /// Back-tick-quotes a raw identifier, doubling any embedded back-tick per MySQL's identifier-quoting
        /// escape rule.
        /// </summary>
        private static string QuoteIdentifier(string identifier) =>
            $"`{identifier?.Replace("`", "``")}`";

        #endregion
    }
}
