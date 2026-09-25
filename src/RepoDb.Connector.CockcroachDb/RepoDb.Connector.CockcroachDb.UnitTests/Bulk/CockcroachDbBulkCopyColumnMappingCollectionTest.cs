#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockcroachDb.Bulk;

namespace RepoDb.Connector.CockcroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockcroachDbBulkCopyColumnMappingCollectionTest
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=password;";

        private static CockcroachDbBulkCopyColumnMappingCollection CreateCollection()
        {
            var bulkCopy = new CockcroachDbBulkCopy(ConnectionString);
            return bulkCopy.ColumnMappings;
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionCountForAddMapping()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionIndexerForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection[0];

            // Assert
            Assert.AreSame(mapping, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionAddForOrdinalOverload()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.Add(0, 1);

            // Assert
            Assert.HasCount(1, collection);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionContainsForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));

            // Act
            var output = collection.Contains(mapping);

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionIndexOfForAddedMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));
            var mapping = collection.Add(new CockcroachDbBulkColumnMapping("Name", "Name"));

            // Act
            var output = collection.IndexOf(mapping);

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionRemoveForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.Remove(mapping);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionRemoveAtForRemovesMapping()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionClearForRemovesAllMappings()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));
            collection.Add(new CockcroachDbBulkColumnMapping("Name", "Name"));

            // Act
            collection.Clear();

            // Assert
            Assert.IsEmpty(collection);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));
            var mapping = new CockcroachDbBulkColumnMapping("Name", "Name");

            // Act
            collection.Insert(0, mapping);

            // Assert
            Assert.AreSame(mapping, collection[0]);
        }

        [TestMethod]
        public void TestCockcroachDbBulkCopyColumnMappingCollectionCopyToForCopiesMappings()
        {
            // Setup
            var collection = CreateCollection();
            var mapping = collection.Add(new CockcroachDbBulkColumnMapping("Id", "Id"));
            var array = new CockcroachDbBulkColumnMapping[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreSame(mapping, array[0]);
        }
    }
}
