#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDb.IntegrationTests.Setup;

namespace RepoDb.Connector.MariaDb.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteScalarTest
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
        public void TestMariaDbExecuteScalarTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteScalarTest'), (UUID(), 'ExecuteScalarTest'), (UUID(), 'ExecuteScalarTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                object result;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteScalarTest';";
                    result = command.ExecuteScalar();
                }

                // Assert
                Assert.AreEqual(3L, result);
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteScalarWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT COUNT(*) FROM `InvalidTable`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteScalar());
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteScalarWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC COUNT(*) FROM `InsertModel`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteScalar());
                }
            }
        }

        [TestMethod]
        public async Task TestMariaDbExecuteScalarAsyncTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteScalarAsyncTest'), (UUID(), 'ExecuteScalarAsyncTest'), (UUID(), 'ExecuteScalarAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                object result;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteScalarAsyncTest';";
                    result = await command.ExecuteScalarAsync();
                }

                // Assert
                Assert.AreEqual(3L, result);
            }
        }

        [TestMethod]
        public async Task ThrowOnMariaDbExecuteScalarAsyncWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT COUNT(*) FROM `InvalidTable`;";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<MariaDbException>(() => command.ExecuteScalarAsync());
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnMariaDbExecuteScalarAsyncWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC COUNT(*) FROM `InsertModel`;";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<MariaDbException>(() => command.ExecuteScalarAsync());
                }
            }
        }
    }
}
