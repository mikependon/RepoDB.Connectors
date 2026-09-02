#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestEDBConnectionStringBuilderHostForGetSet()
        {
            // Setup
            var builder = new EDBConnectionStringBuilder();

            // Act
            builder.Host = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Host);
        }

        [TestMethod]
        public void TestEDBConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new EDBConnectionStringBuilder();

            // Act
            builder.Port = 5432;

            // Assert
            Assert.AreEqual(5432, builder.Port);
        }

        [TestMethod]
        public void TestEDBConnectionStringBuilderDatabaseForGetSet()
        {
            // Setup
            var builder = new EDBConnectionStringBuilder();

            // Act
            builder.Database = "TestDb";

            // Assert
            Assert.AreEqual("TestDb", builder.Database);
        }

        [TestMethod]
        public void TestEDBConnectionStringBuilderUsernameForGetSet()
        {
            // Setup
            var builder = new EDBConnectionStringBuilder();

            // Act
            builder.Username = "postgres";

            // Assert
            Assert.AreEqual("postgres", builder.Username);
        }

        [TestMethod]
        public void TestEDBConnectionStringBuilderPasswordForGetSet()
        {
            // Setup
            var builder = new EDBConnectionStringBuilder();

            // Act
            builder.Password = "password";

            // Assert
            Assert.AreEqual("password", builder.Password);
        }

        [TestMethod]
        public void TestEDBConnectionStringBuilderHostForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

            // Act
            var builder = new EDBConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Host);
        }
    }
}
