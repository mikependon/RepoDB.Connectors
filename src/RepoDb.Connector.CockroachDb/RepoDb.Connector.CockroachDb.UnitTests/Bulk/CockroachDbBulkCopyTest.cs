#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using RepoDb.Connector.CockroachDb.Bulk;

namespace RepoDb.Connector.CockroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockroachDbBulkCopyTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingsForNewInstanceIsEmpty()
        {
            // Setup
            var bulkCopy = new CockroachDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.ColumnMappings.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyBulkCopyTimeoutForGetSet()
        {
            // Setup
            var bulkCopy = new CockroachDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.BulkCopyTimeout = 60;

            // Assert
            Assert.AreEqual(60, bulkCopy.BulkCopyTimeout);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyDestinationTableNameForGetSet()
        {
            // Setup
            var bulkCopy = new CockroachDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.DestinationTableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", bulkCopy.DestinationTableName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyRowsCopiedForDefaultValue()
        {
            // Setup
            var bulkCopy = new CockroachDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.RowsCopied;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyDisposeForClosesConnection()
        {
            // Setup
            var connection = new CockroachDbConnection(ConnectionString);
            var bulkCopy = new CockroachDbBulkCopy(connection);

            // Act
            bulkCopy.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingsForConstructorWithConnection()
        {
            // Setup
            var connection = new CockroachDbConnection(ConnectionString);

            // Act
            var bulkCopy = new CockroachDbBulkCopy(connection);

            // Assert
            Assert.IsNotNull(bulkCopy.ColumnMappings);
        }
    }
}
