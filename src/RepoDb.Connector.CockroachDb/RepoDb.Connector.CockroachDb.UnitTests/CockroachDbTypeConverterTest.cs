#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;

namespace RepoDb.Connector.CockroachDb.UnitTests
{
    [TestClass]
    public sealed class CockroachDbTypeConverterTest
    {
        #region ToNpgsqlDbType

        [TestMethod]
        [DataRow(CockroachDbType.SmallInt, NpgsqlDbType.Smallint)]
        [DataRow(CockroachDbType.Integer, NpgsqlDbType.Integer)]
        [DataRow(CockroachDbType.BigInt, NpgsqlDbType.Bigint)]
        [DataRow(CockroachDbType.Decimal, NpgsqlDbType.Numeric)]
        [DataRow(CockroachDbType.Real, NpgsqlDbType.Real)]
        [DataRow(CockroachDbType.Double, NpgsqlDbType.Double)]
        [DataRow(CockroachDbType.Boolean, NpgsqlDbType.Boolean)]
        [DataRow(CockroachDbType.Char, NpgsqlDbType.Char)]
        [DataRow(CockroachDbType.VarChar, NpgsqlDbType.Varchar)]
        [DataRow(CockroachDbType.Text, NpgsqlDbType.Text)]
        [DataRow(CockroachDbType.Name, NpgsqlDbType.Name)]
        [DataRow(CockroachDbType.Citext, NpgsqlDbType.Citext)]
        [DataRow(CockroachDbType.Bytea, NpgsqlDbType.Bytea)]
        [DataRow(CockroachDbType.Date, NpgsqlDbType.Date)]
        [DataRow(CockroachDbType.Time, NpgsqlDbType.Time)]
        [DataRow(CockroachDbType.TimeTz, NpgsqlDbType.TimeTz)]
        [DataRow(CockroachDbType.Timestamp, NpgsqlDbType.Timestamp)]
        [DataRow(CockroachDbType.TimestampTz, NpgsqlDbType.TimestampTz)]
        [DataRow(CockroachDbType.Interval, NpgsqlDbType.Interval)]
        [DataRow(CockroachDbType.Inet, NpgsqlDbType.Inet)]
        [DataRow(CockroachDbType.Bit, NpgsqlDbType.Bit)]
        [DataRow(CockroachDbType.VarBit, NpgsqlDbType.Varbit)]
        [DataRow(CockroachDbType.Jsonb, NpgsqlDbType.Jsonb)]
        [DataRow(CockroachDbType.Uuid, NpgsqlDbType.Uuid)]
        [DataRow(CockroachDbType.Oid, NpgsqlDbType.Oid)]
        [DataRow(CockroachDbType.LTree, NpgsqlDbType.LTree)]
        [DataRow(CockroachDbType.TsVector, NpgsqlDbType.TsVector)]
        [DataRow(CockroachDbType.TsQuery, NpgsqlDbType.TsQuery)]
        [DataRow(CockroachDbType.Geometry, NpgsqlDbType.Geometry)]
        [DataRow(CockroachDbType.Geography, NpgsqlDbType.Geography)]
        public void TestCockroachDbTypeConverterToNpgsqlDbType(
            CockroachDbType input,
            NpgsqlDbType expected)
        {
            // Act
            var output = CockroachDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestCockroachDbTypeConverterToNpgsqlDbTypeForAllCockroachDbTypes()
        {
            // Setup
            var inputs = Enum.GetValues<CockroachDbType>();

            foreach (var input in inputs)
            {
                // Act
                var output = CockroachDbTypeConverter.ToNpgsqlDbType(input);

                // Assert - every CockroachDbType must round-trip back to itself
                Assert.AreEqual(input, CockroachDbTypeConverter.ToCockroachDbType(output));
            }
        }

        [TestMethod]
        public void TestCockroachDbTypeConverterToNpgsqlDbTypeForUndefinedValueThrowsNotSupportedException()
        {
            // Setup
            var input = (CockroachDbType)int.MaxValue;

            // Act
            void Act() => CockroachDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion

        #region ToCockroachDbType

        [TestMethod]
        [DataRow(NpgsqlDbType.Smallint, CockroachDbType.SmallInt)]
        [DataRow(NpgsqlDbType.Integer, CockroachDbType.Integer)]
        [DataRow(NpgsqlDbType.Bigint, CockroachDbType.BigInt)]
        [DataRow(NpgsqlDbType.Numeric, CockroachDbType.Decimal)]
        [DataRow(NpgsqlDbType.Real, CockroachDbType.Real)]
        [DataRow(NpgsqlDbType.Double, CockroachDbType.Double)]
        [DataRow(NpgsqlDbType.Boolean, CockroachDbType.Boolean)]
        [DataRow(NpgsqlDbType.Char, CockroachDbType.Char)]
        [DataRow(NpgsqlDbType.Varchar, CockroachDbType.VarChar)]
        [DataRow(NpgsqlDbType.Text, CockroachDbType.Text)]
        [DataRow(NpgsqlDbType.Name, CockroachDbType.Name)]
        [DataRow(NpgsqlDbType.Citext, CockroachDbType.Citext)]
        [DataRow(NpgsqlDbType.Bytea, CockroachDbType.Bytea)]
        [DataRow(NpgsqlDbType.Date, CockroachDbType.Date)]
        [DataRow(NpgsqlDbType.Time, CockroachDbType.Time)]
        [DataRow(NpgsqlDbType.TimeTz, CockroachDbType.TimeTz)]
        [DataRow(NpgsqlDbType.Timestamp, CockroachDbType.Timestamp)]
        [DataRow(NpgsqlDbType.TimestampTz, CockroachDbType.TimestampTz)]
        [DataRow(NpgsqlDbType.Interval, CockroachDbType.Interval)]
        [DataRow(NpgsqlDbType.Inet, CockroachDbType.Inet)]
        [DataRow(NpgsqlDbType.Bit, CockroachDbType.Bit)]
        [DataRow(NpgsqlDbType.Varbit, CockroachDbType.VarBit)]
        [DataRow(NpgsqlDbType.Json, CockroachDbType.Jsonb)]
        [DataRow(NpgsqlDbType.Jsonb, CockroachDbType.Jsonb)]
        [DataRow(NpgsqlDbType.Uuid, CockroachDbType.Uuid)]
        [DataRow(NpgsqlDbType.Oid, CockroachDbType.Oid)]
        [DataRow(NpgsqlDbType.LTree, CockroachDbType.LTree)]
        [DataRow(NpgsqlDbType.TsVector, CockroachDbType.TsVector)]
        [DataRow(NpgsqlDbType.TsQuery, CockroachDbType.TsQuery)]
        [DataRow(NpgsqlDbType.Geometry, CockroachDbType.Geometry)]
        [DataRow(NpgsqlDbType.Geography, CockroachDbType.Geography)]
        public void TestCockroachDbTypeConverterToCockroachDbType(
            NpgsqlDbType input,
            CockroachDbType expected)
        {
            // Act
            var output = CockroachDbTypeConverter.ToCockroachDbType(input);

            // Assert
            Assert.AreEqual(expected, output);
        }

        /// <summary>
        /// PostgreSQL types that CockroachDB does not implement must not be convertible.
        /// </summary>
        [TestMethod]
        [DataRow(NpgsqlDbType.Money)]
        [DataRow(NpgsqlDbType.Cidr)]
        [DataRow(NpgsqlDbType.MacAddr)]
        [DataRow(NpgsqlDbType.MacAddr8)]
        [DataRow(NpgsqlDbType.Xml)]
        [DataRow(NpgsqlDbType.Hstore)]
        [DataRow(NpgsqlDbType.JsonPath)]
        [DataRow(NpgsqlDbType.Point)]
        [DataRow(NpgsqlDbType.Line)]
        [DataRow(NpgsqlDbType.LSeg)]
        [DataRow(NpgsqlDbType.Box)]
        [DataRow(NpgsqlDbType.Path)]
        [DataRow(NpgsqlDbType.Polygon)]
        [DataRow(NpgsqlDbType.Circle)]
        [DataRow(NpgsqlDbType.IntegerRange)]
        [DataRow(NpgsqlDbType.BigIntRange)]
        [DataRow(NpgsqlDbType.NumericRange)]
        [DataRow(NpgsqlDbType.DateRange)]
        [DataRow(NpgsqlDbType.TimestampRange)]
        [DataRow(NpgsqlDbType.TimestampTzRange)]
        public void TestCockroachDbTypeConverterToCockroachDbTypeForUnsupportedTypeThrowsNotSupportedException(
            NpgsqlDbType input)
        {
            // Act
            void Act() => CockroachDbTypeConverter.ToCockroachDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        [TestMethod]
        public void TestCockroachDbTypeConverterToCockroachDbTypeForArrayThrowsNotSupportedException()
        {
            // Setup
            var input = NpgsqlDbType.Array;

            // Act
            void Act() => CockroachDbTypeConverter.ToCockroachDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion
    }
}
