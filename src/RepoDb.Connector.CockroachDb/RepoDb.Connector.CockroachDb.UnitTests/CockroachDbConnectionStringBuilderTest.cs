#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderHostForGetSet()
        {
            // Setup
            var builder = new CockroachDbConnectionStringBuilder();

            // Act
            builder.Host = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new CockroachDbConnectionStringBuilder();

            // Act
            builder.Port = 5432;

            // Assert
            Assert.AreEqual(5432, builder.Port);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderDatabaseForGetSet()
        {
            // Setup
            var builder = new CockroachDbConnectionStringBuilder();

            // Act
            builder.Database = "TestDb";

            // Assert
            Assert.AreEqual("TestDb", builder.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderUsernameForGetSet()
        {
            // Setup
            var builder = new CockroachDbConnectionStringBuilder();

            // Act
            builder.Username = "postgres";

            // Assert
            Assert.AreEqual("postgres", builder.Username, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderPasswordForGetSet()
        {
            // Setup
            var builder = new CockroachDbConnectionStringBuilder();

            // Act
            builder.Password = "password";

            // Assert
            Assert.AreEqual("password", builder.Password, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbConnectionStringBuilderHostForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

            // Act
            var builder = new CockroachDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }
    }
}
