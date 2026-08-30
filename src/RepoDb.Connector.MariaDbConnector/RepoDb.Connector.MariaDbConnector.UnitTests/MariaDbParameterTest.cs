#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;

namespace RepoDb.Connector.MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbParameterTest
    {
        [TestMethod]
        public void TestMariaDbParameterParameterNameForGetSet()
        {
            // Setup
            var parameter = new MariaDbParameter();

            // Act
            parameter.ParameterName = "@Id";

            // Assert
            Assert.AreEqual("@Id", parameter.ParameterName);
        }

        [TestMethod]
        public void TestMariaDbParameterValueForGetSet()
        {
            // Setup
            var parameter = new MariaDbParameter();

            // Act
            parameter.Value = 100;

            // Assert
            Assert.AreEqual(100, parameter.Value);
        }

        [TestMethod]
        public void TestMariaDbParameterDirectionForDefaultValue()
        {
            // Setup
            var parameter = new MariaDbParameter();

            // Act
            var output = parameter.Direction;

            // Assert
            Assert.AreEqual(ParameterDirection.Input, output);
        }

        [TestMethod]
        public void TestMariaDbParameterIsNullableForDefaultValue()
        {
            // Setup
            var parameter = new MariaDbParameter();

            // Act
            var output = parameter.IsNullable;

            // Assert
            Assert.IsFalse(output);
        }

        [TestMethod]
        public void TestMariaDbParameterMariaDbTypeForGetSet()
        {
            // Setup
            var parameter = new MariaDbParameter();

            // Act
            parameter.MariaDbType = MariaDbType.BigInt;

            // Assert
            Assert.AreEqual(MariaDbType.BigInt, parameter.MariaDbType);
        }

        [TestMethod]
        public void TestMariaDbParameterResetDbTypeForForwardsToInnerParameter()
        {
            // Setup
            var parameter = new MariaDbParameter { Value = "hello", DbType = DbType.Int32 };

            // Act
            parameter.ResetDbType();

            // Assert
            Assert.AreEqual(DbType.String, parameter.DbType);
        }
    }
}
