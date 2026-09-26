#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider.Driver.Exceptions;
using Npgsql;
using System.Data.Common;

namespace RepoDb.Connector.AuroraDb.UnitTests
{
    [TestClass]
    public sealed class AuroraDbExceptionTest
    {
        [TestMethod]
        public void TestAuroraDbExceptionMessageForNpgsqlException()
        {
            // Setup
            var inner = new NpgsqlException("Connection failed");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual("Connection failed", output.Message, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbExceptionInnerExceptionForNpgsqlException()
        {
            // Setup
            var inner = new NpgsqlException("Connection failed");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreSame(inner, output.InnerException);
        }

        [TestMethod]
        public void TestAuroraDbExceptionIsDbException()
        {
            // Act
            var output = new AuroraDbException(new NpgsqlException("Error"));

            // Assert
            Assert.IsInstanceOfType<DbException>(output);
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
        public void TestAuroraDbExceptionSqlStateForNpgsqlExceptionWithoutSqlState()
        {
            // Act
            var output = new AuroraDbException(new NpgsqlException("Error"));

            // Assert - a client-side (non-PostgresException) error carries no SQL state
            Assert.IsNull(output.SqlState);
        }

        [TestMethod]
        public void TestAuroraDbExceptionSqlStateForPostgresException()
        {
            // Setup
            var inner = new PostgresException("relation does not exist", "ERROR", "ERROR", "42P01");

            // Act
            var output = new AuroraDbException(inner);

            // Assert
            Assert.AreEqual("42P01", output.SqlState, StringComparer.Ordinal);
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
