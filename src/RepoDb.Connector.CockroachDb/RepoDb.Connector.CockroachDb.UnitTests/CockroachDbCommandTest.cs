#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbCommandTest
    {
        [TestMethod]
        public void TestCockroachDbCommandCommandTextForConstructorWithCommandText()
        {
            // Setup
            using var command = new CockroachDbCommand("SELECT 1");

            // Act
            var output = command.CommandText;

            // Assert
            Assert.AreEqual("SELECT 1", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbCommandCommandTimeoutForDefaultValue()
        {
            // Setup
            using var command = new CockroachDbCommand();

            // Act
            var output = command.CommandTimeout;

            // Assert
            Assert.AreEqual(30, output);
        }

        [TestMethod]
        public void TestCockroachDbCommandCommandTypeForDefaultValue()
        {
            // Setup
            using var command = new CockroachDbCommand();

            // Act
            var output = command.CommandType;

            // Assert
            Assert.AreEqual(CommandType.Text, output);
        }

        [TestMethod]
        public void TestCockroachDbCommandParametersForNewCommand()
        {
            // Setup
            using var command = new CockroachDbCommand();

            // Act
            var output = command.Parameters;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestCockroachDbCommandDesignTimeVisibleForGetSet()
        {
            // Setup
            using var command = new CockroachDbCommand();

            // Act
            command.DesignTimeVisible = false;

            // Assert
            Assert.IsFalse(command.DesignTimeVisible);
        }

        [TestMethod]
        public void TestCockroachDbCommandUpdatedRowSourceForGetSet()
        {
            // Setup
            using var command = new CockroachDbCommand();

            // Act
            command.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            // Assert
            Assert.AreEqual(UpdateRowSource.FirstReturnedRecord, command.UpdatedRowSource);
        }

        [TestMethod]
        public async Task TestCockroachDbCommandExecuteNonQueryAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteNonQueryAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestCockroachDbCommandExecuteScalarAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteScalarAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestCockroachDbCommandExecuteReaderAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteReaderAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }
    }
}
