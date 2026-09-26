#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests
{
    [TestClass]
    public sealed class AuroraDbFactoryTest
    {
        [TestMethod]
        public void TestAuroraDbFactoryInstanceForNotNull()
        {
            // Setup
            var factory = AuroraDbFactory.Instance;

            // Act
            var output = factory;

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void TestAuroraDbFactoryCreateConnectionForReturnsAuroraDbConnection()
        {
            // Setup
            var factory = AuroraDbFactory.Instance;

            // Act
            using var output = factory.CreateConnection();

            // Assert
            Assert.IsInstanceOfType<AuroraDbConnection>(output);
        }

        [TestMethod]
        public void TestAuroraDbFactoryCreateCommandForReturnsAuroraDbCommand()
        {
            // Setup
            var factory = AuroraDbFactory.Instance;

            // Act
            using var output = factory.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<AuroraDbCommand>(output);
        }

        [TestMethod]
        public void TestAuroraDbFactoryCreateParameterForReturnsAuroraDbParameter()
        {
            // Setup
            var factory = AuroraDbFactory.Instance;

            // Act
            var output = factory.CreateParameter();

            // Assert
            Assert.IsInstanceOfType<AuroraDbParameter>(output);
        }

        [TestMethod]
        public void TestAuroraDbFactoryCreateConnectionStringBuilderForReturnsAuroraDbConnectionStringBuilder()
        {
            // Setup
            var factory = AuroraDbFactory.Instance;

            // Act
            var output = factory.CreateConnectionStringBuilder();

            // Assert
            Assert.IsInstanceOfType<AuroraDbConnectionStringBuilder>(output);
        }
    }
}
