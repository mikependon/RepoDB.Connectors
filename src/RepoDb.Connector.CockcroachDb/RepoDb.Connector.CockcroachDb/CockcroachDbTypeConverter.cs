#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using System;

namespace RepoDb.Connector.CockcroachDb
{
    /// <summary>
    /// Converts between <see cref="NpgsqlDbType"/> and <see cref="CockcroachDbType"/>.
    /// Only the <see cref="NpgsqlDbType"/> values that correspond to a type supported by CockroachDB are convertible;
    /// all others (e.g. MONEY, CIDR, MACADDR, XML, HSTORE, the geometric and the range types) throw a <see cref="NotSupportedException"/>.
    /// </summary>
    public static class CockcroachDbTypeConverter
    {
        #region Methods

        /// <summary>
        /// Converts the given <see cref="NpgsqlDbType"/> into its corresponding <see cref="CockcroachDbType"/>.
        /// </summary>
        /// <param name="npgsqlDbType">The <see cref="NpgsqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="CockcroachDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="npgsqlDbType"/> has no corresponding <see cref="CockcroachDbType"/>.</exception>
        public static CockcroachDbType ToCockcroachDbType(
            NpgsqlDbType npgsqlDbType)
        {
            switch (npgsqlDbType)
            {
                case NpgsqlDbType.Smallint:
                    return CockcroachDbType.SmallInt;
                case NpgsqlDbType.Integer:
                    return CockcroachDbType.Integer;
                case NpgsqlDbType.Bigint:
                    return CockcroachDbType.BigInt;
                case NpgsqlDbType.Numeric:
                    return CockcroachDbType.Decimal;
                case NpgsqlDbType.Real:
                    return CockcroachDbType.Real;
                case NpgsqlDbType.Double:
                    return CockcroachDbType.Double;
                case NpgsqlDbType.Boolean:
                    return CockcroachDbType.Boolean;
                case NpgsqlDbType.Char:
                    return CockcroachDbType.Char;
                case NpgsqlDbType.Varchar:
                    return CockcroachDbType.VarChar;
                case NpgsqlDbType.Text:
                    return CockcroachDbType.Text;
                case NpgsqlDbType.Name:
                    return CockcroachDbType.Name;
                case NpgsqlDbType.Citext:
                    return CockcroachDbType.Citext;
                case NpgsqlDbType.Bytea:
                    return CockcroachDbType.Bytea;
                case NpgsqlDbType.Date:
                    return CockcroachDbType.Date;
                case NpgsqlDbType.Time:
                    return CockcroachDbType.Time;
                case NpgsqlDbType.TimeTz:
                    return CockcroachDbType.TimeTz;
                case NpgsqlDbType.Timestamp:
                    return CockcroachDbType.Timestamp;
                case NpgsqlDbType.TimestampTz:
                    return CockcroachDbType.TimestampTz;
                case NpgsqlDbType.Interval:
                    return CockcroachDbType.Interval;
                case NpgsqlDbType.Inet:
                    return CockcroachDbType.Inet;
                case NpgsqlDbType.Bit:
                    return CockcroachDbType.Bit;
                case NpgsqlDbType.Varbit:
                    return CockcroachDbType.VarBit;
                // JSON is an alias of JSONB in CockroachDB
                case NpgsqlDbType.Json:
                case NpgsqlDbType.Jsonb:
                    return CockcroachDbType.Jsonb;
                case NpgsqlDbType.Uuid:
                    return CockcroachDbType.Uuid;
                case NpgsqlDbType.Oid:
                    return CockcroachDbType.Oid;
                case NpgsqlDbType.LTree:
                    return CockcroachDbType.LTree;
                case NpgsqlDbType.TsVector:
                    return CockcroachDbType.TsVector;
                case NpgsqlDbType.TsQuery:
                    return CockcroachDbType.TsQuery;
                case NpgsqlDbType.Geometry:
                    return CockcroachDbType.Geometry;
                case NpgsqlDbType.Geography:
                    return CockcroachDbType.Geography;
                default:
                    throw new NotSupportedException($"The NpgsqlDbType '{npgsqlDbType}' has no corresponding CockcroachDbType.");
            }
        }

        /// <summary>
        /// Converts the given <see cref="CockcroachDbType"/> into its corresponding <see cref="NpgsqlDbType"/>.
        /// </summary>
        /// <param name="cockcroachDbType">The <see cref="CockcroachDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="NpgsqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="cockcroachDbType"/> has no corresponding <see cref="NpgsqlDbType"/>.</exception>
        public static NpgsqlDbType ToNpgsqlDbType(
            CockcroachDbType cockcroachDbType)
        {
            switch (cockcroachDbType)
            {
                case CockcroachDbType.SmallInt:
                    return NpgsqlDbType.Smallint;
                case CockcroachDbType.Integer:
                    return NpgsqlDbType.Integer;
                case CockcroachDbType.BigInt:
                    return NpgsqlDbType.Bigint;
                case CockcroachDbType.Decimal:
                    return NpgsqlDbType.Numeric;
                case CockcroachDbType.Real:
                    return NpgsqlDbType.Real;
                case CockcroachDbType.Double:
                    return NpgsqlDbType.Double;
                case CockcroachDbType.Boolean:
                    return NpgsqlDbType.Boolean;
                case CockcroachDbType.Char:
                    return NpgsqlDbType.Char;
                case CockcroachDbType.VarChar:
                    return NpgsqlDbType.Varchar;
                case CockcroachDbType.Text:
                    return NpgsqlDbType.Text;
                case CockcroachDbType.Name:
                    return NpgsqlDbType.Name;
                case CockcroachDbType.Citext:
                    return NpgsqlDbType.Citext;
                case CockcroachDbType.Bytea:
                    return NpgsqlDbType.Bytea;
                case CockcroachDbType.Date:
                    return NpgsqlDbType.Date;
                case CockcroachDbType.Time:
                    return NpgsqlDbType.Time;
                case CockcroachDbType.TimeTz:
                    return NpgsqlDbType.TimeTz;
                case CockcroachDbType.Timestamp:
                    return NpgsqlDbType.Timestamp;
                case CockcroachDbType.TimestampTz:
                    return NpgsqlDbType.TimestampTz;
                case CockcroachDbType.Interval:
                    return NpgsqlDbType.Interval;
                case CockcroachDbType.Inet:
                    return NpgsqlDbType.Inet;
                case CockcroachDbType.Bit:
                    return NpgsqlDbType.Bit;
                case CockcroachDbType.VarBit:
                    return NpgsqlDbType.Varbit;
                case CockcroachDbType.Jsonb:
                    return NpgsqlDbType.Jsonb;
                case CockcroachDbType.Uuid:
                    return NpgsqlDbType.Uuid;
                case CockcroachDbType.Oid:
                    return NpgsqlDbType.Oid;
                case CockcroachDbType.LTree:
                    return NpgsqlDbType.LTree;
                case CockcroachDbType.TsVector:
                    return NpgsqlDbType.TsVector;
                case CockcroachDbType.TsQuery:
                    return NpgsqlDbType.TsQuery;
                case CockcroachDbType.Geometry:
                    return NpgsqlDbType.Geometry;
                case CockcroachDbType.Geography:
                    return NpgsqlDbType.Geography;
                default:
                    throw new NotSupportedException($"The CockcroachDbType '{cockcroachDbType}' has no corresponding NpgsqlDbType.");
            }
        }

        #endregion
    }
}
