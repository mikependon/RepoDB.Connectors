#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.EnterpriseDb.UnitTests
{
    [TestClass]
    public sealed class EDBParameterCollectionTest
    {
        private static EDBParameterCollection CreateCollection()
        {
            var command = new EDBCommand();
            return command.Parameters;
        }

        [TestMethod]
        public void TestEDBParameterCollectionCountForAddWithValue()
        {
            // Setup
            var collection = CreateCollection();

            // Act
            collection.AddWithValue("@Id", 100);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestEDBParameterCollectionContainsForParameterName()
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
        public void TestEDBParameterCollectionIndexOfForParameterName()
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
        public void TestEDBParameterCollectionAddForEDBParameter()
        {
            // Setup
            var collection = CreateCollection();
            var parameter = new EDBParameter { ParameterName = "@Id", Value = 100 };

            // Act
            collection.Add(parameter);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void TestEDBParameterCollectionRemoveAtForIndex()
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
        public void TestEDBParameterCollectionRemoveAtForParameterName()
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
        public void TestEDBParameterCollectionClearForRemovesAllItems()
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
        public void TestEDBParameterCollectionInsertForInsertsAtIndex()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var parameter = new EDBParameter { ParameterName = "@Name", Value = "John" };

            // Act
            collection.Insert(0, parameter);

            // Assert
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestEDBParameterCollectionCopyToForCopiesEDBParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);
            var array = new EDBParameter[1];

            // Act
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual("@Id", array[0].ParameterName);
        }

        [TestMethod]
        public void TestEDBParameterCollectionGetEnumeratorForYieldsEDBParameterInstances()
        {
            // Setup
            var collection = CreateCollection();
            collection.AddWithValue("@Id", 100);

            // Act
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.IsInstanceOfType<EDBParameter>(enumerator.Current);
        }
    }
}
