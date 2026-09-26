#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System;

namespace RepoDb.Connector.AuroraDb.MySqlConnector
{
    /// <summary>
    /// Converts between <see cref="MySqlDbType"/> and <see cref="AuroraDbType"/>.
    /// </summary>
    public static class AuroraDbTypeConverter
    {
        #region Methods

        /// <summary>
        /// Converts the given <see cref="MySqlDbType"/> into its corresponding <see cref="AuroraDbType"/>.
        /// </summary>
        /// <param name="mySqlDbType">The <see cref="MySqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="AuroraDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="mySqlDbType"/> has no corresponding <see cref="AuroraDbType"/>.</exception>
        public static AuroraDbType ToAuroraDbType(
            MySqlDbType mySqlDbType)
        {
            switch (mySqlDbType)
            {
                case MySqlDbType.Decimal:
                case MySqlDbType.NewDecimal:
                    return AuroraDbType.Decimal;
                case MySqlDbType.Byte:
                case MySqlDbType.UByte:
                    return AuroraDbType.TinyInt;
                case MySqlDbType.Int16:
                case MySqlDbType.UInt16:
                    return AuroraDbType.SmallInt;
                case MySqlDbType.Int24:
                case MySqlDbType.UInt24:
                    return AuroraDbType.MediumInt;
                case MySqlDbType.Int32:
                case MySqlDbType.UInt32:
                    return AuroraDbType.Int;
                case MySqlDbType.Int64:
                case MySqlDbType.UInt64:
                    return AuroraDbType.BigInt;
                case MySqlDbType.Float:
                    return AuroraDbType.Float;
                case MySqlDbType.Double:
                    return AuroraDbType.Double;
                case MySqlDbType.Bit:
                    return AuroraDbType.Bit;
                case MySqlDbType.VarString:
                case MySqlDbType.VarChar:
                    return AuroraDbType.VarChar;
                case MySqlDbType.String:
                    return AuroraDbType.Char;
                case MySqlDbType.TinyText:
                    return AuroraDbType.TinyText;
                case MySqlDbType.Text:
                    return AuroraDbType.Text;
                case MySqlDbType.MediumText:
                    return AuroraDbType.MediumText;
                case MySqlDbType.LongText:
                    return AuroraDbType.LongText;
                case MySqlDbType.Enum:
                    return AuroraDbType.Enum;
                case MySqlDbType.Set:
                    return AuroraDbType.Set;
                case MySqlDbType.Binary:
                case MySqlDbType.Guid:
                    return AuroraDbType.Binary;
                case MySqlDbType.VarBinary:
                    return AuroraDbType.VarBinary;
                case MySqlDbType.TinyBlob:
                    return AuroraDbType.TinyBlob;
                case MySqlDbType.Blob:
                    return AuroraDbType.Blob;
                case MySqlDbType.MediumBlob:
                    return AuroraDbType.MediumBlob;
                case MySqlDbType.LongBlob:
                    return AuroraDbType.LongBlob;
                case MySqlDbType.Date:
                case MySqlDbType.Newdate:
                    return AuroraDbType.Date;
                case MySqlDbType.Time:
                    return AuroraDbType.Time;
                case MySqlDbType.DateTime:
                    return AuroraDbType.DateTime;
                case MySqlDbType.Timestamp:
                    return AuroraDbType.Timestamp;
                case MySqlDbType.Year:
                    return AuroraDbType.Year;
                case MySqlDbType.JSON:
                    return AuroraDbType.Json;
                case MySqlDbType.Geometry:
                    return AuroraDbType.Geometry;
                default:
                    throw new NotSupportedException($"The MySqlDbType '{mySqlDbType}' has no corresponding AuroraDbType.");
            }
        }

        /// <summary>
        /// Converts the given <see cref="AuroraDbType"/> into its corresponding <see cref="MySqlDbType"/>.
        /// </summary>
        /// <param name="auroraDbType">The <see cref="AuroraDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="MySqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="auroraDbType"/> has no corresponding <see cref="MySqlDbType"/>.</exception>
        public static MySqlDbType ToMySqlDbType(
            AuroraDbType auroraDbType)
        {
            switch (auroraDbType)
            {
                case AuroraDbType.TinyInt:
                    return MySqlDbType.Byte;
                case AuroraDbType.SmallInt:
                    return MySqlDbType.Int16;
                case AuroraDbType.MediumInt:
                    return MySqlDbType.Int24;
                case AuroraDbType.Int:
                    return MySqlDbType.Int32;
                case AuroraDbType.BigInt:
                    return MySqlDbType.Int64;
                case AuroraDbType.Decimal:
                    return MySqlDbType.Decimal;
                case AuroraDbType.Float:
                    return MySqlDbType.Float;
                case AuroraDbType.Double:
                    return MySqlDbType.Double;
                case AuroraDbType.Bit:
                    return MySqlDbType.Bit;
                case AuroraDbType.Char:
                    return MySqlDbType.String;
                case AuroraDbType.VarChar:
                    return MySqlDbType.VarChar;
                case AuroraDbType.TinyText:
                    return MySqlDbType.TinyText;
                case AuroraDbType.Text:
                    return MySqlDbType.Text;
                case AuroraDbType.MediumText:
                    return MySqlDbType.MediumText;
                case AuroraDbType.LongText:
                    return MySqlDbType.LongText;
                case AuroraDbType.Enum:
                    return MySqlDbType.Enum;
                case AuroraDbType.Set:
                    return MySqlDbType.Set;
                case AuroraDbType.Binary:
                    return MySqlDbType.Binary;
                case AuroraDbType.VarBinary:
                    return MySqlDbType.VarBinary;
                case AuroraDbType.TinyBlob:
                    return MySqlDbType.TinyBlob;
                case AuroraDbType.Blob:
                    return MySqlDbType.Blob;
                case AuroraDbType.MediumBlob:
                    return MySqlDbType.MediumBlob;
                case AuroraDbType.LongBlob:
                    return MySqlDbType.LongBlob;
                case AuroraDbType.Date:
                    return MySqlDbType.Date;
                case AuroraDbType.Time:
                    return MySqlDbType.Time;
                case AuroraDbType.DateTime:
                    return MySqlDbType.DateTime;
                case AuroraDbType.Timestamp:
                    return MySqlDbType.Timestamp;
                case AuroraDbType.Year:
                    return MySqlDbType.Year;
                case AuroraDbType.Json:
                    return MySqlDbType.JSON;
                case AuroraDbType.Geometry:
                case AuroraDbType.Point:
                case AuroraDbType.LineString:
                case AuroraDbType.Polygon:
                case AuroraDbType.MultiPoint:
                case AuroraDbType.MultiLineString:
                case AuroraDbType.MultiPolygon:
                case AuroraDbType.GeometryCollection:
                    return MySqlDbType.Geometry;
                default:
                    throw new NotSupportedException($"The AuroraDbType '{auroraDbType}' has no corresponding MySqlDbType.");
            }
        }

        #endregion
    }
}
