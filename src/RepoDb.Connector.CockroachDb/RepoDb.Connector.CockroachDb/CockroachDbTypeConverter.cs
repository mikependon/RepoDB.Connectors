#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using System;

namespace RepoDb.Connector.CockroachDb
{
    /// <summary>
    /// Converts between <see cref="NpgsqlDbType"/> and <see cref="CockroachDbType"/>.
    /// Only the <see cref="NpgsqlDbType"/> values that correspond to a type supported by CockroachDB are convertible;
    /// all others (e.g. MONEY, CIDR, MACADDR, XML, HSTORE, the geometric and the range types) throw a <see cref="NotSupportedException"/>.
    /// </summary>
    public static class CockroachDbTypeConverter
    {
        #region Methods

        /// <summary>
        /// Converts the given <see cref="NpgsqlDbType"/> into its corresponding <see cref="CockroachDbType"/>.
        /// </summary>
        /// <param name="npgsqlDbType">The <see cref="NpgsqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="CockroachDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="npgsqlDbType"/> has no corresponding <see cref="CockroachDbType"/>.</exception>
        public static CockroachDbType ToCockroachDbType(
            NpgsqlDbType npgsqlDbType)
        {
            switch (npgsqlDbType)
            {
                case NpgsqlDbType.Smallint:
                    return CockroachDbType.SmallInt;
                case NpgsqlDbType.Integer:
                    return CockroachDbType.Integer;
                case NpgsqlDbType.Bigint:
                    return CockroachDbType.BigInt;
                case NpgsqlDbType.Numeric:
                    return CockroachDbType.Decimal;
                case NpgsqlDbType.Real:
                    return CockroachDbType.Real;
                case NpgsqlDbType.Double:
                    return CockroachDbType.Double;
                case NpgsqlDbType.Boolean:
                    return CockroachDbType.Boolean;
                case NpgsqlDbType.Char:
                    return CockroachDbType.Char;
                case NpgsqlDbType.Varchar:
                    return CockroachDbType.VarChar;
                case NpgsqlDbType.Text:
                    return CockroachDbType.Text;
                case NpgsqlDbType.Name:
                    return CockroachDbType.Name;
                case NpgsqlDbType.Citext:
                    return CockroachDbType.Citext;
                case NpgsqlDbType.Bytea:
                    return CockroachDbType.Bytea;
                case NpgsqlDbType.Date:
                    return CockroachDbType.Date;
                case NpgsqlDbType.Time:
                    return CockroachDbType.Time;
                case NpgsqlDbType.TimeTz:
                    return CockroachDbType.TimeTz;
                case NpgsqlDbType.Timestamp:
                    return CockroachDbType.Timestamp;
                case NpgsqlDbType.TimestampTz:
                    return CockroachDbType.TimestampTz;
                case NpgsqlDbType.Interval:
                    return CockroachDbType.Interval;
                case NpgsqlDbType.Inet:
                    return CockroachDbType.Inet;
                case NpgsqlDbType.Bit:
                    return CockroachDbType.Bit;
                case NpgsqlDbType.Varbit:
                    return CockroachDbType.VarBit;
                // JSON is an alias of JSONB in CockroachDB
                case NpgsqlDbType.Json:
                case NpgsqlDbType.Jsonb:
                    return CockroachDbType.Jsonb;
                case NpgsqlDbType.Uuid:
                    return CockroachDbType.Uuid;
                case NpgsqlDbType.Oid:
                    return CockroachDbType.Oid;
                case NpgsqlDbType.LTree:
                    return CockroachDbType.LTree;
                case NpgsqlDbType.TsVector:
                    return CockroachDbType.TsVector;
                case NpgsqlDbType.TsQuery:
                    return CockroachDbType.TsQuery;
                case NpgsqlDbType.Geometry:
                    return CockroachDbType.Geometry;
                case NpgsqlDbType.Geography:
                    return CockroachDbType.Geography;
                default:
                    throw new NotSupportedException($"The NpgsqlDbType '{npgsqlDbType}' has no corresponding CockroachDbType.");
            }
        }

        /// <summary>
        /// Converts the given <see cref="CockroachDbType"/> into its corresponding <see cref="NpgsqlDbType"/>.
        /// </summary>
        /// <param name="cockroachDbType">The <see cref="CockroachDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="NpgsqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="cockroachDbType"/> has no corresponding <see cref="NpgsqlDbType"/>.</exception>
        public static NpgsqlDbType ToNpgsqlDbType(
            CockroachDbType cockroachDbType)
        {
            switch (cockroachDbType)
            {
                case CockroachDbType.SmallInt:
                    return NpgsqlDbType.Smallint;
                case CockroachDbType.Integer:
                    return NpgsqlDbType.Integer;
                case CockroachDbType.BigInt:
                    return NpgsqlDbType.Bigint;
                case CockroachDbType.Decimal:
                    return NpgsqlDbType.Numeric;
                case CockroachDbType.Real:
                    return NpgsqlDbType.Real;
                case CockroachDbType.Double:
                    return NpgsqlDbType.Double;
                case CockroachDbType.Boolean:
                    return NpgsqlDbType.Boolean;
                case CockroachDbType.Char:
                    return NpgsqlDbType.Char;
                case CockroachDbType.VarChar:
                    return NpgsqlDbType.Varchar;
                case CockroachDbType.Text:
                    return NpgsqlDbType.Text;
                case CockroachDbType.Name:
                    return NpgsqlDbType.Name;
                case CockroachDbType.Citext:
                    return NpgsqlDbType.Citext;
                case CockroachDbType.Bytea:
                    return NpgsqlDbType.Bytea;
                case CockroachDbType.Date:
                    return NpgsqlDbType.Date;
                case CockroachDbType.Time:
                    return NpgsqlDbType.Time;
                case CockroachDbType.TimeTz:
                    return NpgsqlDbType.TimeTz;
                case CockroachDbType.Timestamp:
                    return NpgsqlDbType.Timestamp;
                case CockroachDbType.TimestampTz:
                    return NpgsqlDbType.TimestampTz;
                case CockroachDbType.Interval:
                    return NpgsqlDbType.Interval;
                case CockroachDbType.Inet:
                    return NpgsqlDbType.Inet;
                case CockroachDbType.Bit:
                    return NpgsqlDbType.Bit;
                case CockroachDbType.VarBit:
                    return NpgsqlDbType.Varbit;
                case CockroachDbType.Jsonb:
                    return NpgsqlDbType.Jsonb;
                case CockroachDbType.Uuid:
                    return NpgsqlDbType.Uuid;
                case CockroachDbType.Oid:
                    return NpgsqlDbType.Oid;
                case CockroachDbType.LTree:
                    return NpgsqlDbType.LTree;
                case CockroachDbType.TsVector:
                    return NpgsqlDbType.TsVector;
                case CockroachDbType.TsQuery:
                    return NpgsqlDbType.TsQuery;
                case CockroachDbType.Geometry:
                    return NpgsqlDbType.Geometry;
                case CockroachDbType.Geography:
                    return NpgsqlDbType.Geography;
                default:
                    throw new NotSupportedException($"The CockroachDbType '{cockroachDbType}' has no corresponding NpgsqlDbType.");
            }
        }

        #endregion
    }
}
