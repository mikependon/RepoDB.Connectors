#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;

namespace RepoDb.Connector.AuroraDb.Npgsql.UnitTests
{
    [TestClass]
    public sealed class AuroraDbTypeConverterTest
    {
        #region ToNpgsqlDbType

        [TestMethod]
        [DataRow(AuroraDbType.SmallInt, NpgsqlDbType.Smallint)]
        [DataRow(AuroraDbType.Integer, NpgsqlDbType.Integer)]
        [DataRow(AuroraDbType.BigInt, NpgsqlDbType.Bigint)]
        [DataRow(AuroraDbType.Decimal, NpgsqlDbType.Numeric)]
        [DataRow(AuroraDbType.Real, NpgsqlDbType.Real)]
        [DataRow(AuroraDbType.Double, NpgsqlDbType.Double)]
        [DataRow(AuroraDbType.Money, NpgsqlDbType.Money)]
        [DataRow(AuroraDbType.Boolean, NpgsqlDbType.Boolean)]
        [DataRow(AuroraDbType.Char, NpgsqlDbType.Char)]
        [DataRow(AuroraDbType.VarChar, NpgsqlDbType.Varchar)]
        [DataRow(AuroraDbType.Text, NpgsqlDbType.Text)]
        [DataRow(AuroraDbType.Name, NpgsqlDbType.Name)]
        [DataRow(AuroraDbType.Citext, NpgsqlDbType.Citext)]
        [DataRow(AuroraDbType.Bytea, NpgsqlDbType.Bytea)]
        [DataRow(AuroraDbType.Date, NpgsqlDbType.Date)]
        [DataRow(AuroraDbType.Time, NpgsqlDbType.Time)]
        [DataRow(AuroraDbType.TimeTz, NpgsqlDbType.TimeTz)]
        [DataRow(AuroraDbType.Timestamp, NpgsqlDbType.Timestamp)]
        [DataRow(AuroraDbType.TimestampTz, NpgsqlDbType.TimestampTz)]
        [DataRow(AuroraDbType.Interval, NpgsqlDbType.Interval)]
        [DataRow(AuroraDbType.Inet, NpgsqlDbType.Inet)]
        [DataRow(AuroraDbType.Cidr, NpgsqlDbType.Cidr)]
        [DataRow(AuroraDbType.MacAddr, NpgsqlDbType.MacAddr)]
        [DataRow(AuroraDbType.MacAddr8, NpgsqlDbType.MacAddr8)]
        [DataRow(AuroraDbType.Bit, NpgsqlDbType.Bit)]
        [DataRow(AuroraDbType.VarBit, NpgsqlDbType.Varbit)]
        [DataRow(AuroraDbType.Json, NpgsqlDbType.Json)]
        [DataRow(AuroraDbType.Jsonb, NpgsqlDbType.Jsonb)]
        [DataRow(AuroraDbType.JsonPath, NpgsqlDbType.JsonPath)]
        [DataRow(AuroraDbType.Xml, NpgsqlDbType.Xml)]
        [DataRow(AuroraDbType.Point, NpgsqlDbType.Point)]
        [DataRow(AuroraDbType.Line, NpgsqlDbType.Line)]
        [DataRow(AuroraDbType.LSeg, NpgsqlDbType.LSeg)]
        [DataRow(AuroraDbType.Box, NpgsqlDbType.Box)]
        [DataRow(AuroraDbType.Path, NpgsqlDbType.Path)]
        [DataRow(AuroraDbType.Polygon, NpgsqlDbType.Polygon)]
        [DataRow(AuroraDbType.Circle, NpgsqlDbType.Circle)]
        [DataRow(AuroraDbType.IntegerRange, NpgsqlDbType.IntegerRange)]
        [DataRow(AuroraDbType.BigIntRange, NpgsqlDbType.BigIntRange)]
        [DataRow(AuroraDbType.NumericRange, NpgsqlDbType.NumericRange)]
        [DataRow(AuroraDbType.TimestampRange, NpgsqlDbType.TimestampRange)]
        [DataRow(AuroraDbType.TimestampTzRange, NpgsqlDbType.TimestampTzRange)]
        [DataRow(AuroraDbType.DateRange, NpgsqlDbType.DateRange)]
        [DataRow(AuroraDbType.Uuid, NpgsqlDbType.Uuid)]
        [DataRow(AuroraDbType.Oid, NpgsqlDbType.Oid)]
        [DataRow(AuroraDbType.Hstore, NpgsqlDbType.Hstore)]
        [DataRow(AuroraDbType.LTree, NpgsqlDbType.LTree)]
        [DataRow(AuroraDbType.TsVector, NpgsqlDbType.TsVector)]
        [DataRow(AuroraDbType.TsQuery, NpgsqlDbType.TsQuery)]
        [DataRow(AuroraDbType.Geometry, NpgsqlDbType.Geometry)]
        [DataRow(AuroraDbType.Geography, NpgsqlDbType.Geography)]
        public void TestAuroraDbTypeConverterToNpgsqlDbType(
            AuroraDbType input,
            NpgsqlDbType expected)
        {
            // Act
            var output = AuroraDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToNpgsqlDbTypeForAllAuroraDbTypes()
        {
            // Setup
            var inputs = Enum.GetValues<AuroraDbType>();

            foreach (var input in inputs)
            {
                // Act
                var output = AuroraDbTypeConverter.ToNpgsqlDbType(input);

                // Assert - every AuroraDbType must round-trip back to itself
                Assert.AreEqual(input, AuroraDbTypeConverter.ToAuroraDbType(output));
            }
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToNpgsqlDbTypeForAllAuroraDbTypesAreDistinct()
        {
            // Setup
            var inputs = Enum.GetValues<AuroraDbType>();

            // Act
            var outputs = inputs.Select(AuroraDbTypeConverter.ToNpgsqlDbType).ToHashSet();

            // Assert
            Assert.HasCount(inputs.Length, outputs);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(-1)]
        public void TestAuroraDbTypeConverterToNpgsqlDbTypeForUndefinedValueThrowsNotSupportedException(
            int value)
        {
            // Setup
            var input = (AuroraDbType)value;

            // Act
            void Act() => AuroraDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion

        #region ToAuroraDbType

        [TestMethod]
        [DataRow(NpgsqlDbType.Smallint, AuroraDbType.SmallInt)]
        [DataRow(NpgsqlDbType.Integer, AuroraDbType.Integer)]
        [DataRow(NpgsqlDbType.Bigint, AuroraDbType.BigInt)]
        [DataRow(NpgsqlDbType.Numeric, AuroraDbType.Decimal)]
        [DataRow(NpgsqlDbType.Real, AuroraDbType.Real)]
        [DataRow(NpgsqlDbType.Double, AuroraDbType.Double)]
        [DataRow(NpgsqlDbType.Money, AuroraDbType.Money)]
        [DataRow(NpgsqlDbType.Boolean, AuroraDbType.Boolean)]
        [DataRow(NpgsqlDbType.Char, AuroraDbType.Char)]
        [DataRow(NpgsqlDbType.Varchar, AuroraDbType.VarChar)]
        [DataRow(NpgsqlDbType.Text, AuroraDbType.Text)]
        [DataRow(NpgsqlDbType.Name, AuroraDbType.Name)]
        [DataRow(NpgsqlDbType.Citext, AuroraDbType.Citext)]
        [DataRow(NpgsqlDbType.Bytea, AuroraDbType.Bytea)]
        [DataRow(NpgsqlDbType.Date, AuroraDbType.Date)]
        [DataRow(NpgsqlDbType.Time, AuroraDbType.Time)]
        [DataRow(NpgsqlDbType.TimeTz, AuroraDbType.TimeTz)]
        [DataRow(NpgsqlDbType.Timestamp, AuroraDbType.Timestamp)]
        [DataRow(NpgsqlDbType.TimestampTz, AuroraDbType.TimestampTz)]
        [DataRow(NpgsqlDbType.Interval, AuroraDbType.Interval)]
        [DataRow(NpgsqlDbType.Inet, AuroraDbType.Inet)]
        [DataRow(NpgsqlDbType.Cidr, AuroraDbType.Cidr)]
        [DataRow(NpgsqlDbType.MacAddr, AuroraDbType.MacAddr)]
        [DataRow(NpgsqlDbType.MacAddr8, AuroraDbType.MacAddr8)]
        [DataRow(NpgsqlDbType.Bit, AuroraDbType.Bit)]
        [DataRow(NpgsqlDbType.Varbit, AuroraDbType.VarBit)]
        [DataRow(NpgsqlDbType.Json, AuroraDbType.Json)]
        [DataRow(NpgsqlDbType.Jsonb, AuroraDbType.Jsonb)]
        [DataRow(NpgsqlDbType.JsonPath, AuroraDbType.JsonPath)]
        [DataRow(NpgsqlDbType.Xml, AuroraDbType.Xml)]
        [DataRow(NpgsqlDbType.Point, AuroraDbType.Point)]
        [DataRow(NpgsqlDbType.Line, AuroraDbType.Line)]
        [DataRow(NpgsqlDbType.LSeg, AuroraDbType.LSeg)]
        [DataRow(NpgsqlDbType.Box, AuroraDbType.Box)]
        [DataRow(NpgsqlDbType.Path, AuroraDbType.Path)]
        [DataRow(NpgsqlDbType.Polygon, AuroraDbType.Polygon)]
        [DataRow(NpgsqlDbType.Circle, AuroraDbType.Circle)]
        [DataRow(NpgsqlDbType.IntegerRange, AuroraDbType.IntegerRange)]
        [DataRow(NpgsqlDbType.BigIntRange, AuroraDbType.BigIntRange)]
        [DataRow(NpgsqlDbType.NumericRange, AuroraDbType.NumericRange)]
        [DataRow(NpgsqlDbType.TimestampRange, AuroraDbType.TimestampRange)]
        [DataRow(NpgsqlDbType.TimestampTzRange, AuroraDbType.TimestampTzRange)]
        [DataRow(NpgsqlDbType.DateRange, AuroraDbType.DateRange)]
        [DataRow(NpgsqlDbType.Uuid, AuroraDbType.Uuid)]
        [DataRow(NpgsqlDbType.Oid, AuroraDbType.Oid)]
        [DataRow(NpgsqlDbType.Hstore, AuroraDbType.Hstore)]
        [DataRow(NpgsqlDbType.LTree, AuroraDbType.LTree)]
        [DataRow(NpgsqlDbType.TsVector, AuroraDbType.TsVector)]
        [DataRow(NpgsqlDbType.TsQuery, AuroraDbType.TsQuery)]
        [DataRow(NpgsqlDbType.Geometry, AuroraDbType.Geometry)]
        [DataRow(NpgsqlDbType.Geography, AuroraDbType.Geography)]
        public void TestAuroraDbTypeConverterToAuroraDbType(
            NpgsqlDbType input,
            AuroraDbType expected)
        {
            // Act
            var output = AuroraDbTypeConverter.ToAuroraDbType(input);

            // Assert
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestAuroraDbTypeConverterToAuroraDbTypeForJsonIsNotAliasedToJsonb()
        {
            // Act
            var json = AuroraDbTypeConverter.ToAuroraDbType(NpgsqlDbType.Json);
            var jsonb = AuroraDbTypeConverter.ToAuroraDbType(NpgsqlDbType.Jsonb);

            // Assert - unlike CockroachDB, Aurora PostgreSQL has distinct JSON and JSONB types
            Assert.AreNotEqual(json, jsonb);
        }

        /// <summary>
        /// Composite/internal NpgsqlDbType values (arrays, generic ranges, multiranges, ...) have no scalar AuroraDbType.
        /// </summary>
        [TestMethod]
        [DataRow(NpgsqlDbType.Array)]
        [DataRow(NpgsqlDbType.Range)]
        [DataRow(NpgsqlDbType.Multirange)]
        [DataRow(NpgsqlDbType.Array | NpgsqlDbType.Integer)]
        [DataRow(NpgsqlDbType.Range | NpgsqlDbType.Geometry)]
        [DataRow(NpgsqlDbType.Unknown)]
        [DataRow(NpgsqlDbType.Refcursor)]
        [DataRow(NpgsqlDbType.Regconfig)]
        [DataRow((NpgsqlDbType)int.MaxValue)]
        public void TestAuroraDbTypeConverterToAuroraDbTypeForUnsupportedTypeThrowsNotSupportedException(
            NpgsqlDbType input)
        {
            // Act
            void Act() => AuroraDbTypeConverter.ToAuroraDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion
    }
}
