#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockcroachDb.IntegrationTests.Setup;

namespace RepoDb.Connector.CockcroachDb.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteNonQueryTest
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
        public void TestCockcroachDbExecuteNonQueryTest()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE \"InsertModel\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void ThrowOnCockcroachDbExecuteNonQueryWithInvalidTable()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE \"InvalidTable\";";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<CockcroachDbException>(() => command.ExecuteNonQuery());
                }
            }
        }

        [TestMethod]
        public void ThrowOnCockcroachDbExecuteNonQueryWithInvalidSyntax()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE Invalid Table;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<CockcroachDbException>(() => command.ExecuteNonQuery());
                }
            }
        }

        [TestMethod]
        public async Task TestCockcroachDbExecuteNonQueryAsyncTest()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE \"InsertModel\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnCockcroachDbExecuteNonQueryAsyncWithInvalidTable()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE \"InvalidTable\";";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<CockcroachDbException>(() => command.ExecuteNonQueryAsync());
                }
            }
        }

        [TestMethod]
        public async Task ThrowOnCockcroachDbExecuteNonQueryAsyncWithInvalidSyntax()
        {
            using (var connection = new CockcroachDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE Invalid Table;";

                // Act
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Assert.ThrowsAsync<CockcroachDbException>(() => command.ExecuteNonQueryAsync());
                }
            }
        }
    }
}
