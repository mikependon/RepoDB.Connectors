#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDb.Bulk;

namespace RepoDb.Connector.MariaDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class MariaDbBulkCopyColumnMappingCollectionTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        private static MariaDbBulkCopyColumnMappingCollection CreateCollection()
        {
            var bulkCopy = new MariaDbBulkCopy(ConnectionString);
            return bulkCopy.ColumnMappings;
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionCountForAddMapping()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionIndexerForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection[0];

            // Assert
            Assert.AreSame(mapping, output);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionAddForOrdinalOverload()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(0, 1);

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionContainsForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection.Contains(mapping);

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionIndexOfForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));
            var mapping = collection.Add(new MariaDbBulkColumnMapping("Name", "Name"));

            // Act
            var output = collection.IndexOf(mapping);

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionRemoveForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.Remove(mapping);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionRemoveAtForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionClearForRemovesAllMappings()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));
            collection.Add(new MariaDbBulkColumnMapping("Name", "Name"));

            // Act
            collection.Clear();

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));
            var mapping = new MariaDbBulkColumnMapping("Name", "Name");

            // Act
            collection.Insert(0, mapping);

            // Assert
            Assert.AreSame(mapping, collection[0]);
        }

        [TestMethod]
        public void TestMariaDbBulkCopyColumnMappingCollectionCopyToForCopiesMappings()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new MariaDbBulkColumnMapping("Id", "Id"));
            var array = new MariaDbBulkColumnMapping[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreSame(mapping, array[0]);
        }
    }
}
