#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb.IntegrationTests.Setup;

namespace RepoDb.Connector.EnterpriseDb.IntegrationTests.Operations
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
        public void TestEDBExecuteScalarTest()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'ExecuteScalarTest'), (gen_random_uuid(), 'ExecuteScalarTest'), (gen_random_uuid(), 'ExecuteScalarTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                object result;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'ExecuteScalarTest';";
                    result = command.ExecuteScalar();
                }

                // Assert
                Assert.AreEqual(3L, result);
            }
        }

        [TestMethod]
        public void ThrowOnEDBExecuteScalarWithInvalidTable()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT COUNT(*) FROM \"InvalidTable\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<EDBException>(() => command.ExecuteScalar());
                }
            }
        }

        [TestMethod]
        public void ThrowOnEDBExecuteScalarWithInvalidSyntax()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC COUNT(*) FROM \"InsertModel\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<EDBException>(() => command.ExecuteScalar());
                }
            }
        }

        [TestMethod]
        public async Task TestEDBExecuteScalarAsyncTest()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                await connection.OpenAsync();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES " +
                        "(gen_random_uuid(), 'ExecuteScalarAsyncTest'), (gen_random_uuid(), 'ExecuteScalarAsyncTest'), (gen_random_uuid(), 'ExecuteScalarAsyncTest');";
                    await insertCommand.ExecuteNonQueryAsync();
                }

                // Act
                object result;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM \"InsertModel\" WHERE \"ColumnNVarChar\" = 'ExecuteScalarAsyncTest';";
                    result = await command.ExecuteScalarAsync();
                }

                // Assert
                Assert.AreEqual(3L, result);
            }
        }

        [TestMethod]
        public async Task ThrowOnEDBExecuteScalarAsyncWithInvalidTable()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT COUNT(*) FROM \"InvalidTable\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<EDBException>(() => command.ExecuteScalarAsync());
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnEDBExecuteScalarAsyncWithInvalidSyntax()
        {
            using (var connection = new EDBConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC COUNT(*) FROM \"InsertModel\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<EDBException>(() => command.ExecuteScalarAsync());
                }
            }
        }
    }
}
