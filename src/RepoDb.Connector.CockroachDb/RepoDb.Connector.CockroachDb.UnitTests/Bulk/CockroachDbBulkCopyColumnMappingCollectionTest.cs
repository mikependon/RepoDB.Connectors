#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockroachDb.Bulk;

namespace RepoDb.Connector.CockroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockroachDbBulkCopyColumnMappingCollectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        private static CockroachDbBulkCopyColumnMappingCollection CreateCollection()
        {
            var bulkCopy = new CockroachDbBulkCopy(ConnectionString);
            return bulkCopy.ColumnMappings;
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionCountForAddMapping()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionIndexerForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection[0];

            // Assert
            Assert.AreSame(mapping, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionAddForOrdinalOverload()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(0, 1);

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionContainsForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection.Contains(mapping);

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionIndexOfForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));
            var mapping = collection.Add(new CockroachDbBulkColumnMapping("Name", "Name"));

            // Act
            var output = collection.IndexOf(mapping);

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionRemoveForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.Remove(mapping);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionRemoveAtForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionClearForRemovesAllMappings()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));
            collection.Add(new CockroachDbBulkColumnMapping("Name", "Name"));

            // Act
            collection.Clear();

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));
            var mapping = new CockroachDbBulkColumnMapping("Name", "Name");

            // Act
            collection.Insert(0, mapping);

            // Assert
            Assert.AreSame(mapping, collection[0]);
        }

        [TestMethod]
        public void TestCockroachDbBulkCopyColumnMappingCollectionCopyToForCopiesMappings()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockroachDbBulkColumnMapping("Id", "Id"));
            var array = new CockroachDbBulkColumnMapping[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreSame(mapping, array[0]);
        }
    }
}
