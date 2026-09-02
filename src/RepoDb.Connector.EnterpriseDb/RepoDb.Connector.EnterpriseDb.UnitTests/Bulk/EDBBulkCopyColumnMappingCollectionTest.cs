#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb.Bulk;

namespace RepoDb.Connector.EnterpriseDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class EDBBulkCopyColumnMappingCollectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        private static EDBBulkCopyColumnMappingCollection CreateCollection()
        {
            var bulkCopy = new EDBBulkCopy(ConnectionString);
            return bulkCopy.ColumnMappings;
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionCountForAddMapping()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(new EDBBulkColumnMapping("Id", "Id"));

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionIndexerForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new EDBBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection[0];

            // Assert
            Assert.AreSame(mapping, output);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionAddForOrdinalOverload()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(0, 1);

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionContainsForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new EDBBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection.Contains(mapping);

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionIndexOfForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new EDBBulkColumnMapping("Id", "Id"));
            var mapping = collection.Add(new EDBBulkColumnMapping("Name", "Name"));

            // Act
            var output = collection.IndexOf(mapping);

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionRemoveForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new EDBBulkColumnMapping("Id", "Id"));

            // Act
            collection.Remove(mapping);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionRemoveAtForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new EDBBulkColumnMapping("Id", "Id"));

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionClearForRemovesAllMappings()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new EDBBulkColumnMapping("Id", "Id"));
            collection.Add(new EDBBulkColumnMapping("Name", "Name"));

            // Act
            collection.Clear();

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new EDBBulkColumnMapping("Id", "Id"));
            var mapping = new EDBBulkColumnMapping("Name", "Name");

            // Act
            collection.Insert(0, mapping);

            // Assert
            Assert.AreSame(mapping, collection[0]);
        }

        [TestMethod]
        public void TestEDBBulkCopyColumnMappingCollectionCopyToForCopiesMappings()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new EDBBulkColumnMapping("Id", "Id"));
            var array = new EDBBulkColumnMapping[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreSame(mapping, array[0]);
        }
    }
}
