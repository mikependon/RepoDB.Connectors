#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbConnectionTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestMariaDbConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("localhost", output);
        }

        [TestMethod]
        public void TestMariaDbConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output);
        }

        [TestMethod]
        public void TestMariaDbConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestMariaDbConnectionCreateCommandForReturnsMariaDbCommand()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<MariaDbCommand>(output);
        }

        [TestMethod]
        public async Task TestMariaDbConnectionOpenAsyncForCancelledTokenThrowsOperationCanceledException()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
        }
    }
}
