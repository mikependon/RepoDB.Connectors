#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using RepoDb.Connector.CockcroachDb.Bulk;

namespace RepoDb.Connector.CockcroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockcroachDbBulkCopyTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingsForNewInstanceIsEmpty()
        {
            // Setup
            var bulkCopy = new CockcroachDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.ColumnMappings.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyBulkCopyTimeoutForGetSet()
        {
            // Setup
            var bulkCopy = new CockcroachDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.BulkCopyTimeout = 60;

            // Assert
            Assert.AreEqual(60, bulkCopy.BulkCopyTimeout);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyDestinationTableNameForGetSet()
        {
            // Setup
            var bulkCopy = new CockcroachDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.DestinationTableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", bulkCopy.DestinationTableName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyRowsCopiedForDefaultValue()
        {
            // Setup
            var bulkCopy = new CockcroachDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.RowsCopied;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyDisposeForClosesConnection()
        {
            // Setup
            var connection = new CockcroachDbConnection(ConnectionString);
            var bulkCopy = new CockcroachDbBulkCopy(connection);

            // Act
            bulkCopy.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingsForConstructorWithConnection()
        {
            // Setup
            var connection = new CockcroachDbConnection(ConnectionString);

            // Act
            var bulkCopy = new CockcroachDbBulkCopy(connection);

            // Assert
            Assert.IsNotNull(bulkCopy.ColumnMappings);
        }
    }
}
