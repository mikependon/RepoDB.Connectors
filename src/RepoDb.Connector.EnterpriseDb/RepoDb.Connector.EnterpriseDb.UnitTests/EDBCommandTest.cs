#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBCommandTest
    {
        [TestMethod]
        public void TestEDBCommandCommandTextForConstructorWithCommandText()
        {
            // Setup
            using var command = new EDBCommand("SELECT 1");

            // Act
            var output = command.CommandText;

            // Assert
            Assert.AreEqual("SELECT 1", output);
        }

        [TestMethod]
        public void TestEDBCommandCommandTimeoutForDefaultValue()
        {
            // Setup
            using var command = new EDBCommand();

            // Act
            var output = command.CommandTimeout;

            // Assert
            Assert.AreEqual(30, output);
        }

        [TestMethod]
        public void TestEDBCommandCommandTypeForDefaultValue()
        {
            // Setup
            using var command = new EDBCommand();

            // Act
            var output = command.CommandType;

            // Assert
            Assert.AreEqual(CommandType.Text, output);
        }

        [TestMethod]
        public void TestEDBCommandParametersForNewCommand()
        {
            // Setup
            using var command = new EDBCommand();

            // Act
            var output = command.Parameters;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestEDBCommandDesignTimeVisibleForGetSet()
        {
            // Setup
            using var command = new EDBCommand();

            // Act
            command.DesignTimeVisible = false;

            // Assert
            Assert.IsFalse(command.DesignTimeVisible);
        }

        [TestMethod]
        public void TestEDBCommandUpdatedRowSourceForGetSet()
        {
            // Setup
            using var command = new EDBCommand();

            // Act
            command.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

            // Assert
            Assert.AreEqual(UpdateRowSource.FirstReturnedRecord, command.UpdatedRowSource);
        }

        [TestMethod]
        public async Task TestEDBCommandExecuteNonQueryAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new EDBCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteNonQueryAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestEDBCommandExecuteScalarAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new EDBCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteScalarAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }

        [TestMethod]
        public async Task TestEDBCommandExecuteReaderAsyncForCommandWithoutConnectionThrowsInvalidOperationException()
        {
            // Setup
            using var command = new EDBCommand("SELECT 1");

            // Act
            Task Act() => command.ExecuteReaderAsync();

            // Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(Act);
        }
    }
}
