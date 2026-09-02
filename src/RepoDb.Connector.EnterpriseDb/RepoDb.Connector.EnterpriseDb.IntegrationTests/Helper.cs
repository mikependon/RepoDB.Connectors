#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb;
using System.Data;

namespace RepoDb.Connector.EnterpriseDb.IntegrationTests
{
    /// <summary>
    /// A helper class for the integration testing.
    /// </summary>
    public static class Helper
    {
        static Helper()
        {
            EpocDate = new DateTime(1970, 1, 1, 0, 0, 0);
        }

        /// <summary>
        /// Gets the epoc date used as the baseline when generating deterministic date/time values for the test data.
        /// </summary>
        public static DateTime EpocDate { get; }

        #region InsertModel

        /*
         * Actual Class
         */

        /// <summary>
        ///
        /// </summary>
        /// <param name="count"></param>
        /// <param name="hasId"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(int count,
            bool hasId = false)
        {
            var table = new DataTable("InsertModel");

            if (hasId)
            {
                table.Columns.Add("Id", typeof(long));
            }
            table.Columns.Add("RowGuid", typeof(Guid));
            table.Columns.Add("ColumnBit", typeof(short));
            table.Columns.Add("ColumnDateTime", typeof(DateTime));
            table.Columns.Add("ColumnDateTime2", typeof(DateTime));
            table.Columns.Add("ColumnDecimal", typeof(decimal));
            table.Columns.Add("ColumnFloat", typeof(double));
            table.Columns.Add("ColumnInt", typeof(int));
            table.Columns.Add("ColumnNVarChar", typeof(string));

            for (var i = 0; i < count; i++)
            {
                var row = table.NewRow();
                if (hasId)
                {
                    row["Id"] = i + 1;
                }
                row["RowGuid"] = Guid.NewGuid();
                row["ColumnBit"] = (short)(i % 2);
                row["ColumnDateTime"] = EpocDate.AddDays(i);
                row["ColumnDateTime2"] = EpocDate.AddDays(i).AddSeconds(i);
                row["ColumnDecimal"] = i * 100M;
                row["ColumnFloat"] = i * 100D;
                row["ColumnInt"] = i;
                row["ColumnNVarChar"] = $"ColumnNVarChar{i}";
                table.Rows.Add(row);
            }

            return table;
        }

        #endregion

        #region Helpers

        /// <summary>
        ///
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="commandText"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal static void ExecuteNonQuery(
            EDBConnection connection,
            string commandText)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="commandText"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal static T ExecuteScalar<T>(
            EDBConnection connection,
            string commandText)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                var result = command.ExecuteScalar();
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static int CountRows(
            EDBConnection connection,
            string tableName)
        {
            return ExecuteScalar<int>(connection, $"SELECT COUNT(*) FROM \"{tableName}\";");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="tableName"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        internal static int CountRowsWhere(
            EDBConnection connection,
            string tableName,
            string whereClause)
        {
            return ExecuteScalar<int>(connection, $"SELECT COUNT(*) FROM \"{tableName}\" WHERE {whereClause};");
        }

        #endregion
    }
}
