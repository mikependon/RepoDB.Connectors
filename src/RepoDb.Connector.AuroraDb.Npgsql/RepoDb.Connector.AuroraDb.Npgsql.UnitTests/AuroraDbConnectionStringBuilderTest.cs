#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests
{
    [TestClass]
    public sealed class AuroraDbConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderHostForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Host = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Port = 5432;

            // Assert
            Assert.AreEqual(5432, builder.Port);
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
        public void TestAuroraDbConnectionStringBuilderUsernameForGetSet()
        {
            // Setup
            var builder = new AuroraDbConnectionStringBuilder();

            // Act
            builder.Username = "postgres";

            // Assert
            Assert.AreEqual("postgres", builder.Username, StringComparer.Ordinal);
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
        public void TestAuroraDbConnectionStringBuilderHostForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

            // Act
            var builder = new AuroraDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }
    }
}
