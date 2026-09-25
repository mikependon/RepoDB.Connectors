#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbCommandTest
    {
        [TestMethod]
        public void TestCockcroachDbCommandCommandTextForConstructorWithCommandText()
        {
            // Setup
            using var command = new CockcroachDbCommand("SELECT 1");

            // Act
            var output = command.CommandText;

            // Assert
            Assert.AreEqual("SELECT 1", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbCommandCommandTimeoutForDefaultValue()
        {
            // Setup
            using var command = new CockcroachDbCommand();

            // Act
            var output = command.CommandTimeout;

            // Assert
            Assert.AreEqual(30, output);
        }

        [TestMethod]
        public void TestCockcroachDbCommandCommandTypeForDefaultValue()
        {
            // Setup
            using var command = new CockcroachDbCommand();

            // Act
            var output = command.CommandType;

            // Assert
            Assert.AreEqual(CommandType.Text, output);
        }

        [TestMethod]
        public void TestCockcroachDbCommandParametersForNewCommand()
        {
            // Setup
            using var command = new CockcroachDbCommand();

            // Act
            var output = command.Parameters;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestCockcroachDbCommandDesignTimeVisibleForGetSet()
        {
            // Setup
            using var command = new CockcroachDbCommand();

            // Act
            command.DesignTimeVisible = false;

            // Assert
            Assert.IsFalse(command.DesignTimeVisible);
        }

        [TestMethod]
        public void TestCockcroachDbCommandUpdatedRowSourceForGetSet()
        {
            // Setup
            using var command = new CockcroachDbCommand();

            // Act
            command.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            // Assert
            Assert.AreEqual(UpdateRowSource.FirstReturnedRecord, command.UpdatedRowSource);
        }

        [TestMethod]
        public async Task TestCockcroachDbCommandExecuteNonQueryAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockcroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteNonQueryAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestCockcroachDbCommandExecuteScalarAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockcroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteScalarAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestCockcroachDbCommandExecuteReaderAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new CockcroachDbCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteReaderAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }
    }
}
