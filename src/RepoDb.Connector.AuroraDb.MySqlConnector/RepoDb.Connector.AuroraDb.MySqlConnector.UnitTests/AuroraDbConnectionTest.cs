#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests
{
    [TestClass]
    public sealed class AuroraDbConnectionTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestAuroraDbConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("localhost", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionCreateCommandForReturnsAuroraDbCommand()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<AuroraDbCommand>(output);
        }

        [TestMethod]
        public async Task TestAuroraDbConnectionOpenAsyncForCancelledTokenThrowsOperationCanceledException()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
        }
    }
}
