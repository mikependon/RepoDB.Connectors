#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.AuroraDb.MySqlConnector.IntegrationTests.Setup;

namespace RepoDb.Connector.AuroraDb.MySqlConnector.IntegrationTests.Operations
{
    /// <summary>
    /// Integration tests that round-trip the <see cref="AuroraDbType"/> values through a real Aurora MySQL-compatible server - the value is
    /// stored as an <see cref="AuroraDbParameter"/> explicitly typed via <see cref="AuroraDbParameter.AuroraDbType"/>,
    /// read back, and verified both by value and by the server-reported data type name.
    /// </summary>
    [TestClass]
    public class AuroraDbNativeTypeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
        }

        // NOTE: the MySQL protocol reports coarse data type names - the TEXT family is reported as VARCHAR,
        // the BINARY/BLOB families as BLOB and the spatial types as GEOMETRY.

        #region Helpers

        private static (object Value, string DataTypeName) RoundTrip(
            string columnDefinition,
            AuroraDbType auroraDbType,
            object value)
        {
            using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                connection.Open();
                Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS `NativeTypeModel`;");
                Helper.ExecuteNonQuery(connection, $"CREATE TABLE `NativeTypeModel` (`Id` INT NOT NULL PRIMARY KEY, `Value` {columnDefinition} NULL);");
                try
                {
                    using (var insert = connection.CreateCommand())
                    {
                        insert.CommandText = "INSERT INTO `NativeTypeModel` (`Id`, `Value`) VALUES (1, @value);";
                        var parameter = (AuroraDbParameter)insert.CreateParameter();
                        parameter.ParameterName = "@value";
                        parameter.AuroraDbType = auroraDbType;
                        parameter.Value = value;
                        insert.Parameters.Add(parameter);
                        insert.ExecuteNonQuery();
                    }

                    using (var select = connection.CreateCommand())
                    {
                        select.CommandText = "SELECT `Value` FROM `NativeTypeModel` WHERE `Id` = 1;";
                        using (var reader = select.ExecuteReader())
                        {
                            reader.Read();
                            return (reader.GetValue(0), reader.GetDataTypeName(0));
                        }
                    }
                }
                finally
                {
                    Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS `NativeTypeModel`;");
                }
            }
        }

        private static string GetColumnDataTypeName(
            string columnDefinition)
        {
            using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                connection.Open();
                Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS `NativeTypeModel`;");
                Helper.ExecuteNonQuery(connection, $"CREATE TABLE `NativeTypeModel` (`Id` INT NOT NULL PRIMARY KEY, `Value` {columnDefinition} NULL);");
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT `Value` FROM `NativeTypeModel`;";
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.GetDataTypeName(0);
                        }
                    }
                }
                finally
                {
                    Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS `NativeTypeModel`;");
                }
            }
        }

        #endregion

        #region Numeric

        [TestMethod]
        public void TestAuroraDbNativeTypeForTinyInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TINYINT", AuroraDbType.TinyInt, (sbyte)100);

            // Assert
            Assert.AreEqual((sbyte)100, value);
            Assert.AreEqual("TINYINT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForSmallInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("SMALLINT", AuroraDbType.SmallInt, (short)32000);

            // Assert
            Assert.AreEqual((short)32000, value);
            Assert.AreEqual("SMALLINT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForMediumInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("MEDIUMINT", AuroraDbType.MediumInt, 8000000);

            // Assert
            Assert.AreEqual(8000000, value);
            Assert.AreEqual("MEDIUMINT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("INT", AuroraDbType.Int, 2000000000);

            // Assert
            Assert.AreEqual(2000000000, value);
            Assert.AreEqual("INT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForBigInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("BIGINT", AuroraDbType.BigInt, 9000000000000000000L);

            // Assert
            Assert.AreEqual(9000000000000000000L, value);
            Assert.AreEqual("BIGINT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForDecimal()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("DECIMAL(18,2)", AuroraDbType.Decimal, 12345.67M);

            // Assert
            Assert.AreEqual(12345.67M, value);
            Assert.AreEqual("DECIMAL", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForFloat()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("FLOAT", AuroraDbType.Float, 1.5F);

            // Assert
            Assert.AreEqual(1.5F, value);
            Assert.AreEqual("FLOAT", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForDouble()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("DOUBLE", AuroraDbType.Double, 3.14159D);

            // Assert
            Assert.AreEqual(3.14159D, value);
            Assert.AreEqual("DOUBLE", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region String

        [TestMethod]
        public void TestAuroraDbNativeTypeForChar()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("CHAR(1)", AuroraDbType.Char, "A");

            // Assert
            Assert.AreEqual("A", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("CHAR(1)", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForVarChar()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("VARCHAR(100)", AuroraDbType.VarChar, "VarChar 日本語");

            // Assert
            Assert.AreEqual("VarChar 日本語", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("VARCHAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTinyText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TINYTEXT", AuroraDbType.TinyText, "TinyText");

            // Assert
            Assert.AreEqual("TinyText", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("VARCHAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TEXT", AuroraDbType.Text, "Text Ünïcödé");

            // Assert
            Assert.AreEqual("Text Ünïcödé", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("VARCHAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForMediumText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("MEDIUMTEXT", AuroraDbType.MediumText, "MediumText");

            // Assert
            Assert.AreEqual("MediumText", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("VARCHAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForLongText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("LONGTEXT", AuroraDbType.LongText, "LongText");

            // Assert
            Assert.AreEqual("LongText", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("VARCHAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForEnum()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("ENUM('Small','Medium','Large')", AuroraDbType.Enum, "Medium");

            // Assert
            Assert.AreEqual("Medium", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("ENUM", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForSet()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("SET('Read','Write','Execute')", AuroraDbType.Set, "Read,Execute");

            // Assert
            Assert.AreEqual("Read,Execute", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("SET", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Binary

        [TestMethod]
        public void TestAuroraDbNativeTypeForBinary()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("BINARY(4)", AuroraDbType.Binary, new byte[] { 1, 2, 3, 4 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForVarBinary()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("VARBINARY(16)", AuroraDbType.VarBinary, new byte[] { 1, 2, 3 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTinyBlob()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TINYBLOB", AuroraDbType.TinyBlob, new byte[] { 9, 8, 7 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 9, 8, 7 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForBlob()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("BLOB", AuroraDbType.Blob, new byte[] { 9, 8, 7 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 9, 8, 7 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForMediumBlob()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("MEDIUMBLOB", AuroraDbType.MediumBlob, new byte[] { 9, 8, 7 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 9, 8, 7 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForLongBlob()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("LONGBLOB", AuroraDbType.LongBlob, new byte[] { 9, 8, 7 });

            // Assert
            CollectionAssert.AreEqual(new byte[] { 9, 8, 7 }, (byte[])value);
            Assert.AreEqual("BLOB", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Date and Time

        [TestMethod]
        public void TestAuroraDbNativeTypeForDate()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("DATE", AuroraDbType.Date, new DateTime(2026, 9, 25));

            // Assert
            Assert.AreEqual(new DateTime(2026, 9, 25), value);
            Assert.AreEqual("DATE", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTime()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TIME", AuroraDbType.Time, new TimeSpan(13, 45, 30));

            // Assert
            Assert.AreEqual(new TimeSpan(13, 45, 30), value);
            Assert.AreEqual("TIME", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForDateTime()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("DATETIME(6)", AuroraDbType.DateTime, new DateTime(2026, 9, 25, 13, 45, 30));

            // Assert
            Assert.AreEqual(new DateTime(2026, 9, 25, 13, 45, 30), value);
            Assert.AreEqual("DATETIME", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTimestamp()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("TIMESTAMP", AuroraDbType.Timestamp, new DateTime(2026, 9, 25, 13, 45, 30));

            // Assert
            Assert.AreEqual(new DateTime(2026, 9, 25, 13, 45, 30), value);
            Assert.AreEqual("TIMESTAMP", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForYear()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("YEAR", AuroraDbType.Year, 2026);

            // Assert
            Assert.AreEqual(2026, value);
            Assert.AreEqual("YEAR", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region JSON

        [TestMethod]
        public void TestAuroraDbNativeTypeForJson()
        {
            // Act
            var (value, dataTypeName) = RoundTrip("JSON", AuroraDbType.Json, "{\"name\": \"RepoDB\", \"version\": 1}");

            // Assert
            Assert.AreEqual("{\"name\": \"RepoDB\", \"version\": 1}", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("JSON", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Aurora MySQL-specific behaviors

        [TestMethod]
        [DataRow("GEOMETRY")]
        [DataRow("POINT")]
        [DataRow("LINESTRING")]
        [DataRow("POLYGON")]
        [DataRow("MULTIPOINT")]
        [DataRow("MULTILINESTRING")]
        [DataRow("MULTIPOLYGON")]
        [DataRow("GEOMETRYCOLLECTION")]
        public void TestAuroraDbNativeTypeForSpatialColumn(
            string columnDefinition)
        {
            // Act
            var dataTypeName = GetColumnDataTypeName(columnDefinition);

            // Assert - the MySQL protocol reports every spatial type as GEOMETRY
            Assert.AreEqual("GEOMETRY", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForNullValue()
        {
            // Act
            var (value, _) = RoundTrip("VARCHAR(100)", AuroraDbType.VarChar, DBNull.Value);

            // Assert
            Assert.AreEqual(DBNull.Value, value);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTinyIntUnsignedRange()
        {
            // Act
            var (value, _) = RoundTrip("TINYINT UNSIGNED", AuroraDbType.TinyInt, (byte)200);

            // Assert
            Assert.AreEqual((byte)200, value);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForJsonColumnIsNotAliasedToLongText()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("JSON");

            // Assert - unlike MariaDB, JSON is a native type in MySQL (Aurora MySQL)
            Assert.AreEqual("JSON", dataTypeName, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        [DataRow("NOT_A_TYPE")]
        [DataRow("VARCHAR(")]
        [DataRow("INT INT")]
        [DataRow("UUID")]
        public void ThrowOnAuroraDbNativeTypeForInvalidColumnType(
            string columnDefinition)
        {
            // Act
            void Act() => GetColumnDataTypeName(columnDefinition);

            // Assert - UUID is a MariaDB-only type, which is not implemented by Aurora MySQL
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForOutOfRangeValue()
        {
            // Act
            void Act() => RoundTrip("TINYINT", AuroraDbType.TinyInt, 1000);

            // Assert - strict SQL mode rejects the value that does not fit the column
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForInvalidJsonDocument()
        {
            // Act
            void Act() => RoundTrip("JSON", AuroraDbType.Json, "{ this is not json");

            // Assert
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForInvalidEnumValue()
        {
            // Act
            void Act() => RoundTrip("ENUM('Small','Medium','Large')", AuroraDbType.Enum, "ExtraLarge");

            // Assert
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForTextTooLongForColumn()
        {
            // Act
            void Act() => RoundTrip("VARCHAR(5)", AuroraDbType.VarChar, "This is far too long");

            // Assert
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        #endregion
    }
}
