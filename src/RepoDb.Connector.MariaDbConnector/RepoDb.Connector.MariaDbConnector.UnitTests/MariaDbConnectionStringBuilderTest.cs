#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDbConnector.UnitTests
{
    [TestClass]
    public sealed class MariaDbConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestMariaDbConnectionStringBuilderServerForGetSet()
        {
            // Setup
            var builder = new MariaDbConnectionStringBuilder();

            // Act
            builder.Server = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Server);
        }

        [TestMethod]
        public void TestMariaDbConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new MariaDbConnectionStringBuilder();

            // Act
            builder.Port = 3306;

            // Assert
            Assert.AreEqual((uint)3306, builder.Port);
        }

        [TestMethod]
        public void TestMariaDbConnectionStringBuilderDatabaseForGetSet()
        {
            // Setup
            var builder = new MariaDbConnectionStringBuilder();

            // Act
            builder.Database = "TestDb";

            // Assert
            Assert.AreEqual("TestDb", builder.Database);
        }

        [TestMethod]
        public void TestMariaDbConnectionStringBuilderUserIdForGetSet()
        {
            // Setup
            var builder = new MariaDbConnectionStringBuilder();

            // Act
            builder.UserId = "root";

            // Assert
            Assert.AreEqual("root", builder.UserId);
        }

        [TestMethod]
        public void TestMariaDbConnectionStringBuilderPasswordForGetSet()
        {
            // Setup
            var builder = new MariaDbConnectionStringBuilder();

            // Act
            builder.Password = "password";

            // Assert
            Assert.AreEqual("password", builder.Password);
        }

        [TestMethod]
        public void TestMariaDbConnectionStringBuilderServerForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

            // Act
            var builder = new MariaDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Server);
        }
    }
}
