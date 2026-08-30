#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySql.Data.MySqlClient;
using System;

namespace RepoDb.Connector.MariaDb
{
    /// <summary>
    /// Converts between <see cref="MySqlDbType"/> and <see cref="MariaDbType"/>.
    /// </summary>
    public static class MariaDbTypeConverter
    {
        #region Methods

        /// <summary>
        /// Converts the given <see cref="MySqlDbType"/> into its corresponding <see cref="MariaDbType"/>.
        /// </summary>
        /// <param name="mySqlDbType">The <see cref="MySqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="MariaDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="mySqlDbType"/> has no corresponding <see cref="MariaDbType"/>.</exception>
        public static MariaDbType ToMariaDbType(
            MySqlDbType mySqlDbType)
        {
            switch (mySqlDbType)
            {
                case MySqlDbType.Decimal:
                case MySqlDbType.NewDecimal:
                    return MariaDbType.Decimal;
                case MySqlDbType.Byte:
                case MySqlDbType.UByte:
                    return MariaDbType.TinyInt;
                case MySqlDbType.Int16:
                case MySqlDbType.UInt16:
                    return MariaDbType.SmallInt;
                case MySqlDbType.Int24:
                case MySqlDbType.UInt24:
                    return MariaDbType.MediumInt;
                case MySqlDbType.Int32:
                case MySqlDbType.UInt32:
                    return MariaDbType.Int;
                case MySqlDbType.Int64:
                case MySqlDbType.UInt64:
                    return MariaDbType.BigInt;
                case MySqlDbType.Float:
                    return MariaDbType.Float;
                case MySqlDbType.Double:
                    return MariaDbType.Double;
                case MySqlDbType.Bit:
                    return MariaDbType.Bit;
                case MySqlDbType.VarString:
                case MySqlDbType.VarChar:
                    return MariaDbType.VarChar;
                case MySqlDbType.String:
                    return MariaDbType.Char;
                case MySqlDbType.TinyText:
                    return MariaDbType.TinyText;
                case MySqlDbType.Text:
                    return MariaDbType.Text;
                case MySqlDbType.MediumText:
                    return MariaDbType.MediumText;
                case MySqlDbType.LongText:
                    return MariaDbType.LongText;
                case MySqlDbType.Enum:
                    return MariaDbType.Enum;
                case MySqlDbType.Set:
                    return MariaDbType.Set;
                case MySqlDbType.Binary:
                case MySqlDbType.Guid:
                    return MariaDbType.Binary;
                case MySqlDbType.VarBinary:
                    return MariaDbType.VarBinary;
                case MySqlDbType.TinyBlob:
                    return MariaDbType.TinyBlob;
                case MySqlDbType.Blob:
                    return MariaDbType.Blob;
                case MySqlDbType.MediumBlob:
                    return MariaDbType.MediumBlob;
                case MySqlDbType.LongBlob:
                    return MariaDbType.LongBlob;
                case MySqlDbType.Date:
                case MySqlDbType.Newdate:
                    return MariaDbType.Date;
                case MySqlDbType.Time:
                    return MariaDbType.Time;
                case MySqlDbType.DateTime:
                    return MariaDbType.DateTime;
                case MySqlDbType.Timestamp:
                    return MariaDbType.Timestamp;
                case MySqlDbType.Year:
                    return MariaDbType.Year;
                case MySqlDbType.JSON:
                    return MariaDbType.Json;
                case MySqlDbType.Geometry:
                    return MariaDbType.Geometry;
                default:
                    throw new NotSupportedException($"The MySqlDbType '{mySqlDbType}' has no corresponding MariaDbType.");
            }
        }

        /// <summary>
        /// Converts the given <see cref="MariaDbType"/> into its corresponding <see cref="MySqlDbType"/>.
        /// </summary>
        /// <param name="mariaDbType">The <see cref="MariaDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="MySqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="mariaDbType"/> has no corresponding <see cref="MySqlDbType"/>.</exception>
        public static MySqlDbType ToMySqlDbType(
            MariaDbType mariaDbType)
        {
            switch (mariaDbType)
            {
                case MariaDbType.TinyInt:
                    return MySqlDbType.Byte;
                case MariaDbType.SmallInt:
                    return MySqlDbType.Int16;
                case MariaDbType.MediumInt:
                    return MySqlDbType.Int24;
                case MariaDbType.Int:
                    return MySqlDbType.Int32;
                case MariaDbType.BigInt:
                    return MySqlDbType.Int64;
                case MariaDbType.Decimal:
                    return MySqlDbType.Decimal;
                case MariaDbType.Float:
                    return MySqlDbType.Float;
                case MariaDbType.Double:
                    return MySqlDbType.Double;
                case MariaDbType.Bit:
                    return MySqlDbType.Bit;
                case MariaDbType.Char:
                    return MySqlDbType.String;
                case MariaDbType.VarChar:
                    return MySqlDbType.VarChar;
                case MariaDbType.TinyText:
                    return MySqlDbType.TinyText;
                case MariaDbType.Text:
                    return MySqlDbType.Text;
                case MariaDbType.MediumText:
                    return MySqlDbType.MediumText;
                case MariaDbType.LongText:
                    return MySqlDbType.LongText;
                case MariaDbType.Enum:
                    return MySqlDbType.Enum;
                case MariaDbType.Set:
                    return MySqlDbType.Set;
                case MariaDbType.Binary:
                    return MySqlDbType.Binary;
                case MariaDbType.VarBinary:
                    return MySqlDbType.VarBinary;
                case MariaDbType.TinyBlob:
                    return MySqlDbType.TinyBlob;
                case MariaDbType.Blob:
                    return MySqlDbType.Blob;
                case MariaDbType.MediumBlob:
                    return MySqlDbType.MediumBlob;
                case MariaDbType.LongBlob:
                    return MySqlDbType.LongBlob;
                case MariaDbType.Date:
                    return MySqlDbType.Date;
                case MariaDbType.Time:
                    return MySqlDbType.Time;
                case MariaDbType.DateTime:
                    return MySqlDbType.DateTime;
                case MariaDbType.Timestamp:
                    return MySqlDbType.Timestamp;
                case MariaDbType.Year:
                    return MySqlDbType.Year;
                case MariaDbType.Json:
                    return MySqlDbType.JSON;
                case MariaDbType.Geometry:
                case MariaDbType.Point:
                case MariaDbType.LineString:
                case MariaDbType.Polygon:
                case MariaDbType.MultiPoint:
                case MariaDbType.MultiLineString:
                case MariaDbType.MultiPolygon:
                case MariaDbType.GeometryCollection:
                    return MySqlDbType.Geometry;
                default:
                    throw new NotSupportedException($"The MariaDbType '{mariaDbType}' has no corresponding MySqlDbType.");
            }
        }

        #endregion
    }
}
