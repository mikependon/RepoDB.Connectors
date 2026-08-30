#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDbConnector.IntegrationTests.Setup;

namespace RepoDb.Connector.MariaDbConnector.IntegrationTests.Operations
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
        public void TestMariaDbExecuteReaderTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteReaderTest'), (UUID(), 'ExecuteReaderTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var rowCount = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteReaderTest';";
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
        public void ThrowOnMariaDbExecuteReaderWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT * FROM `InvalidTable`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteReader());
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteReaderWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC * FROM `InsertModel`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteReader());
                }
            }
        }

        [TestMethod]
        public async Task TestMariaDbExecuteReaderAsyncTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteReaderAsyncTest'), (UUID(), 'ExecuteReaderAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                var rowCount = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteReaderAsyncTest';";
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
        public async Task ThrowOnMariaDbExecuteReaderAsyncWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT * FROM `InvalidTable`;";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<MariaDbException>(() => command.ExecuteReaderAsync());
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnMariaDbExecuteReaderAsyncWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC * FROM `InsertModel`;";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<MariaDbException>(() => command.ExecuteReaderAsync());
                }
            }
        }

        [TestMethod]
        public async Task TestMariaDbDataReaderNextResultAsyncForMultipleResultSets()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'NextResultAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT `ColumnNVarChar` FROM `InsertModel` WHERE `ColumnNVarChar` = 'NextResultAsyncTest'; " +
                        "SELECT COUNT(*) AS `Total` FROM `InsertModel` WHERE `ColumnNVarChar` = 'NextResultAsyncTest';";
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
        public async Task TestMariaDbDataReaderIsDBNullAsyncForNullAndNonNullValues()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'IsDBNullAsyncTest'), (UUID(), NULL);";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT `ColumnNVarChar` FROM `InsertModel` " +
                        "WHERE `ColumnNVarChar` = 'IsDBNullAsyncTest' OR `ColumnNVarChar` IS NULL " +
                        "ORDER BY `ColumnNVarChar` IS NULL;";
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
