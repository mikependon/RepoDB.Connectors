#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBTypeConverterTest
    {
        [TestMethod]
        public void TestEDBTypeConverterToNpgsqlDbTypeForBigInt()
        {
            // Setup
            var input = EDBType.BigInt;

            // Act
            var output = EDBTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(NpgsqlDbType.Bigint, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToEDBTypeForVarchar()
        {
            // Setup
            var input = NpgsqlDbType.Varchar;

            // Act
            var output = EDBTypeConverter.ToEDBType(input);

            // Assert
            Assert.AreEqual(EDBType.VarChar, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToEDBTypeForJsonb()
        {
            // Setup
            var input = NpgsqlDbType.Jsonb;

            // Act
            var output = EDBTypeConverter.ToEDBType(input);

            // Assert
            Assert.AreEqual(EDBType.Jsonb, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToNpgsqlDbTypeForPoint()
        {
            // Setup
            var input = EDBType.Point;

            // Act
            var output = EDBTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(NpgsqlDbType.Point, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToEDBTypeForJsonPath()
        {
            // Setup
            var input = NpgsqlDbType.JsonPath;

            // Act
            var output = EDBTypeConverter.ToEDBType(input);

            // Assert
            Assert.AreEqual(EDBType.JsonPath, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToNpgsqlDbTypeForNumericRange()
        {
            // Setup
            var input = EDBType.NumericRange;

            // Act
            var output = EDBTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(NpgsqlDbType.NumericRange, output);
        }

        [TestMethod]
        public void TestEDBTypeConverterToEDBTypeForArrayThrowsNotSupportedException()
        {
            // Setup
            var input = NpgsqlDbType.Array;

            // Act
            void Act() => EDBTypeConverter.ToEDBType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }
    }
}
