#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.MariaDb.UnitTests
{
    [TestClass]
    public sealed class MariaDbCommandTest
    {
        [TestMethod]
        public void TestMariaDbCommandCommandTextForConstructorWithCommandText()
        {
            // Setup
            using var command = new MariaDbCommand("SELECT 1");

            // Act
            var output = command.CommandText;

            // Assert
            Assert.AreEqual("SELECT 1", output);
        }

        [TestMethod]
        public void TestMariaDbCommandCommandTimeoutForDefaultValue()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.CommandTimeout;

            // Assert
            Assert.AreEqual(30, output);
        }

        [TestMethod]
        public void TestMariaDbCommandCommandTypeForDefaultValue()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.CommandType;

            // Assert
            Assert.AreEqual(CommandType.Text, output);
        }

        [TestMethod]
        public void TestMariaDbCommandParametersForNewCommand()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            var output = command.Parameters;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestMariaDbCommandDesignTimeVisibleForGetSet()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            command.DesignTimeVisible = false;

            // Assert
            Assert.IsFalse(command.DesignTimeVisible);
        }

        [TestMethod]
        public void TestMariaDbCommandUpdatedRowSourceForGetSet()
        {
            // Setup
            using var command = new MariaDbCommand();

            // Act
            command.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            // Assert
            Assert.AreEqual(UpdateRowSource.FirstReturnedRecord, command.UpdatedRowSource);
        }

        [TestMethod]
        public async Task TestMariaDbCommandExecuteNonQueryAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new MariaDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteNonQueryAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestMariaDbCommandExecuteScalarAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new MariaDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteScalarAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestMariaDbCommandExecuteReaderAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new MariaDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteReaderAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }
    }
}
