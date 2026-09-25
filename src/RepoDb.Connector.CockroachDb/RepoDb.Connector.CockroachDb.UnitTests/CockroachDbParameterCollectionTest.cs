#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbParameterCollectionTest
    {
        private static CockroachDbParameterCollection CreateCollection()
        {
            var command = new CockroachDbCommand();
            return command.Parameters;
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionCountForAddWithValue()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.AddWithValue("@Id", 100);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionContainsForParameterName()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var output = collection.Contains("@Id");

            // Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionIndexOfForParameterName()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var output = collection.IndexOf("@Id");

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionAddForCockroachDbParameter()
        {
            // Setup
            var collection = CreateCollection();
            var parameter = new CockroachDbParameter { ParameterName = "@Id", Value = 100 };

            // Act
            collection.Add(parameter);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionRemoveAtForIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionRemoveAtForParameterName()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            collection.RemoveAt("@Id");

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionClearForRemovesAllItems()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            collection.AddWithValue("@Name", "John");

            // Act
            collection.Clear();

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var parameter = new CockroachDbParameter { ParameterName = "@Name", Value = "John" };

            // Act
            collection.Insert(0, parameter);

            // Assert
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionCopyToForCopiesCockroachDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var array = new CockroachDbParameter[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual("@Id", array[0].ParameterName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbParameterCollectionGetEnumeratorForYieldsCockroachDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.IsInstanceOfType<CockroachDbParameter>(enumerator.Current);
        }
    }
}
