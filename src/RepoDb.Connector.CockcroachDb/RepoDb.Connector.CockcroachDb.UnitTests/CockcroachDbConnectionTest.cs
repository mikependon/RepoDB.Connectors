#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbConnectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestCockcroachDbConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new CockcroachDbConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("tcp://localhost:5432", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new CockcroachDbConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new CockcroachDbConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionCreateCommandForReturnsCockcroachDbCommand()
        {
            // Setup
            using var connection = new CockcroachDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbCommand>(output);
        }

        [TestMethod]
        public async Task TestCockcroachDbConnectionOpenAsyncForCancelledTokenThrowsOperationCanceledException()
        {
            // Setup
            using var connection = new CockcroachDbConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
        }
    }
}
