#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbConnectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestCockroachDbConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new CockroachDbConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("tcp://localhost:5432", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new CockroachDbConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new CockroachDbConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestCockroachDbConnectionCreateCommandForReturnsCockroachDbCommand()
        {
            // Setup
            using var connection = new CockroachDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<CockroachDbCommand>(output);
        }

        [TestMethod]
        public async Task TestCockroachDbConnectionOpenAsyncForCancelledTokenThrowsOperationCanceledException()
        {
            // Setup
            using var connection = new CockroachDbConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
        }
    }
}
