#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using System.Data;

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests
{
    /// <summary>
    /// Tests the <see cref="AuroraDbType"/> handling of <see cref="AuroraDbParameter"/>.
    /// </summary>
    [TestClass]
    public sealed class AuroraDbParameterTypeTest
    {
        #region Positive

        [TestMethod]
        [DataRow(AuroraDbType.Json, NpgsqlDbType.Json)]
        [DataRow(AuroraDbType.Jsonb, NpgsqlDbType.Jsonb)]
        [DataRow(AuroraDbType.Money, NpgsqlDbType.Money)]
        [DataRow(AuroraDbType.Xml, NpgsqlDbType.Xml)]
        [DataRow(AuroraDbType.Cidr, NpgsqlDbType.Cidr)]
        [DataRow(AuroraDbType.Point, NpgsqlDbType.Point)]
        [DataRow(AuroraDbType.IntegerRange, NpgsqlDbType.IntegerRange)]
        public void TestAuroraDbParameterAuroraDbTypeForSetReflectsOnTheUnderlyingNpgsqlDbType(
            AuroraDbType auroraDbType,
            NpgsqlDbType expected)
        {
            // Setup
            var parameter = new AuroraDbParameter();

            // Act
            parameter.AuroraDbType = auroraDbType;

            // Assert
            Assert.AreEqual(expected, parameter.InnerParameter.NpgsqlDbType);
            Assert.AreEqual(auroraDbType, parameter.AuroraDbType);
        }

        [TestMethod]
        public void TestAuroraDbParameterAuroraDbTypeForDbTypeInference()
        {
            // Setup
            var parameter = new AuroraDbParameter
            {
                DbType = DbType.Int64
            };

            // Act
            var output = parameter.AuroraDbType;

            // Assert
            Assert.AreEqual(AuroraDbType.BigInt, output);
        }

        [TestMethod]
        public void TestAuroraDbParameterSizeForGetSet()
        {
            // Setup
            var parameter = new AuroraDbParameter();

            // Act
            parameter.Size = 100;

            // Assert
            Assert.AreEqual(100, parameter.Size);
        }

        [TestMethod]
        public void TestAuroraDbParameterSourceColumnForGetSet()
        {
            // Setup
            var parameter = new AuroraDbParameter();

            // Act
            parameter.SourceColumn = "Name";

            // Assert
            Assert.AreEqual("Name", parameter.SourceColumn, StringComparer.Ordinal);
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbParameterAuroraDbTypeForUndefinedValue()
        {
            // Setup
            var parameter = new AuroraDbParameter();

            // Act
            void Act() => parameter.AuroraDbType = (AuroraDbType)int.MaxValue;

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbParameterAuroraDbTypeForUnsupportedNpgsqlDbType()
        {
            // Setup
            var parameter = new AuroraDbParameter();
            parameter.InnerParameter.NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Integer;

            // Act
            void Act() => _ = parameter.AuroraDbType;

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion
    }
}
