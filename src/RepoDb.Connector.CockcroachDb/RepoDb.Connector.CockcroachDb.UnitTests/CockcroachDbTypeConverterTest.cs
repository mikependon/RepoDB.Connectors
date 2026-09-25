#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;

namespace RepoDb.Connector.CockcroachDb.UnitTests
{
    [TestClass]
    public sealed class CockcroachDbTypeConverterTest
    {
        #region ToNpgsqlDbType

        [TestMethod]
        [DataRow(CockcroachDbType.SmallInt, NpgsqlDbType.Smallint)]
        [DataRow(CockcroachDbType.Integer, NpgsqlDbType.Integer)]
        [DataRow(CockcroachDbType.BigInt, NpgsqlDbType.Bigint)]
        [DataRow(CockcroachDbType.Decimal, NpgsqlDbType.Numeric)]
        [DataRow(CockcroachDbType.Real, NpgsqlDbType.Real)]
        [DataRow(CockcroachDbType.Double, NpgsqlDbType.Double)]
        [DataRow(CockcroachDbType.Boolean, NpgsqlDbType.Boolean)]
        [DataRow(CockcroachDbType.Char, NpgsqlDbType.Char)]
        [DataRow(CockcroachDbType.VarChar, NpgsqlDbType.Varchar)]
        [DataRow(CockcroachDbType.Text, NpgsqlDbType.Text)]
        [DataRow(CockcroachDbType.Name, NpgsqlDbType.Name)]
        [DataRow(CockcroachDbType.Citext, NpgsqlDbType.Citext)]
        [DataRow(CockcroachDbType.Bytea, NpgsqlDbType.Bytea)]
        [DataRow(CockcroachDbType.Date, NpgsqlDbType.Date)]
        [DataRow(CockcroachDbType.Time, NpgsqlDbType.Time)]
        [DataRow(CockcroachDbType.TimeTz, NpgsqlDbType.TimeTz)]
        [DataRow(CockcroachDbType.Timestamp, NpgsqlDbType.Timestamp)]
        [DataRow(CockcroachDbType.TimestampTz, NpgsqlDbType.TimestampTz)]
        [DataRow(CockcroachDbType.Interval, NpgsqlDbType.Interval)]
        [DataRow(CockcroachDbType.Inet, NpgsqlDbType.Inet)]
        [DataRow(CockcroachDbType.Bit, NpgsqlDbType.Bit)]
        [DataRow(CockcroachDbType.VarBit, NpgsqlDbType.Varbit)]
        [DataRow(CockcroachDbType.Jsonb, NpgsqlDbType.Jsonb)]
        [DataRow(CockcroachDbType.Uuid, NpgsqlDbType.Uuid)]
        [DataRow(CockcroachDbType.Oid, NpgsqlDbType.Oid)]
        [DataRow(CockcroachDbType.LTree, NpgsqlDbType.LTree)]
        [DataRow(CockcroachDbType.TsVector, NpgsqlDbType.TsVector)]
        [DataRow(CockcroachDbType.TsQuery, NpgsqlDbType.TsQuery)]
        [DataRow(CockcroachDbType.Geometry, NpgsqlDbType.Geometry)]
        [DataRow(CockcroachDbType.Geography, NpgsqlDbType.Geography)]
        public void TestCockcroachDbTypeConverterToNpgsqlDbType(
            CockcroachDbType input,
            NpgsqlDbType expected)
        {
            // Act
            var output = CockcroachDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestCockcroachDbTypeConverterToNpgsqlDbTypeForAllCockcroachDbTypes()
        {
            // Setup
            var inputs = Enum.GetValues<CockcroachDbType>();

            foreach (var input in inputs)
            {
                // Act
                var output = CockcroachDbTypeConverter.ToNpgsqlDbType(input);

                // Assert - every CockcroachDbType must round-trip back to itself
                Assert.AreEqual(input, CockcroachDbTypeConverter.ToCockcroachDbType(output));
            }
        }

        [TestMethod]
        public void TestCockcroachDbTypeConverterToNpgsqlDbTypeForUndefinedValueThrowsNotSupportedException()
        {
            // Setup
            var input = (CockcroachDbType)int.MaxValue;

            // Act
            void Act() => CockcroachDbTypeConverter.ToNpgsqlDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion

        #region ToCockcroachDbType

        [TestMethod]
        [DataRow(NpgsqlDbType.Smallint, CockcroachDbType.SmallInt)]
        [DataRow(NpgsqlDbType.Integer, CockcroachDbType.Integer)]
        [DataRow(NpgsqlDbType.Bigint, CockcroachDbType.BigInt)]
        [DataRow(NpgsqlDbType.Numeric, CockcroachDbType.Decimal)]
        [DataRow(NpgsqlDbType.Real, CockcroachDbType.Real)]
        [DataRow(NpgsqlDbType.Double, CockcroachDbType.Double)]
        [DataRow(NpgsqlDbType.Boolean, CockcroachDbType.Boolean)]
        [DataRow(NpgsqlDbType.Char, CockcroachDbType.Char)]
        [DataRow(NpgsqlDbType.Varchar, CockcroachDbType.VarChar)]
        [DataRow(NpgsqlDbType.Text, CockcroachDbType.Text)]
        [DataRow(NpgsqlDbType.Name, CockcroachDbType.Name)]
        [DataRow(NpgsqlDbType.Citext, CockcroachDbType.Citext)]
        [DataRow(NpgsqlDbType.Bytea, CockcroachDbType.Bytea)]
        [DataRow(NpgsqlDbType.Date, CockcroachDbType.Date)]
        [DataRow(NpgsqlDbType.Time, CockcroachDbType.Time)]
        [DataRow(NpgsqlDbType.TimeTz, CockcroachDbType.TimeTz)]
        [DataRow(NpgsqlDbType.Timestamp, CockcroachDbType.Timestamp)]
        [DataRow(NpgsqlDbType.TimestampTz, CockcroachDbType.TimestampTz)]
        [DataRow(NpgsqlDbType.Interval, CockcroachDbType.Interval)]
        [DataRow(NpgsqlDbType.Inet, CockcroachDbType.Inet)]
        [DataRow(NpgsqlDbType.Bit, CockcroachDbType.Bit)]
        [DataRow(NpgsqlDbType.Varbit, CockcroachDbType.VarBit)]
        [DataRow(NpgsqlDbType.Json, CockcroachDbType.Jsonb)]
        [DataRow(NpgsqlDbType.Jsonb, CockcroachDbType.Jsonb)]
        [DataRow(NpgsqlDbType.Uuid, CockcroachDbType.Uuid)]
        [DataRow(NpgsqlDbType.Oid, CockcroachDbType.Oid)]
        [DataRow(NpgsqlDbType.LTree, CockcroachDbType.LTree)]
        [DataRow(NpgsqlDbType.TsVector, CockcroachDbType.TsVector)]
        [DataRow(NpgsqlDbType.TsQuery, CockcroachDbType.TsQuery)]
        [DataRow(NpgsqlDbType.Geometry, CockcroachDbType.Geometry)]
        [DataRow(NpgsqlDbType.Geography, CockcroachDbType.Geography)]
        public void TestCockcroachDbTypeConverterToCockcroachDbType(
            NpgsqlDbType input,
            CockcroachDbType expected)
        {
            // Act
            var output = CockcroachDbTypeConverter.ToCockcroachDbType(input);

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
        public void TestCockcroachDbTypeConverterToCockcroachDbTypeForUnsupportedTypeThrowsNotSupportedException(
            NpgsqlDbType input)
        {
            // Act
            void Act() => CockcroachDbTypeConverter.ToCockcroachDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        [TestMethod]
        public void TestCockcroachDbTypeConverterToCockcroachDbTypeForArrayThrowsNotSupportedException()
        {
            // Setup
            var input = NpgsqlDbType.Array;

            // Act
            void Act() => CockcroachDbTypeConverter.ToCockcroachDbType(input);

            // Assert
            Assert.ThrowsExactly<NotSupportedException>(Act);
        }

        #endregion
    }
}
