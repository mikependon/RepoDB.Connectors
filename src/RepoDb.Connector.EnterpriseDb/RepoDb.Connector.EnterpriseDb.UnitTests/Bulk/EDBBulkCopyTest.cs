#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using RepoDb.Connector.EnterpriseDb.Bulk;

namespace RepoDb.Connector.EnterpriseDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class EDBBulkCopyTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingsForNewInstanceIsEmpty()
        {
            // Setup
            var bulkCopy = new EDBBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.ColumnMappings.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestEDBBulkCopyBulkCopyTimeoutForGetSet()
        {
            // Setup
            var bulkCopy = new EDBBulkCopy(ConnectionString);

            // Act
            bulkCopy.BulkCopyTimeout = 60;

            // Assert
            Assert.AreEqual(60, bulkCopy.BulkCopyTimeout);
        }

        [TestMethod]
        public void TestEDBBulkCopyDestinationTableNameForGetSet()
        {
            // Setup
            var bulkCopy = new EDBBulkCopy(ConnectionString);

            // Act
            bulkCopy.DestinationTableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", bulkCopy.DestinationTableName);
        }

        [TestMethod]
        public void TestEDBBulkCopyRowsCopiedForDefaultValue()
        {
            // Setup
            var bulkCopy = new EDBBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.RowsCopied;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestEDBBulkCopyDisposeForClosesConnection()
        {
            // Setup
            var connection = new EDBConnection(ConnectionString);
            var bulkCopy = new EDBBulkCopy(connection);

            // Act
            bulkCopy.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingsForConstructorWithConnection()
        {
            // Setup
            var connection = new EDBConnection(ConnectionString);

            // Act
            var bulkCopy = new EDBBulkCopy(connection);

            // Assert
            Assert.IsNotNull(bulkCopy.ColumnMappings);
        }
    }
}
