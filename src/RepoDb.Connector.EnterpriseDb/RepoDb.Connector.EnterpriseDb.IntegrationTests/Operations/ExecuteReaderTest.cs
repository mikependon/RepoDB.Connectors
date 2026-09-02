#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb.IntegrationTests.Setup;

namespace RepoDb.Connector.EnterpriseDb.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteReaderTest
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
        public void TestEDBExecuteReaderTest()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'ExecuteReaderTest'), (gen_random_uuid(), 'ExecuteReaderTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var rowCount = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'ExecuteReaderTest';";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rowCount++;
                        }
                    }
                }

                // Assert
                Assert.AreEqual(2, rowCount);
            }
        }

        [TestMethod]
        public void ThrowOnEDBExecuteReaderWithInvalidTable()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT * FROM \"InvalidTable\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<EDBException>(() => command.ExecuteReader());
                }
            }
        }

        [TestMethod]
        public void ThrowOnEDBExecuteReaderWithInvalidSyntax()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC * FROM \"InsertModel\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<EDBException>(() => command.ExecuteReader());
                }
            }
        }

        [TestMethod]
        public async Task TestEDBExecuteReaderAsyncTest()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'ExecuteReaderAsyncTest'), (gen_random_uuid(), 'ExecuteReaderAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                var rowCount = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'ExecuteReaderAsyncTest';";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            rowCount++;
                        }
                    }
                }

                // Assert
                Assert.AreEqual(2, rowCount);
            }
        }

        [TestMethod]
        public async Task ThrowOnEDBExecuteReaderAsyncWithInvalidTable()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT * FROM \"InvalidTable\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<EDBException>(() => command.ExecuteReaderAsync());
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnEDBExecuteReaderAsyncWithInvalidSyntax()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC * FROM \"InsertModel\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<EDBException>(() => command.ExecuteReaderAsync());
                }
            }
        }

        [TestMethod]
        public async Task TestEDBDataReaderNextResultAsyncForMultipleResultSets()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'NextResultAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT \"ColumnNVarChar\" FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'NextResultAsyncTest'; " +
                        "SELECT COUNT(*) AS \"Total\" FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'NextResultAsyncTest';";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Assert - first result set
                        Assert.IsTrue(await reader.ReadAsync());
                        Assert.AreEqual("NextResultAsyncTest", reader.GetString(0));

                        // Act & Assert - move to the second result set
                        Assert.IsTrue(await reader.NextResultAsync());
                        Assert.IsTrue(await reader.ReadAsync());
                        Assert.AreEqual(1L, reader.GetInt64(0));

                        // Assert - no further result sets
                        Assert.IsFalse(await reader.NextResultAsync());
                    }
                }
            }
        }

        [TestMethod]
        public async Task TestEDBDataReaderIsDBNullAsyncForNullAndNonNullValues()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'IsDBNullAsyncTest'), (gen_random_uuid(), NULL);";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT \"ColumnNVarChar\" FROM \"InsertModel\" " +
                        "WHERE \"ColumnNVarChar\" = 'IsDBNullAsyncTest' OR \"ColumnNVarChar\" IS NULL " +
                        "ORDER BY \"ColumnNVarChar\" IS NULL;";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Assert - non-null row
                        Assert.IsTrue(await reader.ReadAsync());
                        Assert.IsFalse(await reader.IsDBNullAsync(0));

                        // Assert - null row
                        Assert.IsTrue(await reader.ReadAsync());
                        Assert.IsTrue(await reader.IsDBNullAsync(0));
                    }
                }
            }
        }
    }
}
