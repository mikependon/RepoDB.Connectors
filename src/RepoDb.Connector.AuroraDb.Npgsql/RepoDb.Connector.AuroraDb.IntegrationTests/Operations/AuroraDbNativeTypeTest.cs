#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using RepoDb.Connector.AuroraDb.IntegrationTests.Setup;
using System.Collections;
using System.Net;

namespace RepoDb.Connector.AuroraDb.IntegrationTests.Operations
{
    /// <summary>
    /// Integration tests that round-trip every <see cref="AuroraDbType"/> through a real Aurora PostgreSQL-compatible server - the value is
    /// sent as a <see cref="AuroraDbParameter"/> explicitly typed via <see cref="AuroraDbParameter.AuroraDbType"/>,
    /// echoed back by the server, and verified both by value and by the server-reported data type name.
    /// </summary>
    [TestClass]
    public class AuroraDbNativeTypeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
        }

        #region Helpers

        private static AuroraDbConnection OpenConnection()
        {
            var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            return connection;
        }

        private static (object Value, string DataTypeName) RoundTrip(
            AuroraDbType auroraDbType,
            object value)
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT @value;";
                var parameter = (AuroraDbParameter)command.CreateParameter();
                parameter.ParameterName = "@value";
                parameter.AuroraDbType = auroraDbType;
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
        public void TestAuroraDbNativeTypeForSmallInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.SmallInt, (short)32000);

            // Assert
            Assert.AreEqual((short)32000, value);
            Assert.AreEqual("smallint", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForInteger()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Integer, int.MaxValue);

            // Assert
            Assert.AreEqual(int.MaxValue, value);
            Assert.AreEqual("integer", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForBigInt()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.BigInt, long.MaxValue);

            // Assert
            Assert.AreEqual(long.MaxValue, value);
            Assert.AreEqual("bigint", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForDecimal()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Decimal, 12345.6789M);

            // Assert
            Assert.AreEqual(12345.6789M, value);
            Assert.AreEqual("numeric", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForReal()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Real, 1.5F);

            // Assert
            Assert.AreEqual(1.5F, value);
            Assert.AreEqual("real", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForDouble()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Double, 3.14159265358979D);

            // Assert
            Assert.AreEqual(3.14159265358979D, value);
            Assert.AreEqual("double precision", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForBoolean()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Boolean, true);

            // Assert
            Assert.IsTrue((bool)value);
            Assert.AreEqual("boolean", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region String

        [TestMethod]
        public void TestAuroraDbNativeTypeForChar()
        {
            // Act
            var (value, _) = RoundTrip(AuroraDbType.Char, "A");

            // Assert
            Assert.AreEqual("A", (string)value, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForVarChar()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.VarChar, "VarChar 日本語");

            // Assert
            Assert.AreEqual("VarChar 日本語", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("character varying", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForText()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Text, "Text Ünïcödé");

            // Assert
            Assert.AreEqual("Text Ünïcödé", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("text", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForName()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Name, "InsertModel");

            // Assert
            Assert.AreEqual("InsertModel", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("name", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForCitext()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Citext, "CaseInsensitive");

            // Assert
            Assert.AreEqual("CaseInsensitive", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("public.citext", dataTypeName, StringComparer.Ordinal); // extension types are schema-qualified
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForCitextComparesCaseInsensitively()
        {
            using (var connection = OpenConnection())
            using (var command = connection.CreateCommand())
            {
                // Setup
                command.CommandText = "SELECT @value = 'CASEINSENSITIVE'::CITEXT;";
                var parameter = (AuroraDbParameter)command.CreateParameter();
                parameter.ParameterName = "@value";
                parameter.AuroraDbType = AuroraDbType.Citext;
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
        public void TestAuroraDbNativeTypeForBytea()
        {
            // Setup
            var expected = new byte[] { 0x00, 0x01, 0xFE, 0xFF };

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Bytea, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (byte[])value);
            Assert.AreEqual("bytea", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Date and Time

        [TestMethod]
        public void TestAuroraDbNativeTypeForDate()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Date, new DateOnly(2026, 9, 25));

            // Assert
            Assert.AreEqual(new DateTime(2026, 9, 25), value);
            Assert.AreEqual("date", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTime()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Time, new TimeOnly(13, 45, 30));

            // Assert
            Assert.AreEqual(new TimeSpan(13, 45, 30), value);
            Assert.AreEqual("time without time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTimeTz()
        {
            // Setup
            var expected = new DateTimeOffset(1, 1, 2, 13, 45, 30, TimeSpan.FromHours(2));

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.TimeTz, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("time with time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTimestamp()
        {
            // Setup
            var expected = new DateTime(2026, 9, 25, 13, 45, 30, DateTimeKind.Unspecified).AddTicks(1234560);

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Timestamp, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("timestamp without time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTimestampTz()
        {
            // Setup
            var expected = new DateTime(2026, 9, 25, 13, 45, 30, DateTimeKind.Utc);

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.TimestampTz, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual(DateTimeKind.Utc, ((DateTime)value).Kind);
            Assert.AreEqual("timestamp with time zone", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForInterval()
        {
            // Setup
            var expected = new TimeSpan(3, 4, 5, 6);

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Interval, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("interval", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Network Address

        [TestMethod]
        public void TestAuroraDbNativeTypeForInet()
        {
            // Setup
            var expected = IPAddress.Parse("192.168.1.10");

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Inet, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("inet", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Bit String

        [TestMethod]
        public void TestAuroraDbNativeTypeForBit()
        {
            // Setup
            var expected = new BitArray(new[] { true, false, true });

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Bit, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (BitArray)value);
            Assert.AreEqual("bit", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForVarBit()
        {
            // Setup
            var expected = new BitArray(new[] { true, true, false, false, true });

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.VarBit, expected);

            // Assert
            CollectionAssert.AreEqual(expected, (BitArray)value);
            Assert.AreEqual("bit varying", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region JSON

        [TestMethod]
        public void TestAuroraDbNativeTypeForJsonb()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Jsonb, "{\"name\": \"RepoDB\", \"version\": 1}");

            // Assert
            Assert.AreEqual("{\"name\": \"RepoDB\", \"version\": 1}", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("jsonb", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForJson()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Json, "{\"name\":  \"RepoDB\"}");

            // Assert - unlike JSONB, JSON preserves the original text (including whitespaces)
            Assert.AreEqual("{\"name\":  \"RepoDB\"}", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("json", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForJsonColumnIsNotAliasedToJsonb()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("JSON");

            // Assert
            Assert.AreEqual("json", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForJsonbNormalizesWhitespaces()
        {
            // Act
            var (value, _) = RoundTrip(AuroraDbType.Jsonb, "{\"name\":   \"RepoDB\"}");

            // Assert
            Assert.AreEqual("{\"name\": \"RepoDB\"}", (string)value, StringComparer.Ordinal);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForJsonWithInvalidDocument()
        {
            // Act
            void Act() => RoundTrip(AuroraDbType.Jsonb, "{ this is not json");

            // Assert
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        #endregion

        #region Other

        [TestMethod]
        public void TestAuroraDbNativeTypeForUuid()
        {
            // Setup
            var expected = Guid.NewGuid();

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Uuid, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("uuid", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForOid()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Oid, 26U);

            // Assert
            Assert.AreEqual(26U, value);
            Assert.AreEqual("oid", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForLTree()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.LTree, "Top.Science.Astronomy");

            // Assert
            Assert.AreEqual("Top.Science.Astronomy", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("public.ltree", dataTypeName, StringComparer.Ordinal); // extension types are schema-qualified
        }

        #endregion

        #region Text Search

        [TestMethod]
        public void TestAuroraDbNativeTypeForTsVector()
        {
            // Setup - built server-side, as client-side NpgsqlTsVector parsing is obsolete
            var expected = ExecuteScalar<NpgsqlTsVector>("SELECT to_tsvector('simple', 'a fat cat sat on a mat');");

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.TsVector, expected);

            // Assert
            Assert.AreEqual(expected.ToString(), value.ToString(), StringComparer.Ordinal);
            Assert.AreEqual("tsvector", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForTsQuery()
        {
            // Setup - built server-side, as client-side NpgsqlTsQuery parsing is obsolete
            var expected = ExecuteScalar<NpgsqlTsQuery>("SELECT to_tsquery('simple', 'fat & cat');");

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.TsQuery, expected);

            // Assert
            Assert.AreEqual(expected.ToString(), value.ToString(), StringComparer.Ordinal);
            Assert.AreEqual("tsquery", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Spatial

        [TestMethod]
        public void TestAuroraDbNativeTypeForGeometryColumn()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("GEOMETRY");

            // Assert
            Assert.AreEqual("public.geometry", dataTypeName, StringComparer.Ordinal); // PostGIS types are schema-qualified
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForGeographyColumn()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("GEOGRAPHY");

            // Assert
            Assert.AreEqual("public.geography", dataTypeName, StringComparer.Ordinal); // PostGIS types are schema-qualified
        }

        #endregion

        #region PostgreSQL-specific types

        [TestMethod]
        public void TestAuroraDbNativeTypeForMoney()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Money, 12.34M);

            // Assert
            Assert.AreEqual(12.34M, value);
            Assert.AreEqual("money", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForXml()
        {
            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Xml, "<root><name>RepoDB</name></root>");

            // Assert
            Assert.AreEqual("<root><name>RepoDB</name></root>", (string)value, StringComparer.Ordinal);
            Assert.AreEqual("xml", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForMacAddr()
        {
            // Setup
            var expected = System.Net.NetworkInformation.PhysicalAddress.Parse("08-00-2B-01-02-03");

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.MacAddr, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("macaddr", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForPoint()
        {
            // Setup
            var expected = new NpgsqlPoint(1.5, 2.5);

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.Point, expected);

            // Assert
            Assert.AreEqual(expected, value);
            Assert.AreEqual("point", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForIntegerRange()
        {
            // Setup
            var input = new NpgsqlRange<int>(1, 10);

            // Act
            var (value, dataTypeName) = RoundTrip(AuroraDbType.IntegerRange, input);

            // Assert - PostgreSQL canonicalizes discrete ranges to the inclusive-lower/exclusive-upper form
            Assert.AreEqual("[1,11)", value.ToString(), StringComparer.Ordinal);
            Assert.AreEqual("int4range", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        public void TestAuroraDbNativeTypeForCidrColumn()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("CIDR");

            // Assert
            Assert.AreEqual("cidr", dataTypeName, StringComparer.Ordinal);
        }

        #endregion

        #region Aurora PostgreSQL-specific behaviors

        [TestMethod]
        public void TestAuroraDbNativeTypeForIntColumnIsReportedAsInteger()
        {
            // Act
            var dataTypeName = GetColumnDataTypeName("INT");

            // Assert - unlike CockroachDB, INT/INTEGER is an INT4 in PostgreSQL
            Assert.AreEqual("integer", dataTypeName, StringComparer.Ordinal);
        }

        [TestMethod]
        [DataRow("NOT_A_TYPE")]
        [DataRow("VARCHAR(")]
        [DataRow("INT4 INT4")]
        public void ThrowOnAuroraDbNativeTypeForInvalidColumnType(
            string columnDefinition)
        {
            // Act
            void Act() => GetColumnDataTypeName(columnDefinition);

            // Assert
            Assert.ThrowsExactly<AuroraDbException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbNativeTypeForMismatchedParameterValue()
        {
            // Act
            void Act() => RoundTrip(AuroraDbType.Uuid, "not-a-uuid-value");

            // Assert
            Assert.ThrowsExactly<InvalidCastException>(Act);
        }

        #endregion
    }
}
