#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBConnectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestEDBConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new EDBConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("tcp://localhost:5432", output);
        }

        [TestMethod]
        public void TestEDBConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new EDBConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output);
        }

        [TestMethod]
        public void TestEDBConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new EDBConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestEDBConnectionCreateCommandForReturnsEDBCommand()
        {
            // Setup
            using var connection = new EDBConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<EDBCommand>(output);
        }

        [TestMethod]
        public async Task TestEDBConnectionOpenAsyncForCancelledTokenThrowsOperationCanceledException()
        {
            // Setup
            using var connection = new EDBConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
        }
    }
}
