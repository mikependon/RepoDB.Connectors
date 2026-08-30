#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using RepoDb.Connector.MariaDbConnector.Bulk;

namespace RepoDb.Connector.MariaDbConnector.UnitTests.Bulk
{
    [TestClass]
    public sealed class MariaDbBulkCopyTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingsForNewInstanceIsEmpty()
        {
            // Setup
            var bulkCopy = new MariaDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.ColumnMappings.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyBulkCopyTimeoutForGetSet()
        {
            // Setup
            var bulkCopy = new MariaDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.BulkCopyTimeout = 60;

            // Assert
            Assert.AreEqual(60, bulkCopy.BulkCopyTimeout);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyDestinationTableNameForGetSet()
        {
            // Setup
            var bulkCopy = new MariaDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.DestinationTableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", bulkCopy.DestinationTableName);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyRowsCopiedForDefaultValue()
        {
            // Setup
            var bulkCopy = new MariaDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.RowsCopied;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyDisposeForClosesConnection()
        {
            // Setup
            var connection = new MariaDbConnection(ConnectionString);
            var bulkCopy = new MariaDbBulkCopy(connection);

            // Act
            bulkCopy.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingsForConstructorWithConnection()
        {
            // Setup
            var connection = new MariaDbConnection(ConnectionString);

            // Act
            var bulkCopy = new MariaDbBulkCopy(connection);

            // Assert
            Assert.IsNotNull(bulkCopy.ColumnMappings);
        }
    }
}
