#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDb.Bulk;

namespace RepoDb.Connector.MariaDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class MariaDbBulkLoaderTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestMariaDbBulkLoaderConnectionForConstructorSetsConnection()
        {
            // Setup
            var connection = new MariaDbConnection(ConnectionString);
            var loader = new MariaDbBulkLoader(connection);

            // Act
            var output = loader.Connection;

            // Assert
            Assert.AreSame(connection, output);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderLocalForDefaultValue()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            var output = loader.Local;

            // Assert
            Assert.IsFalse(output);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderFieldTerminatorForGetSet()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            loader.FieldTerminator = ",";

            // Assert
            Assert.AreEqual(",", loader.FieldTerminator);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderTableNameForGetSet()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            loader.TableName = "Customer";

            // Assert
            Assert.AreEqual("Customer", loader.TableName);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderConflictOptionForGetSet()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            loader.ConflictOption = MariaDbBulkLoaderConflictOption.Replace;

            // Assert
            Assert.AreEqual(MariaDbBulkLoaderConflictOption.Replace, loader.ConflictOption);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderPriorityForGetSet()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            loader.Priority = MariaDbBulkLoaderPriority.Concurrent;

            // Assert
            Assert.AreEqual(MariaDbBulkLoaderPriority.Concurrent, loader.Priority);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderColumnsForNewInstanceIsEmpty()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            var output = loader.Columns.Count;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestMariaDbBulkLoaderColumnsForAddAppendsColumn()
        {
            // Setup
            var loader = new MariaDbBulkLoader(new MariaDbConnection(ConnectionString));

            // Act
            loader.Columns.Add("Id");

            // Assert
            Assert.HasCount(1, loader.Columns);
        }
    }
}
