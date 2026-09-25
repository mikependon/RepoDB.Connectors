#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using RepoDb.Connector.CockcroachDb.IntegrationTests.Setup;
using System.Collections;
using System.Net;

namespace RepoDb.Connector.CockcroachDb.IntegrationTests.Operations
{
    /// <summary>
    /// Integration tests that round-trip every <see cref="CockcroachDbType"/> through a real CockroachDB server - the value is
    /// sent as a <see cref="CockcroachDbParameter"/> explicitly typed via <see cref="CockcroachDbParameter.CockcroachDbType"/>,
    /// echoed back by the server, and verified both by value and by the server-reported data type name.
    /// </summary>
    [TestClass]
    public class CockcroachDbNativeTypeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
        }

        #region Helpers

        private static CockcroachDbConnection OpenConnection()
        {
            var connection = new CockcroachDbConnection(Database.ConnectionString);
            connection.Open();
            return connection;
        }

        private static (object Value, string DataTypeName) RoundTrip(
            CockcroachDbType cockcroachDbType,
            object value)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT @value;";
                var parameter = (CockcroachDbParameter)command.CreateParameter();
                parameter.ParameterName = "@value";
                parameter.CockcroachDbType = cockcroachDbType;
                parameter.Value = value;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    return (reader.GetValue(0), reader.GetDataTypeName(0));
                }
            }
        }

        private static T ExecuteScalar<T>(
            string commandText)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                return (T)command.ExecuteScalar();
            }
        }

        private static string GetColumnDataTypeName(
            string columnDefinition)
        {
            using (var connection = OpenConnection())
            {
                Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS \"NativeTypeModel\";");
                Helper.ExecuteNonQuery(connection, $"CREATE TABLE \"NativeTypeModel\" (\"Id\" INT8 PRIMARY KEY, \"Value\" {columnDefinition} NULL);");
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT \"Value\" FROM \"NativeTypeModel\";";
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.GetDataTypeName(0);
                        }
                    }
                }
                finally
                {
                    Helper.ExecuteNonQuery(connection, "DROP TABLE IF EXISTS \"NativeTypeModel\";");
                }
            }
        }

        #endregion

        #region Numeric

        [TestMethod]
        public void TestCockcroachDbNativeTypeForSmallInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.SmallInt, (short)32000);

            // Assert
            Assert.AreEqual((short)32000, value);
            Assert.AreEqual("smallint", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForInteger()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Integer, int.MaxValue);

            // Assert
            Assert.AreEqual(int.MaxValue, value);
            Assert.AreEqual("integer", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForBigInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.BigInt, long.MaxValue);

            // Assert
            Assert.AreEqual(long.MaxValue, value);
            Assert.AreEqual("bigint", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForDecimal()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Decimal, 12345.6789M);

            // Assert
            Assert.AreEqual(12345.6789M, value);
            Assert.AreEqual("numeric", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForReal()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Real, 1.5F);

            // Assert
            Assert.AreEqual(1.5F, value);
            Assert.AreEqual("real", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForDouble()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Double, 3.14159265358979D);

            // Assert
            Assert.AreEqual(3.14159265358979D, value);
            Assert.AreEqual("double precision", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForBoolean()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Boolean, true);

            // Assert
            Assert.IsTrue((bool)value);
            Assert.AreEqual("boolean", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region String

        [TestMethod]
        public void TestCockcroachDbNativeTypeForChar()
        {
            // Act
            var (value, _) = RoundTrip(CockcroachDbType.Char, "A");

            // Assert
            Assert.AreEqual("A", (string)value, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForVarChar()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.VarChar, "VarChar 日本語");

            // Assert
            Assert.AreEqual("VarChar 日本語", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("character varying", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Text, "Text Ünïcödé");

            // Assert
            Assert.AreEqual("Text Ünïcödé", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("text", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForName()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Name, "InsertModel");

            // Assert
            Assert.AreEqual("InsertModel", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("name", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForCitext()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Citext, "CaseInsensitive");

            // Assert
            Assert.AreEqual("CaseInsensitive", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("citext", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForCitextComparesCaseInsensitively()
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                // Setup
                command.CommandText = "SELECT @value = 'CASEINSENSITIVE'::CITEXT;";
                var parameter = (CockcroachDbParameter)command.CreateParameter();
                parameter.ParameterName = "@value";
                parameter.CockcroachDbType = CockcroachDbType.Citext;
                parameter.Value = "caseinsensitive";
                command.Parameters.Add(parameter);

                // Act
                var result = command.ExecuteScalar();

                // Assert
                Assert.IsTrue((bool)result);
            }
        }

        #endregion

        #region Binary

        [TestMethod]
        public void TestCockcroachDbNativeTypeForBytea()
        {
            // Setup
            var expected = new byte[] { 0x00, 0x01, 0xFE, 0xFF };

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Bytea, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (byte[])value);
            Assert.AreEqual("bytea", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Date and Time

        [TestMethod]
        public void TestCockcroachDbNativeTypeForDate()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Date, new DateOnly(2026, 9, 25));

            // Assert
            Assert.AreEqual(new DateOnly(2026, 9, 25), value);
            Assert.AreEqual("date", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTime()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Time, new TimeOnly(13, 45, 30));

            // Assert
            Assert.AreEqual(new TimeOnly(13, 45, 30), value);
            Assert.AreEqual("time without time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTimeTz()
        {
            // Setup
            var expected = new DateTimeOffset(1, 1, 2, 13, 45, 30, TimeSpan.FromHours(2));

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.TimeTz, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("time with time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTimestamp()
        {
            // Setup
            var expected = new DateTime(2026, 9, 25, 13, 45, 30, DateTimeKind.Unspecified).AddTicks(1234560);

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Timestamp, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("timestamp without time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTimestampTz()
        {
            // Setup
            var expected = new DateTime(2026, 9, 25, 13, 45, 30, DateTimeKind.Utc);

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.TimestampTz, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual(DateTimeKind.Utc, ((DateTime)value).Kind);
            Assert.AreEqual("timestamp with time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForInterval()
        {
            // Setup
            var expected = new TimeSpan(3, 4, 5, 6);

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Interval, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("interval", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Network Address

        [TestMethod]
        public void TestCockcroachDbNativeTypeForInet()
        {
            // Setup
            var expected = IPAddress.Parse("192.168.1.10");

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Inet, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("inet", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Bit String

        [TestMethod]
        public void TestCockcroachDbNativeTypeForBit()
        {
            // Setup
            var expected = new BitArray(new[] { true, false, true });

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Bit, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (BitArray)value);
            Assert.AreEqual("bit", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForVarBit()
        {
            // Setup
            var expected = new BitArray(new[] { true, true, false, false, true });

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.VarBit, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (BitArray)value);
            Assert.AreEqual("bit varying", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region JSON

        [TestMethod]
        public void TestCockcroachDbNativeTypeForJsonb()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Jsonb, "{\"name\": \"RepoDB\", \"version\": 1}");

            // Assert
            Assert.AreEqual("{\"name\": \"RepoDB\", \"version\": 1}", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("jsonb", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForJsonColumnIsReportedAsJsonb()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("JSON");

            // Assert - JSON is an alias of JSONB in CockroachDB
            Assert.AreEqual("jsonb", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Other

        [TestMethod]
        public void TestCockcroachDbNativeTypeForUuid()
        {
            // Setup
            var expected = Guid.NewGuid();

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Uuid, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("uuid", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForOid()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.Oid, 26U);

            // Assert
            Assert.AreEqual(26U, value);
            Assert.AreEqual("oid", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForLTree()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.LTree, "Top.Science.Astronomy");

            // Assert
            Assert.AreEqual("Top.Science.Astronomy", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("ltree", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Text Search

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTsVector()
        {
            // Setup - built server-side, as client-side NpgsqlTsVector parsing is obsolete
            var expected = ExecuteScalar<NpgsqlTsVector>("SELECT to_tsvector('simple', 'a fat cat sat on a mat');");

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.TsVector, expected);

            // Assert
            Assert.AreEqual(expected.ToString(), value.ToString(), StringComparer.Ordinal);
            Assert.AreEqual("tsvector", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForTsQuery()
        {
            // Setup - built server-side, as client-side NpgsqlTsQuery parsing is obsolete
            var expected = ExecuteScalar<NpgsqlTsQuery>("SELECT to_tsquery('simple', 'fat & cat');");

            // Act
            var (value, dataTypeName) = RoundTrip(CockcroachDbType.TsQuery, expected);

            // Assert
            Assert.AreEqual(expected.ToString(), value.ToString(), StringComparer.Ordinal);
            Assert.AreEqual("tsquery", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Spatial

        [TestMethod]
        public void TestCockcroachDbNativeTypeForGeometryColumn()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("GEOMETRY");

            // Assert
            Assert.AreEqual("geometry", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestCockcroachDbNativeTypeForGeographyColumn()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("GEOGRAPHY");

            // Assert
            Assert.AreEqual("geography", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region CockroachDB-specific behaviors

        [TestMethod]
        public void TestCockcroachDbNativeTypeForIntColumnIsReportedAsBigInt()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("INT");

            // Assert - unlike PostgreSQL, INT/INTEGER is an INT8 in CockroachDB
            Assert.AreEqual("bigint", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        [DataRow("MONEY")]
        [DataRow("CIDR")]
        [DataRow("MACADDR")]
        [DataRow("XML")]
        [DataRow("POINT")]
        [DataRow("INT4RANGE")]
        public void ThrowOnCockcroachDbNativeTypeForUnsupportedColumnType(
            string columnDefinition)
        {
            // Act
            void Act() => GetColumnDataTypeName(columnDefinition);

            // Assert - these PostgreSQL types are not implemented by CockroachDB (and thus not part of CockcroachDbType)
            Assert.ThrowsExactly<CockcroachDbException>(Act);
        }

        #endregion
    }
}
