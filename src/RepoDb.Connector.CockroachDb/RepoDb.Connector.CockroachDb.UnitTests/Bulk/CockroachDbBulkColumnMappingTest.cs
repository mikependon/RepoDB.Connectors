#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockroachDb.Bulk;

namespace RepoDb.Connector.CockroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockroachDbBulkColumnMappingTest
    {
        [TestMethod]
        public void TestCockroachDbBulkColumnMappingSourceOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingDestinationOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingDestinationColumnForOrdinalAndNameConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingDestinationOrdinalForOrdinalAndNameConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingSourceColumnForNameAndOrdinalConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingSourceOrdinalForNameAndOrdinalConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingSourceColumnForNameConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockroachDbBulkColumnMappingDestinationColumnForNameConstructor()
        {
            // Setup
            var mapping = new CockroachDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }
    }
}
