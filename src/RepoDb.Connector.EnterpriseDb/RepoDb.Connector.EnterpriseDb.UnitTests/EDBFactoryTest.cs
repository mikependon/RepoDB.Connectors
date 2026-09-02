#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBFactoryTest
    {
        [TestMethod]
        public void TestEDBFactoryInstanceForNotNull()
        {
            // Setup
            var factory = EDBFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestEDBFactoryCreateConnectionForReturnsEDBConnection()
        {
            // Setup
            var factory = EDBFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<EDBConnection>(output);
        }

        [TestMethod]
        public void TestEDBFactoryCreateCommandForReturnsEDBCommand()
        {
            // Setup
            var factory = EDBFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<EDBCommand>(output);
        }

        [TestMethod]
        public void TestEDBFactoryCreateParameterForReturnsEDBParameter()
        {
            // Setup
            var factory = EDBFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<EDBParameter>(output);
        }

        [TestMethod]
        public void TestEDBFactoryCreateConnectionStringBuilderForReturnsEDBConnectionStringBuilder()
        {
            // Setup
            var factory = EDBFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<EDBConnectionStringBuilder>(output);
        }
    }
}
