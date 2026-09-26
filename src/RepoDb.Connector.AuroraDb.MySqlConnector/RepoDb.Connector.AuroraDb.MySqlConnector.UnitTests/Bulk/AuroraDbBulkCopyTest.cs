#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data;
using RepoDb.Connector.AuroraDb.MySqlConnector.Bulk;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.UnitTests.Bulk
{
    [TestClass]
    public sealed class AuroraDbBulkCopyTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingsForNewInstanceIsEmpty()
        {
            // Setup
            var bulkCopy = new AuroraDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.ColumnMappings.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyBulkCopyTimeoutForGetSet()
        {
            // Setup
            var bulkCopy = new AuroraDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.BulkCopyTimeout = 60;

            // Assert
            Assert.AreEqual(60, bulkCopy.BulkCopyTimeout);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyDestinationTableNameForGetSet()
        {
            // Setup
            var bulkCopy = new AuroraDbBulkCopy(ConnectionString);

            // Act
            bulkCopy.DestinationTableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", bulkCopy.DestinationTableName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyRowsCopiedForDefaultValue()
        {
            // Setup
            var bulkCopy = new AuroraDbBulkCopy(ConnectionString);

            // Act
            var output = bulkCopy.RowsCopied;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyDisposeForClosesConnection()
        {
            // Setup
            var connection = new AuroraDbConnection(ConnectionString);
            var bulkCopy = new AuroraDbBulkCopy(connection);

            // Act
            bulkCopy.Dispose();

            // Assert
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingsForConstructorWithConnection()
        {
            // Setup
            var connection = new AuroraDbConnection(ConnectionString);

            // Act
            var bulkCopy = new AuroraDbBulkCopy(connection);

            // Assert
            Assert.IsNotNull(bulkCopy.ColumnMappings);
        }
    }
}
