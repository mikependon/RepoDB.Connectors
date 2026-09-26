#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using Npgsql;
using System.Data;

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests
{
    /// <summary>
    /// Tests the behaviors of <see cref="AuroraDbConnection"/> that are provided by (or guarded on top of) the AWS wrapper.
    /// </summary>
    [TestClass]
    public sealed class AuroraDbConnectionWrapperTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        #region Positive

        [TestMethod]
        public void TestAuroraDbConnectionWrappedConnectionForNotNull()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var output = connection.WrappedConnection;

            // Assert
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType<AwsWrapperConnection<NpgsqlConnection>>(output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionWrappedConnectionForUnwrapToNpgsqlConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var output = connection.WrappedConnection.Unwrap<NpgsqlConnection>();

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionWrappedConnectionForIsWrapperFor()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act / Assert
            Assert.IsTrue(connection.WrappedConnection.IsWrapperFor<NpgsqlConnection>());
        }

        [TestMethod]
        public void TestAuroraDbConnectionConnectionStringForGetSet()
        {
            // Setup
            using var connection = new AuroraDbConnection();

            // Act
            connection.ConnectionString = ConnectionString;

            // Assert
            Assert.AreEqual("TestDb", connection.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStateForDefaultConstructor()
        {
            // Setup
            using var connection = new AuroraDbConnection();

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionCreateCommandForAssociatesTheConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.AreSame(connection, output.Connection);
        }

        [TestMethod]
        public void TestAuroraDbConnectionCloseForNewConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            connection.Close();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public async Task TestAuroraDbConnectionCloseAsyncForNewConnection()
        {
            // Setup
            await using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            await connection.CloseAsync();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestAuroraDbConnectionDisposeForMultipleCalls()
        {
            // Setup
            var connection = new AuroraDbConnection(ConnectionString);

            // Act
            connection.Dispose();
            connection.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbConnectionBeginTransactionForClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            void Act() => connection.BeginTransaction();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionBeginTransactionWithIsolationLevelForClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            void Act() => connection.BeginTransaction(IsolationLevel.Serializable);

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task ThrowOnAuroraDbConnectionOpenAsyncForCancelledTokenBeforeAttemptingToConnect()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            Task Act() => connection.OpenAsync(cts.Token);

            // Assert - the connection attempt (and the wrapper retries) must never be made
            await Assert.ThrowsExactlyAsync<OperationCanceledException>(Act);
            Assert.IsLessThan(2000, stopwatch.ElapsedMilliseconds);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionConnectionStringForMalformedValue()
        {
            // Setup
            using var connection = new AuroraDbConnection();

            // Act
            void Act() => connection.ConnectionString = "this is not a connection string";

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionOpenForUnreachableHost()
        {
            // Setup
            using var connection = new AuroraDbConnection("Host=127.0.0.1;Port=1;Database=TestDb;Username=postgres;Password=password;Timeout=2;");

            // Act
            void Act() => connection.Open();

            // Assert
            Assert.Throws<NpgsqlException>(Act);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        #endregion
    }
}
