#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.AuroraDb.Npgsql.Bulk;

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests.Bulk
{
    [TestClass]
    public sealed class AuroraDbBulkColumnMappingTest
    {
        [TestMethod]
        public void TestAuroraDbBulkColumnMappingSourceOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingDestinationOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping(0, 1);

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingDestinationColumnForOrdinalAndNameConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingDestinationOrdinalForOrdinalAndNameConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingSourceColumnForNameAndOrdinalConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingSourceOrdinalForNameAndOrdinalConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingSourceColumnForNameConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbBulkColumnMappingDestinationColumnForNameConstructor()
        {
            // Setup
            var mapping = new AuroraDbBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output, StringComparer.Ordinal);
        }
    }
}
