#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBParameterTest
    {
        [TestMethod]
        public void TestEDBParameterParameterNameForGetSet()
        {
            // Setup
            var parameter = new EDBParameter();

            // Act
            parameter.ParameterName = "@Id";

            // Assert
            Assert.AreEqual("@Id", parameter.ParameterName);
        }

        [TestMethod]
        public void TestEDBParameterValueForGetSet()
        {
            // Setup
            var parameter = new EDBParameter();

            // Act
            parameter.Value = 100;

            // Assert
            Assert.AreEqual(100, parameter.Value);
        }

        [TestMethod]
        public void TestEDBParameterDirectionForDefaultValue()
        {
            // Setup
            var parameter = new EDBParameter();

            // Act
            var output = parameter.Direction;

            // Assert
            Assert.AreEqual(ParameterDirection.Input, output);
        }

        [TestMethod]
        public void TestEDBParameterIsNullableForDefaultValue()
        {
            // Setup
            var parameter = new EDBParameter();

            // Act
            var output = parameter.IsNullable;

            // Assert
            Assert.IsFalse(output);
        }

        [TestMethod]
        public void TestEDBParameterEDBTypeForGetSet()
        {
            // Setup
            var parameter = new EDBParameter();

            // Act
            parameter.EDBType = EDBType.BigInt;

            // Assert
            Assert.AreEqual(EDBType.BigInt, parameter.EDBType);
        }

        [TestMethod]
        public void TestEDBParameterResetDbTypeForForwardsToInnerParameter()
        {
            // Setup
            var parameter = new EDBParameter { Value = "hello", DbType = DbType.Int32 };

            // Act
            parameter.ResetDbType();

            // Assert - Npgsql's ResetDbType() clears the explicitly-set DbType rather than re-inferring it
            // from Value (unlike MySqlConnector's MariaDbParameter counterpart); the exact fallback value can
            // vary depending on when the global type mapper is initialized, so only the "no longer Int32" part
            // of the contract is asserted here.
            Assert.AreNotEqual(DbType.Int32, parameter.DbType);
        }
    }
}
