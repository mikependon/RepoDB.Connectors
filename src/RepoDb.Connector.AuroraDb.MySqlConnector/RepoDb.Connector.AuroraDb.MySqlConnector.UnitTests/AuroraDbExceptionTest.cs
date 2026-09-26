#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider.Driver.Exceptions;
using MySqlConnector;
using System.Data.Common;
using System.Reflection;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests
{
    [TestClass]
    public sealed class AuroraDbExceptionTest
    {
        /// <summary>
        /// The constructors of <see cref="MySqlException"/> are internal, hence it is created through reflection.
        /// </summary>
        private static MySqlException CreateMySqlException(
            params object[] arguments)
        {
            return (MySqlException)Activator.CreateInstance(
                typeof(MySqlException),
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                arguments,
                culture: null);
        }

        [TestMethod]
        public void TestAuroraDbExceptionMessageForMySqlException()
        {
            // Setup
            var inner = CreateMySqlException("Connection failed");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual("Connection failed", output.Message, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbExceptionInnerExceptionForMySqlException()
        {
            // Setup
            var inner = CreateMySqlException("Connection failed");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreSame(inner, output.InnerException);
        }

        [TestMethod]
        public void TestAuroraDbExceptionIsDbException()
        {
            // Act
            var output = new AuroraDbException(CreateMySqlException("Error"));

            // Assert
            Assert.IsInstanceOfType<DbException>(output);
        }

        [TestMethod]
        public void TestAuroraDbExceptionNumberForMySqlException()
        {
            // Setup
            var inner = CreateMySqlException(MySqlErrorCode.NoSuchTable, "Table does not exist");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual((int)MySqlErrorCode.NoSuchTable, output.Number);
            Assert.AreEqual((int)MySqlErrorCode.NoSuchTable, output.ErrorCode);
        }

        [TestMethod]
        public void TestAuroraDbExceptionSqlStateForMySqlException()
        {
            // Setup
            var inner = CreateMySqlException(MySqlErrorCode.NoSuchTable, "42S02", "Table does not exist");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual("42S02", output.SqlState, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbExceptionSqlStateForAwsWrapperDbException()
        {
            // Setup
            var inner = new AwsWrapperDbException("Wrapper failure", "08006");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual("08006", output.SqlState, StringComparer.Ordinal);
            Assert.AreEqual("Wrapper failure", output.Message, StringComparer.Ordinal);
            Assert.AreSame(inner, output.InnerException);
        }

        [TestMethod]
        public void ThrowOnAuroraDbExceptionForNullException()
        {
            // Act
            void Act() => new AuroraDbException(null);

            // Assert
            Assert.ThrowsExactly<NullReferenceException>(Act);
        }
    }
}
