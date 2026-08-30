#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDb.UnitTests
{
    [TestClass]
    public sealed class MariaDbParameterCollectionTest
    {
        private static MariaDbParameterCollection CreateCollection()
        {
            var command = new MariaDbCommand();
            return command.Parameters;
        }

        [TestMethod]
        public void TestMariaDbParameterCollectionCountForAddWithValue()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.AddWithValue("@Id", 100);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestMariaDbParameterCollectionContainsForParameterName()
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
        public void TestMariaDbParameterCollectionIndexOfForParameterName()
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
        public void TestMariaDbParameterCollectionAddForMariaDbParameter()
        {
            // Setup
            var collection = CreateCollection();
            var parameter = new MariaDbParameter { ParameterName = "@Id", Value = 100 };

            // Act
            collection.Add(parameter);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestMariaDbParameterCollectionRemoveAtForIndex()
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
        public void TestMariaDbParameterCollectionRemoveAtForParameterName()
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
        public void TestMariaDbParameterCollectionClearForRemovesAllItems()
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
        public void TestMariaDbParameterCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var parameter = new MariaDbParameter { ParameterName = "@Name", Value = "John" };

            // Act
            collection.Insert(0, parameter);

            // Assert
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestMariaDbParameterCollectionCopyToForCopiesMariaDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var array = new MariaDbParameter[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual("@Id", array[0].ParameterName);
        }

        [TestMethod]
        public void TestMariaDbParameterCollectionGetEnumeratorForYieldsMariaDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.IsInstanceOfType<MariaDbParameter>(enumerator.Current);
        }
    }
}
