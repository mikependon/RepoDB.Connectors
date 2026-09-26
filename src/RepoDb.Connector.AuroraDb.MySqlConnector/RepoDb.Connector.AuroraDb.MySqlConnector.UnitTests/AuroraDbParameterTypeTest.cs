#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System.Data;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests
{
    /// <summary>
    /// Tests the <see cref="AuroraDbType"/> handling of <see cref="AuroraDbParameter"/>.
    /// </summary>
    [TestClass]
    public sealed class AuroraDbParameterTypeTest
    {
        #region Positive

        [TestMethod]
        [DataRow(AuroraDbType.Json, MySqlDbType.JSON)]
        [DataRow(AuroraDbType.BigInt, MySqlDbType.Int64)]
        [DataRow(AuroraDbType.VarChar, MySqlDbType.VarChar)]
        [DataRow(AuroraDbType.Decimal, MySqlDbType.Decimal)]
        [DataRow(AuroraDbType.DateTime, MySqlDbType.DateTime)]
        [DataRow(AuroraDbType.Point, MySqlDbType.Geometry)]
        public void TestAuroraDbParameterAuroraDbTypeForSetReflectsOnTheUnderlyingMySqlDbType(
            AuroraDbType auroraDbType,
            MySqlDbType expected)
        {
            // Setup
            var parameter = new AuroraDbParameter();

            // Act
            parameter.AuroraDbType = auroraDbType;

            // Assert
            Assert.AreEqual(expected, parameter.InnerParameter.MySqlDbType);
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

        #endregion
    }
}
