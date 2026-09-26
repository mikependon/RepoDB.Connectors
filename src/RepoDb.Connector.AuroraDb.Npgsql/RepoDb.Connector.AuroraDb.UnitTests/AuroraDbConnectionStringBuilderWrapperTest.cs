#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.UnitTests
{
    /// <summary>
    /// Tests the AWS wrapper related behaviors of <see cref="AuroraDbConnectionStringBuilder"/>.
    /// </summary>
    [TestClass]
    public sealed class AuroraDbConnectionStringBuilderWrapperTest
    {
        #region Positive

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPluginsForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Plugins = "failover,efm";

            // Assert
            Assert.AreEqual("failover,efm", builder.Plugins, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPluginsForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Database=TestDb;Plugins=failover;";

            // Act
            var builder = new AuroraDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("failover", builder.Plugins, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPluginsForDefaultValue()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            var output = builder.Plugins;

            // Assert
            Assert.IsNull(output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPortForDefaultValue()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            var output = builder.Port;

            // Assert
            Assert.AreEqual(AuroraDbConnectionStringBuilder.DefaultPort, output);
            Assert.AreEqual(5432, output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderDatabaseForDefaultValue()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            var output = builder.Database;

            // Assert
            Assert.IsNull(output);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPortForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Port=5433;Database=TestDb;";

            // Act
            var builder = new AuroraDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual(5433, builder.Port);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderConnectionStringForRoundTrip()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder
            {
                Host = "my-cluster.cluster-abc.eu-west-1.rds.amazonaws.com",
                Port = 5432,
                Database = "TestDb",
                Username = "postgres",
                Password = "password",
                Plugins = "failover,efm"
            };

            // Act
            var output = new AuroraDbConnectionStringBuilder(builder.ConnectionString);

            // Assert
            Assert.AreEqual(builder.Host, output.Host, StringComparer.Ordinal);
            Assert.AreEqual(builder.Port, output.Port);
            Assert.AreEqual(builder.Database, output.Database, StringComparer.Ordinal);
            Assert.AreEqual(builder.Username, output.Username, StringComparer.Ordinal);
            Assert.AreEqual(builder.Password, output.Password, StringComparer.Ordinal);
            Assert.AreEqual(builder.Plugins, output.Plugins, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderConnectionStringForUsableByConnection()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder
            {
                Host = "localhost",
                Database = "TestDb",
                Username = "postgres",
                Password = "password",
                Plugins = "executionTime"
            };

            // Act
            using var connection = new AuroraDbConnection(builder.ConnectionString);

            // Assert
            Assert.AreEqual("TestDb", connection.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderForNullConnectionString()
        {
            // Act
            var builder = new AuroraDbConnectionStringBuilder(null);

            // Assert - a null connection string is treated as an empty one
            Assert.IsNull(builder.Host);
            Assert.AreEqual(AuroraDbConnectionStringBuilder.DefaultPort, builder.Port);
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbConnectionStringBuilderForMalformedConnectionString()
        {
            // Act
            void Act() => new AuroraDbConnectionStringBuilder("this is not a connection string");

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        #endregion
    }
}
