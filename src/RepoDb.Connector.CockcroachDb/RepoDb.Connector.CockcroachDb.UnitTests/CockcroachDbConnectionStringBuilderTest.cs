#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderHostForGetSet()
        {
            // Setup
            var builder = new CockcroachDbConnectionStringBuilder();

            // Act
            builder.Host = "localhost";

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderPortForGetSet()
        {
            // Setup
            var builder = new CockcroachDbConnectionStringBuilder();

            // Act
            builder.Port = 5432;

            // Assert
            Assert.AreEqual(5432, builder.Port);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderDatabaseForGetSet()
        {
            // Setup
            var builder = new CockcroachDbConnectionStringBuilder();

            // Act
            builder.Database = "TestDb";

            // Assert
            Assert.AreEqual("TestDb", builder.Database, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderUsernameForGetSet()
        {
            // Setup
            var builder = new CockcroachDbConnectionStringBuilder();

            // Act
            builder.Username = "postgres";

            // Assert
            Assert.AreEqual("postgres", builder.Username, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderPasswordForGetSet()
        {
            // Setup
            var builder = new CockcroachDbConnectionStringBuilder();

            // Act
            builder.Password = "password";

            // Assert
            Assert.AreEqual("password", builder.Password, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbConnectionStringBuilderHostForConstructorWithConnectionString()
        {
            // Setup
            var connectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

            // Act
            var builder = new CockcroachDbConnectionStringBuilder(connectionString);

            // Assert
            Assert.AreEqual("localhost", builder.Host, StringComparer.Ordinal);
        }
    }
}
