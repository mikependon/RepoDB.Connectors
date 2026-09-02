#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.EnterpriseDb.Bulk;

namespace RepoDb.Connector.EnterpriseDb.UnitTests.Bulk
{
    [TestClass]
    public sealed class EDBBulkColumnMappingTest
    {
        [TestMethod]
        public void TestEDBBulkColumnMappingSourceOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping(0, 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(0, output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingDestinationOrdinalForOrdinalConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping(0, 1);

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(1, output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingDestinationColumnForOrdinalAndNameConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingDestinationOrdinalForOrdinalAndNameConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping(0, "Dest");

            // Act
            var output = mapping.DestinationOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingSourceColumnForNameAndOrdinalConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingSourceOrdinalForNameAndOrdinalConstructorDefaultsToNegativeOne()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping("Src", 1);

            // Act
            var output = mapping.SourceOrdinal;

            // Assert
            Assert.AreEqual(-1, output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingSourceColumnForNameConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.SourceColumn;

            // Assert
            Assert.AreEqual("Src", output);
        }

        [TestMethod]
        public void TestEDBBulkColumnMappingDestinationColumnForNameConstructor()
        {
            // Setup
            var mapping = new EDBBulkColumnMapping("Src", "Dest");

            // Act
            var output = mapping.DestinationColumn;

            // Assert
            Assert.AreEqual("Dest", output);
        }
    }
}
