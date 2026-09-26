#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using System.Data.Common;

namespace RepoDb.Connector.AuroraDb.UnitTests
{
    /// <summary>
    /// Tests how <see cref="AuroraDbCommand"/> behaves against a missing, or a not-yet-opened, connection.
    /// </summary>
    [TestClass]
    public sealed class AuroraDbCommandConnectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        #region Positive

        [TestMethod]
        public void TestAuroraDbCommandConnectionForConstructorWithConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);

            // Act
            using var command = new AuroraDbCommand("SELECT 1", connection);

            // Assert
            Assert.AreSame(connection, command.Connection);
            Assert.AreEqual("SELECT 1", command.CommandText, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbCommandConnectionForGetSet()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            command.Connection = connection;

            // Assert
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void TestAuroraDbCommandConnectionForSetNull()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var command = new AuroraDbCommand("SELECT 1", connection);

            // Act
            command.Connection = null;

            // Assert
            Assert.IsNull(command.Connection);
        }

        [TestMethod]
        public void TestAuroraDbCommandCreateParameterForReturnsAuroraDbParameter()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            var output = command.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<AuroraDbParameter>(output);
        }

        [TestMethod]
        public void TestAuroraDbCommandCommandTextForGetSet()
        {
            // Setup
            using var command = new AuroraDbCommand();

            // Act
            command.CommandText = "SELECT 2";

            // Assert
            Assert.AreEqual("SELECT 2", command.CommandText, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbCommandCommandTimeoutForGetSet()
        {
            // Setup
            using var command = new AuroraDbCommand();

            // Act
            command.CommandTimeout = 120;

            // Assert
            Assert.AreEqual(120, command.CommandTimeout);
        }

        [TestMethod]
        public void TestAuroraDbCommandParametersForSharesTheUnderlyingCollection()
        {
            // Setup
            using var command = new AuroraDbCommand();

            // Act
            command.Parameters.AddWithValue("@Id", 100L);

            // Assert
            Assert.HasCount(1, command.Parameters);
            Assert.AreSame(command.Parameters, ((DbCommand)command).Parameters);
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteNonQueryForCommandWithoutConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            void Act() => command.ExecuteNonQuery();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteScalarForCommandWithoutConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            void Act() => command.ExecuteScalar();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteReaderForCommandWithoutConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            void Act() => command.ExecuteReader();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteDbDataReaderForCommandWithoutConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            void Act() => ((DbCommand)command).ExecuteReader(CommandBehavior.SingleRow);

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandPrepareForCommandWithoutConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");

            // Act
            void Act() => command.Prepare();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteNonQueryForClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var command = new AuroraDbCommand("SELECT 1", connection);

            // Act
            void Act() => command.ExecuteNonQuery();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteScalarForClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            // Act
            void Act() => command.ExecuteScalar();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandExecuteReaderForClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(ConnectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            // Act
            void Act() => command.ExecuteReader();

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task ThrowOnAuroraDbCommandExecuteNonQueryAsyncForClosedConnection()
        {
            // Setup
            await using var connection = new AuroraDbConnection(ConnectionString);
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            // Act
            Task Act() => command.ExecuteNonQueryAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandConnectionForForeignDbConnection()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");
            using var foreign = new Npgsql.NpgsqlConnection(ConnectionString);

            // Act
            void Act() => ((DbCommand)command).Connection = foreign;

            // Assert
            Assert.ThrowsExactly<InvalidCastException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbCommandParametersForAddingForeignParameter()
        {
            // Setup
            using var command = new AuroraDbCommand("SELECT 1");
            var foreign = new Npgsql.NpgsqlParameter("@Id", 1);

            // Act
            void Act() => ((DbCommand)command).Parameters.Add(foreign);

            // Assert
            Assert.ThrowsExactly<InvalidCastException>(Act);
        }

        #endregion
    }
}
