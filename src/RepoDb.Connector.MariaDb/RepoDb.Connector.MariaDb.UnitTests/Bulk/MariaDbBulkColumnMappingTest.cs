#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDb.Bulk;

namespace RepoDb.Connector.MariaDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class MariaDbBulkColumnMappingTest
    {
        [TestMethod]
        public void TestMariaDbBulkColumnMappingSourceOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingDestinationOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingDestinationColumnForOrdinalAndNameConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingDestinationOrdinalForOrdinalAndNameConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingSourceColumnForNameAndOrdinalConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingSourceOrdinalForNameAndOrdinalConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingSourceColumnForNameConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output);
        }

        [TestMethod]
        public void TestMariaDbBulkColumnMappingDestinationColumnForNameConstructor()
        {
            // Setup
            var mapping = new MariaDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output);
        }
    }
}
