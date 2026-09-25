#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbFactoryTest
    {
        [TestMethod]
        public void TestCockcroachDbFactoryInstanceForNotNull()
        {
            // Setup
            var factory = CockcroachDbFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestCockcroachDbFactoryCreateConnectionForReturnsCockcroachDbConnection()
        {
            // Setup
            var factory = CockcroachDbFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbConnection>(output);
        }

        [TestMethod]
        public void TestCockcroachDbFactoryCreateCommandForReturnsCockcroachDbCommand()
        {
            // Setup
            var factory = CockcroachDbFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbCommand>(output);
        }

        [TestMethod]
        public void TestCockcroachDbFactoryCreateParameterForReturnsCockcroachDbParameter()
        {
            // Setup
            var factory = CockcroachDbFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbParameter>(output);
        }

        [TestMethod]
        public void TestCockcroachDbFactoryCreateConnectionStringBuilderForReturnsCockcroachDbConnectionStringBuilder()
        {
            // Setup
            var factory = CockcroachDbFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbConnectionStringBuilder>(output);
        }
    }
}
