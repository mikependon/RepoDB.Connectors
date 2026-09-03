#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using System;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Converts between <see cref="NpgsqlDbType"/> and <see cref="EDBType"/>.
    /// </summary>
    public static class EDBTypeConverter
    {
        #region Methods

        /// <summary>
        /// Converts the given <see cref="NpgsqlDbType"/> into its corresponding <see cref="EDBType"/>.
        /// </summary>
        /// <param name="npgsqlDbType">The <see cref="NpgsqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="EDBType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="npgsqlDbType"/> has no corresponding <see cref="EDBType"/>.</exception>
        public static EDBType ToEDBType(
            NpgsqlDbType npgsqlDbType)
        {
            switch (npgsqlDbType)
            {
                case NpgsqlDbType.Smallint:
                    return EDBType.SmallInt;
                case NpgsqlDbType.Integer:
                    return EDBType.Integer;
                case NpgsqlDbType.Bigint:
                    return EDBType.BigInt;
                case NpgsqlDbType.Numeric:
                    return EDBType.Decimal;
                case NpgsqlDbType.Real:
                    return EDBType.Real;
                case NpgsqlDbType.Double:
                    return EDBType.Double;
                case NpgsqlDbType.Money:
                    return EDBType.Money;
                case NpgsqlDbType.Boolean:
                    return EDBType.Boolean;
                case NpgsqlDbType.Char:
                    return EDBType.Char;
                case NpgsqlDbType.Varchar:
                    return EDBType.VarChar;
                case NpgsqlDbType.Text:
                    return EDBType.Text;
                case NpgsqlDbType.Name:
                    return EDBType.Name;
                case NpgsqlDbType.Citext:
                    return EDBType.Citext;
                case NpgsqlDbType.Bytea:
                    return EDBType.Bytea;
                case NpgsqlDbType.Date:
                    return EDBType.Date;
                case NpgsqlDbType.Time:
                    return EDBType.Time;
                case NpgsqlDbType.TimeTz:
                    return EDBType.TimeTz;
                case NpgsqlDbType.Timestamp:
                    return EDBType.Timestamp;
                case NpgsqlDbType.TimestampTz:
                    return EDBType.TimestampTz;
                case NpgsqlDbType.Interval:
                    return EDBType.Interval;
                case NpgsqlDbType.Inet:
                    return EDBType.Inet;
                case NpgsqlDbType.Cidr:
                    return EDBType.Cidr;
                case NpgsqlDbType.MacAddr:
                    return EDBType.MacAddr;
                case NpgsqlDbType.MacAddr8:
                    return EDBType.MacAddr8;
                case NpgsqlDbType.Bit:
                    return EDBType.Bit;
                case NpgsqlDbType.Varbit:
                    return EDBType.VarBit;
                case NpgsqlDbType.Json:
                    return EDBType.Json;
                case NpgsqlDbType.Jsonb:
                    return EDBType.Jsonb;
                case NpgsqlDbType.JsonPath:
                    return EDBType.JsonPath;
                case NpgsqlDbType.Uuid:
                    return EDBType.Uuid;
                case NpgsqlDbType.Xml:
                    return EDBType.Xml;
                case NpgsqlDbType.Hstore:
                    return EDBType.Hstore;
                case NpgsqlDbType.TsVector:
                    return EDBType.TsVector;
                case NpgsqlDbType.TsQuery:
                    return EDBType.TsQuery;
                case NpgsqlDbType.Point:
                    return EDBType.Point;
                case NpgsqlDbType.Line:
                    return EDBType.Line;
                case NpgsqlDbType.LSeg:
                    return EDBType.LSeg;
                case NpgsqlDbType.Box:
                    return EDBType.Box;
                case NpgsqlDbType.Path:
                    return EDBType.Path;
                case NpgsqlDbType.Polygon:
                    return EDBType.Polygon;
                case NpgsqlDbType.Circle:
                    return EDBType.Circle;
                case NpgsqlDbType.IntegerRange:
                    return EDBType.IntegerRange;
                case NpgsqlDbType.BigIntRange:
                    return EDBType.BigIntRange;
                case NpgsqlDbType.NumericRange:
                    return EDBType.NumericRange;
                case NpgsqlDbType.DateRange:
                    return EDBType.DateRange;
                case NpgsqlDbType.TimestampRange:
                    return EDBType.TimestampRange;
                case NpgsqlDbType.TimestampTzRange:
                    return EDBType.TimestampTzRange;
                default:
                    throw new NotSupportedException($"The NpgsqlDbType '{npgsqlDbType}' has no corresponding EDBType.");
            }
        }

        /// <summary>
        /// Converts the given <see cref="EDBType"/> into its corresponding <see cref="NpgsqlDbType"/>.
        /// </summary>
        /// <param name="edbType">The <see cref="EDBType"/> to convert.</param>
        /// <returns>The corresponding <see cref="NpgsqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="edbType"/> has no corresponding <see cref="NpgsqlDbType"/>.</exception>
        public static NpgsqlDbType ToNpgsqlDbType(
            EDBType edbType)
        {
            switch (edbType)
            {
                case EDBType.SmallInt:
                    return NpgsqlDbType.Smallint;
                case EDBType.Integer:
                    return NpgsqlDbType.Integer;
                case EDBType.BigInt:
                    return NpgsqlDbType.Bigint;
                case EDBType.Decimal:
                    return NpgsqlDbType.Numeric;
                case EDBType.Real:
                    return NpgsqlDbType.Real;
                case EDBType.Double:
                    return NpgsqlDbType.Double;
                case EDBType.Money:
                    return NpgsqlDbType.Money;
                case EDBType.Boolean:
                    return NpgsqlDbType.Boolean;
                case EDBType.Char:
                    return NpgsqlDbType.Char;
                case EDBType.VarChar:
                    return NpgsqlDbType.Varchar;
                case EDBType.Text:
                    return NpgsqlDbType.Text;
                case EDBType.Name:
                    return NpgsqlDbType.Name;
                case EDBType.Citext:
                    return NpgsqlDbType.Citext;
                case EDBType.Bytea:
                    return NpgsqlDbType.Bytea;
                case EDBType.Date:
                    return NpgsqlDbType.Date;
                case EDBType.Time:
                    return NpgsqlDbType.Time;
                case EDBType.TimeTz:
                    return NpgsqlDbType.TimeTz;
                case EDBType.Timestamp:
                    return NpgsqlDbType.Timestamp;
                case EDBType.TimestampTz:
                    return NpgsqlDbType.TimestampTz;
                case EDBType.Interval:
                    return NpgsqlDbType.Interval;
                case EDBType.Inet:
                    return NpgsqlDbType.Inet;
                case EDBType.Cidr:
                    return NpgsqlDbType.Cidr;
                case EDBType.MacAddr:
                    return NpgsqlDbType.MacAddr;
                case EDBType.MacAddr8:
                    return NpgsqlDbType.MacAddr8;
                case EDBType.Bit:
                    return NpgsqlDbType.Bit;
                case EDBType.VarBit:
                    return NpgsqlDbType.Varbit;
                case EDBType.Json:
                    return NpgsqlDbType.Json;
                case EDBType.Jsonb:
                    return NpgsqlDbType.Jsonb;
                case EDBType.JsonPath:
                    return NpgsqlDbType.JsonPath;
                case EDBType.Uuid:
                    return NpgsqlDbType.Uuid;
                case EDBType.Xml:
                    return NpgsqlDbType.Xml;
                case EDBType.Hstore:
                    return NpgsqlDbType.Hstore;
                case EDBType.TsVector:
                    return NpgsqlDbType.TsVector;
                case EDBType.TsQuery:
                    return NpgsqlDbType.TsQuery;
                case EDBType.Point:
                    return NpgsqlDbType.Point;
                case EDBType.Line:
                    return NpgsqlDbType.Line;
                case EDBType.LSeg:
                    return NpgsqlDbType.LSeg;
                case EDBType.Box:
                    return NpgsqlDbType.Box;
                case EDBType.Path:
                    return NpgsqlDbType.Path;
                case EDBType.Polygon:
                    return NpgsqlDbType.Polygon;
                case EDBType.Circle:
                    return NpgsqlDbType.Circle;
                case EDBType.IntegerRange:
                    return NpgsqlDbType.IntegerRange;
                case EDBType.BigIntRange:
                    return NpgsqlDbType.BigIntRange;
                case EDBType.NumericRange:
                    return NpgsqlDbType.NumericRange;
                case EDBType.DateRange:
                    return NpgsqlDbType.DateRange;
                case EDBType.TimestampRange:
                    return NpgsqlDbType.TimestampRange;
                case EDBType.TimestampTzRange:
                    return NpgsqlDbType.TimestampTzRange;
                default:
                    throw new NotSupportedException($"The EDBType '{edbType}' has no corresponding NpgsqlDbType.");
            }
        }

        #endregion
    }
}
