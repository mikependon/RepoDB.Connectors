#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbFactoryTest
    {
        [TestMethod]
        public void TestCockroachDbFactoryInstanceForNotNull()
        {
            // Setup
            var factory = CockroachDbFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestCockroachDbFactoryCreateConnectionForReturnsCockroachDbConnection()
        {
            // Setup
            var factory = CockroachDbFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<CockroachDbConnection>(output);
        }

        [TestMethod]
        public void TestCockroachDbFactoryCreateCommandForReturnsCockroachDbCommand()
        {
            // Setup
            var factory = CockroachDbFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<CockroachDbCommand>(output);
        }

        [TestMethod]
        public void TestCockroachDbFactoryCreateParameterForReturnsCockroachDbParameter()
        {
            // Setup
            var factory = CockroachDbFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<CockroachDbParameter>(output);
        }

        [TestMethod]
        public void TestCockroachDbFactoryCreateConnectionStringBuilderForReturnsCockroachDbConnectionStringBuilder()
        {
            // Setup
            var factory = CockroachDbFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<CockroachDbConnectionStringBuilder>(output);
        }
    }
}
