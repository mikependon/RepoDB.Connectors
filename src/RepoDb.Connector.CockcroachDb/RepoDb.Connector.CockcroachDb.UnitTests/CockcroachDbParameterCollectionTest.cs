#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbParameterCollectionTest
    {
        private static CockcroachDbParameterCollection CreateCollection()
        {
            var command = new CockcroachDbCommand();
            return command.Parameters;
        }

        [TestMethod]
        public void TestCockcroachDbParameterCollectionCountForAddWithValue()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.AddWithValue("@Id", 100);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestCockcroachDbParameterCollectionContainsForParameterName()
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
        public void TestCockcroachDbParameterCollectionIndexOfForParameterName()
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
        public void TestCockcroachDbParameterCollectionAddForCockcroachDbParameter()
        {
            // Setup
            var collection = CreateCollection();
            var parameter = new CockcroachDbParameter { ParameterName = "@Id", Value = 100 };

            // Act
            collection.Add(parameter);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestCockcroachDbParameterCollectionRemoveAtForIndex()
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
        public void TestCockcroachDbParameterCollectionRemoveAtForParameterName()
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
        public void TestCockcroachDbParameterCollectionClearForRemovesAllItems()
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
        public void TestCockcroachDbParameterCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var parameter = new CockcroachDbParameter { ParameterName = "@Name", Value = "John" };

            // Act
            collection.Insert(0, parameter);

            // Assert
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestCockcroachDbParameterCollectionCopyToForCopiesCockcroachDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var array = new CockcroachDbParameter[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual("@Id", array[0].ParameterName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbParameterCollectionGetEnumeratorForYieldsCockcroachDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.IsInstanceOfType<CockcroachDbParameter>(enumerator.Current);
        }
    }
}
