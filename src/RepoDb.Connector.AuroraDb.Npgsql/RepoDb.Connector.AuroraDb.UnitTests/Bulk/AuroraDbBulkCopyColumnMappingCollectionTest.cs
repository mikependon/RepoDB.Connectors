#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.AuroraDb.Bulk;

namespace RepoDb.Connector.AuroraDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class AuroraDbBulkCopyColumnMappingCollectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        private static AuroraDbBulkCopyColumnMappingCollection CreateCollection()
        {
            var bulkCopy = new AuroraDbBulkCopy(ConnectionString);
            return bulkCopy.ColumnMappings;
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionCountForAddMapping()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionIndexerForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection[0];

            // Assert
            Assert.AreSame(mapping, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionAddForOrdinalOverload()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(0, 1);

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionContainsForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection.Contains(mapping);

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionIndexOfForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));
            var mapping = collection.Add(new AuroraDbBulkColumnMapping("Name", "Name"));

            // Act
            var output = collection.IndexOf(mapping);

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionRemoveForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.Remove(mapping);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionRemoveAtForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionClearForRemovesAllMappings()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));
            collection.Add(new AuroraDbBulkColumnMapping("Name", "Name"));

            // Act
            collection.Clear();

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));
            var mapping = new AuroraDbBulkColumnMapping("Name", "Name");

            // Act
            collection.Insert(0, mapping);

            // Assert
            Assert.AreSame(mapping, collection[0]);
        }

        [TestMethod]
        public void TestAuroraDbBulkCopyColumnMappingCollectionCopyToForCopiesMappings()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new AuroraDbBulkColumnMapping("Id", "Id"));
            var array = new AuroraDbBulkColumnMapping[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreSame(mapping, array[0]);
        }
    }
}
