#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.CockcroachDb.Bulk;

namespace RepoDb.Connector.CockcroachDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class CockcroachDbBulkColumnMappingTest
    {
        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingSourceOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingDestinationOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingDestinationColumnForOrdinalAndNameConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingDestinationOrdinalForOrdinalAndNameConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingSourceColumnForNameAndOrdinalConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingSourceOrdinalForNameAndOrdinalConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingSourceColumnForNameConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbBulkColumnMappingDestinationColumnForNameConstructor()
        {
            // Setup
            var mapping = new CockcroachDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }
    }
}
