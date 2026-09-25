#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbParameterTest
    {
        [TestMethod]
        public void TestCockcroachDbParameterParameterNameForGetSet()
        {
            // Setup
            var parameter = new CockcroachDbParameter();

            // Act
            parameter.ParameterName = "@Id";

            // Assert
            Assert.AreEqual("@Id", parameter.ParameterName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbParameterValueForGetSet()
        {
            // Setup
            var parameter = new CockcroachDbParameter();

            // Act
            parameter.Value = 100;

            // Assert
            Assert.AreEqual(100, parameter.Value);
        }

        [TestMethod]
        public void TestCockcroachDbParameterDirectionForDefaultValue()
        {
            // Setup
            var parameter = new CockcroachDbParameter();

            // Act
            var output = parameter.Direction;

            // Assert
            Assert.AreEqual(ParameterDirection.Input, output);
        }

        [TestMethod]
        public void TestCockcroachDbParameterIsNullableForDefaultValue()
        {
            // Setup
            var parameter = new CockcroachDbParameter();

            // Act
            var output = parameter.IsNullable;

            // Assert
            Assert.IsFalse(output);
        }

        [TestMethod]
        public void TestCockcroachDbParameterCockcroachDbTypeForGetSet()
        {
            // Setup
            var parameter = new CockcroachDbParameter();

            // Act
            parameter.CockcroachDbType = CockcroachDbType.BigInt;

            // Assert
            Assert.AreEqual(CockcroachDbType.BigInt, parameter.CockcroachDbType);
        }

        [TestMethod]
        public void TestCockcroachDbParameterResetDbTypeForForwardsToInnerParameter()
        {
            // Setup
            var parameter = new CockcroachDbParameter { Value = "hello", DbType = DbType.Int32 };

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
