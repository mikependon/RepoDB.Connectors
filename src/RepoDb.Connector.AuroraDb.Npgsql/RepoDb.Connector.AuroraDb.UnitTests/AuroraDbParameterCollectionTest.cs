#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.UnitTests
{
    [TestClass]
    public sealed class AuroraDbParameterCollectionTest
    {
        private static AuroraDbParameterCollection CreateCollection()
        {
            var command = new AuroraDbCommand();
            return command.Parameters;
        }

        [TestMethod]
        public void TestAuroraDbParameterCollectionCountForAddWithValue()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.AddWithValue("@Id", 100);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestAuroraDbParameterCollectionContainsForParameterName()
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
        public void TestAuroraDbParameterCollectionIndexOfForParameterName()
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
        public void TestAuroraDbParameterCollectionAddForAuroraDbParameter()
        {
            // Setup
            var collection = CreateCollection();
            var parameter = new AuroraDbParameter { ParameterName = "@Id", Value = 100 };

            // Act
            collection.Add(parameter);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestAuroraDbParameterCollectionRemoveAtForIndex()
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
        public void TestAuroraDbParameterCollectionRemoveAtForParameterName()
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
        public void TestAuroraDbParameterCollectionClearForRemovesAllItems()
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
        public void TestAuroraDbParameterCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var parameter = new AuroraDbParameter { ParameterName = "@Name", Value = "John" };

            // Act
            collection.Insert(0, parameter);

            // Assert
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestAuroraDbParameterCollectionCopyToForCopiesAuroraDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var array = new AuroraDbParameter[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual("@Id", array[0].ParameterName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbParameterCollectionGetEnumeratorForYieldsAuroraDbParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.IsInstanceOfType<AuroraDbParameter>(enumerator.Current);
        }
    }
}
