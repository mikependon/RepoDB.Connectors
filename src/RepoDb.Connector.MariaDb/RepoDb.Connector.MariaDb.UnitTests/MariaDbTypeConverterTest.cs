#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySql.Data.MySqlClient;

namespace RepoDb.Connector.MariaDb.UnitTests
{
    [TestClass]
    public sealed class MariaDbTypeConverterTest
    {
        [TestMethod]
        public void TestMariaDbTypeConverterToMySqlDbTypeForBigInt()
        {
            // Setup
            var input = MariaDbType.BigInt;

            // Act
            var output = MariaDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Int64, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForVarChar()
        {
            // Setup
            var input = MySqlDbType.VarChar;

            // Act
            var output = MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.AreEqual(MariaDbType.VarChar, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForGuid()
        {
            // Setup
            var input = MySqlDbType.Guid;

            // Act
            var output = MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.AreEqual(MariaDbType.Binary, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMySqlDbTypeForPoint()
        {
            // Setup
            var input = MariaDbType.Point;

            // Act
            var output = MariaDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Geometry, output);
        }

        [TestMethod]
        public void TestMariaDbTypeConverterToMariaDbTypeForVectorThrowsNotSupportedException()
        {
            // Setup
            var input = MySqlDbType.Vector;

            // Act
            void Act() => MariaDbTypeConverter.ToMariaDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }
    }
}
