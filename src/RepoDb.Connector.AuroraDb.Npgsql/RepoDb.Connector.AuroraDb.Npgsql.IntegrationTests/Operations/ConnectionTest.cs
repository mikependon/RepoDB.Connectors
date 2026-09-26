#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using Npgsql;
using RepoDb.Connector.AuroraDb.Npgsql.IntegrationTests.Setup;
using System.Data;

namespace RepoDb.Connector.AuroraDb.Npgsql.IntegrationTests.Operations
{
    [TestClass]
    public class ConnectionTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
        }

        #region Positive

        [TestMethod]
        public void TestAuroraDbConnectionOpenAndClose()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);

            // Act / Assert
            connection.Open();
            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public async Task TestAuroraDbConnectionOpenAndCloseAsync()
        {
            // Setup
            await using var connection = new AuroraDbConnection(Database.ConnectionString);

            // Act / Assert
            await connection.OpenAsync();
            Assert.AreEqual(ConnectionState.Open, connection.State);

            await connection.CloseAsync();
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestAuroraDbConnectionServerVersionAndDatabaseForOpenConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);

            // Act
            connection.Open();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(connection.ServerVersion));
            Assert.AreEqual("RepoDb.Connector.AuroraDb.Npgsql", connection.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionReopenAfterClose()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            connection.Close();

            // Act
            connection.Open();

            // Assert
            Assert.AreEqual(1, Helper.ExecuteScalar<int>(connection, "SELECT 1;"));
        }

        [TestMethod]
        public void TestAuroraDbConnectionWrappedConnectionUnwrapsToNpgsqlConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();

            // Act
            var wrapped = connection.WrappedConnection;

            // Assert - the connection is backed by the AWS wrapper, on top of Npgsql
            Assert.IsInstanceOfType<AwsWrapperConnection>(wrapped);
            Assert.IsTrue(wrapped.IsWrapperFor<NpgsqlConnection>());
            Assert.AreEqual(ConnectionState.Open, wrapped.Unwrap<NpgsqlConnection>().State);
        }

        [TestMethod]
        public void TestAuroraDbConnectionWithAwsWrapperPluginsInConnectionString()
        {
            // Setup - the AWS wrapper properties are accepted in the connection string, and stripped before reaching Npgsql
            var builder = new AuroraDbConnectionStringBuilder(Database.ConnectionString)
            {
                Plugins = "executionTime"
            };
            using var connection = new AuroraDbConnection(builder.ConnectionString);

            // Act
            connection.Open();

            // Assert
            Assert.AreEqual(1, Helper.ExecuteScalar<int>(connection, "SELECT 1;"));
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbConnectionOpenWithInvalidPassword()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder(Database.ConnectionString)
            {
                Password = "InvalidPassword"
            };
            using var connection = new AuroraDbConnection(builder.ConnectionString);

            // Act
            void Act() => connection.Open();

            // Assert
            var exception = Assert.Throws<PostgresException>(Act);
            Assert.AreEqual("28P01", exception.SqlState, StringComparer.Ordinal);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionOpenWithInvalidDatabase()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder(Database.ConnectionString)
            {
                Database = "DatabaseThatDoesNotExist"
            };
            using var connection = new AuroraDbConnection(builder.ConnectionString);

            // Act
            void Act() => connection.Open();

            // Assert
            var exception = Assert.Throws<PostgresException>(Act);
            Assert.AreEqual("3D000", exception.SqlState, StringComparer.Ordinal);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionOpenWithUnreachableHost()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder(Database.ConnectionString)
            {
                Host = "127.0.0.1",
                Port = 1
            };
            using var connection = new AuroraDbConnection(builder.ConnectionString);

            // Act
            void Act() => connection.Open();

            // Assert
            Assert.Throws<NpgsqlException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionOpenTwice()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();

            // Act
            void Act() => connection.Open();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionCommandExecuteOnClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1;";

            // Act
            void Act() => command.ExecuteScalar();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbConnectionExceptionCarriesSqlState()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM \"InvalidTable\";";

            // Act
            void Act() => command.ExecuteReader();

            // Assert - undefined_table
            var exception = Assert.Throws<AuroraDbException>(Act);
            Assert.AreEqual("42P01", exception.SqlState, StringComparer.Ordinal);
            Assert.IsInstanceOfType<PostgresException>(exception.InnerException);
        }

        #endregion
    }
}
