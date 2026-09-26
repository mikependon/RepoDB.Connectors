#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests
{
    [TestClass]
    public sealed class AuroraDbConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderServerForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Server = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Server, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Port = 3306;

            // Assert
            Assert.AreEqual((uint)3306, builder.Port);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderDatabaseForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Database = "TestDb";

            // Assert
            Assert.AreEqual("TestDb", builder.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderUserIdForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.UserId = "root";

            // Assert
            Assert.AreEqual("root", builder.UserId, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPasswordForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Password = "password";

            // Assert
            Assert.AreEqual("password", builder.Password, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderServerForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

            // Act
            var builder = new AuroraDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Server, StringComparer.Ordinal);
        }
    }
}
