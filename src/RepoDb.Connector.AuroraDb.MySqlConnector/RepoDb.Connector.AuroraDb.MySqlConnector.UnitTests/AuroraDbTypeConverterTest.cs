#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests
{
    [TestClass]
    public sealed class AuroraDbTypeConverterTest
    {
        [TestMethod]
        public void TestAuroraDbTypeConverterToMySqlDbTypeForBigInt()
        {
            // Setup
            var input = AuroraDbType.BigInt;

            // Act
            var output = AuroraDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Int64, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToAuroraDbTypeForVarChar()
        {
            // Setup
            var input = MySqlDbType.VarChar;

            // Act
            var output = AuroraDbTypeConverter.ToAuroraDbType(input);

            // Assert
            Assert.AreEqual(AuroraDbType.VarChar, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToAuroraDbTypeForGuid()
        {
            // Setup
            var input = MySqlDbType.Guid;

            // Act
            var output = AuroraDbTypeConverter.ToAuroraDbType(input);

            // Assert
            Assert.AreEqual(AuroraDbType.Binary, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToMySqlDbTypeForPoint()
        {
            // Setup
            var input = AuroraDbType.Point;

            // Act
            var output = AuroraDbTypeConverter.ToMySqlDbType(input);

            // Assert
            Assert.AreEqual(MySqlDbType.Geometry, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToAuroraDbTypeForVectorThrowsNotSupportedException()
        {
            // Setup
            var input = MySqlDbType.Vector;

            // Act
            void Act() => AuroraDbTypeConverter.ToAuroraDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }
    }
}
