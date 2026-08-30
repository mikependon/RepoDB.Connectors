#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDb.UnitTests
{
    [TestClass]
    public sealed class MariaDbProviderFactoryTest
    {
        [TestMethod]
        public void TestMariaDbProviderFactoryInstanceForNotNull()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateConnectionForReturnsMariaDbConnection()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<MariaDbConnection>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateCommandForReturnsMariaDbCommand()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<MariaDbCommand>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateParameterForReturnsMariaDbParameter()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<MariaDbParameter>(output);
        }

        [TestMethod]
        public void TestMariaDbProviderFactoryCreateConnectionStringBuilderForReturnsMariaDbConnectionStringBuilder()
        {
            // Setup
            var factory = MariaDbProviderFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<MariaDbConnectionStringBuilder>(output);
        }
    }
}
