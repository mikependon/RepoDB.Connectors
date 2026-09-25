#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockroachDb.Bulk;
using RepoDb.Connector.CockroachDb.IntegrationTests.Setup;
using System.Data;

namespace RepoDb.Connector.CockroachDb.IntegrationTests.Operations
{
    [TestClass]
    public class WriteToServerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerTest()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithSourceColumnAndDestinationColumnMappings()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithSourceOrdinalAndDestinationColumnMappings()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(i, table.Columns[i].ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithSourceColumnAndDestinationOrdinalMappings()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(table.Columns[i].ColumnName, i + 1);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithSourceOrdinalAndDestinationOrdinalMappings()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(i, i + 1);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void ThrowOnCockroachDbBulkCopyWriteToServerWithOutOfRangeDestinationOrdinal()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                bulkCopy.ColumnMappings.Add(0, 9);

                // Act & Assert
                Assert.Throws<IndexOutOfRangeException>(() => bulkCopy.WriteToServer(table));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithDataRowArray()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var rows = new DataRow[table.Rows.Count];
                table.Rows.CopyTo(rows, 0);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(rows);

                // Assert
                Assert.AreEqual(rows.Length, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithDataRowArraySubset()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var allRows = new DataRow[table.Rows.Count];
                table.Rows.CopyTo(allRows, 0);
                var subset = new[] { allRows[2], allRows[5], allRows[8] };
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(subset);

                // Assert
                Assert.AreEqual(subset.Length, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 5 AND \"ColumnNVarChar\" = 'ColumnNVarChar5'"));
                Assert.AreEqual(0, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 1"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithEmptyDataRowArray()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(0);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(Array.Empty<DataRow>());

                // Assert
                Assert.AreEqual(0, Helper.CountRows(connection, table.TableName));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithNullDataRowArray()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(0);
                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer((DataRow[])null);

                // Assert
                Assert.AreEqual(0, Helper.CountRows(connection, table.TableName));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithDataTableAddedRowState()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(5);
                table.AcceptChanges();

                var newRow = table.NewRow();
                newRow["RowGuid"] = Guid.NewGuid();
                newRow["ColumnBit"] = (short)1;
                newRow["ColumnDateTime"] = Helper.EpocDate;
                newRow["ColumnDateTime2"] = Helper.EpocDate;
                newRow["ColumnDecimal"] = 999M;
                newRow["ColumnFloat"] = 999D;
                newRow["ColumnInt"] = 999;
                newRow["ColumnNVarChar"] = "ColumnNVarChar999";
                table.Rows.Add(newRow);

                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table, DataRowState.Added);

                // Assert
                Assert.AreEqual(1, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 999 AND \"ColumnNVarChar\" = 'ColumnNVarChar999'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithDataTableModifiedRowState()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(5);
                table.AcceptChanges();
                table.Rows[2]["ColumnInt"] = 12345;
                table.Rows[2]["ColumnNVarChar"] = "Updated";

                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table, DataRowState.Modified);

                // Assert
                Assert.AreEqual(1, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 12345 AND \"ColumnNVarChar\" = 'Updated'"));
            }
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyWriteToServerWithDataTableUnchangedRowState()
        {
            using (var connection = new CockroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(5);
                table.AcceptChanges();
                table.Rows[0]["ColumnInt"] = 555;

                var bulkCopy = new CockroachDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table, DataRowState.Unchanged);

                // Assert
                Assert.AreEqual(4, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(0, Helper.CountRowsWhere(connection, table.TableName,
                    "\"ColumnInt\" = 555"));
            }
        }
    }
}
