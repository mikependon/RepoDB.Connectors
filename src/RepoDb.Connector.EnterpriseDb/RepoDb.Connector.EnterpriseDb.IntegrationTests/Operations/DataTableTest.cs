#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb.IntegrationTests.Setup;
using System.Data;
using System.Data.Common;

namespace RepoDb.Connector.EnterpriseDb.IntegrationTests.Operations
{
    [TestClass]
    public class DataTableTest
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
        public void TestEDBDataTableLoadFromDataReader()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnInt\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 1, 'DataTableLoad1'), (gen_random_uuid(), 2, 'DataTableLoad2');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" IN ('DataTableLoad1', 'DataTableLoad2') " +
                        "ORDER BY \"ColumnInt\";";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                Assert.HasCount(2, table.Rows);
                Assert.AreEqual(1, table.Rows[0]["ColumnInt"]);
                Assert.AreEqual("DataTableLoad1", table.Rows[0]["ColumnNVarChar"]);
                Assert.AreEqual(2, table.Rows[1]["ColumnInt"]);
                Assert.AreEqual("DataTableLoad2", table.Rows[1]["ColumnNVarChar"]);
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromDataReaderCreatesMatchingColumns()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM \"InsertModel\";";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                var expectedColumns = new[]
                {
                    "Id", "RowGuid", "ColumnBit", "ColumnDateTime", "ColumnDateTime2",
                    "ColumnDecimal", "ColumnFloat", "ColumnInt", "ColumnNVarChar"
                };
                foreach (var expectedColumn in expectedColumns)
                {
                    Assert.IsTrue(table.Columns.Contains(expectedColumn));
                }
                Assert.AreEqual(typeof(long), table.Columns["Id"].DataType);
                Assert.AreEqual(typeof(int), table.Columns["ColumnInt"].DataType);
                Assert.AreEqual(typeof(string), table.Columns["ColumnNVarChar"].DataType);
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromDataReaderWithNullValues()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnInt\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), NULL, 'DataTableLoadNull');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'DataTableLoadNull';";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                Assert.HasCount(1, table.Rows);
                Assert.AreEqual(DBNull.Value, table.Rows[0]["ColumnInt"]);
                Assert.AreEqual(DBNull.Value, table.Rows[0]["ColumnBit"]);
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromDataReaderWithEmptyResultSet()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM \"InsertModel\" WHERE 1 = 0;";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                Assert.IsEmpty(table.Rows);
                Assert.IsTrue(table.Columns.Contains("ColumnNVarChar"));
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromDataReaderWithMultipleResultSets()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'MultiResultSet1'), (gen_random_uuid(), 'MultiResultSet2'), (gen_random_uuid(), 'MultiResultSet3');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var firstTable = new DataTable();
                var secondTable = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT \"ColumnNVarChar\" FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'MultiResultSet1'; " +
                        "SELECT \"ColumnNVarChar\" FROM \"InsertModel\" WHERE \"ColumnNVarChar\" LIKE 'MultiResultSet%';";
                    using (var reader = command.ExecuteReader())
                    {
                        // Load() advances the reader to the next result set once the current one is exhausted.
                        firstTable.Load(reader);
                        secondTable.Load(reader);
                    }
                }

                // Assert
                Assert.HasCount(1, firstTable.Rows);
                Assert.AreEqual("MultiResultSet1", firstTable.Rows[0]["ColumnNVarChar"]);
                Assert.HasCount(3, secondTable.Rows);
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromDataReaderTwiceMergesRows()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"Id\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), DEFAULT, 'DataTableLoadFirstBatch');";
                    insertCommand.ExecuteNonQuery();
                }

                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'DataTableLoadFirstBatch';";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                table.PrimaryKey = new[] { table.Columns["Id"] };

                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'DataTableLoadSecondBatch');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM \"InsertModel\" " +
                        "WHERE \"ColumnNVarChar\" IN ('DataTableLoadFirstBatch', 'DataTableLoadSecondBatch');";
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                Assert.HasCount(2, table.Rows);
            }
        }

        [TestMethod]
        public async Task TestEDBDataTableLoadFromDataReaderAsync()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'DataTableLoadAsync');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                var table = new DataTable();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'DataTableLoadAsync';";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        table.Load(reader);
                    }
                }

                // Assert
                Assert.HasCount(1, table.Rows);
                Assert.AreEqual("DataTableLoadAsync", table.Rows[0]["ColumnNVarChar"]);
            }
        }

        [TestMethod]
        public void TestEDBDataTableLoadFromClosedDataReaderIsNoOp()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                DbDataReader reader;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM \"InsertModel\";";
                    reader = command.ExecuteReader();
                    reader.Dispose();
                }

                // Act
                var table = new DataTable();
                table.Load(reader);

                // Assert - loading from an already-closed reader is a safe no-op, not an error
                Assert.IsTrue(reader.IsClosed);
                Assert.IsEmpty(table.Rows);
                Assert.IsEmpty(table.Columns);
            }
        }
    }
}
