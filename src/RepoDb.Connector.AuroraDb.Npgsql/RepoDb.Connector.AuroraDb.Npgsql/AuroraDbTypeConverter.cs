#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace RepoDb.Connector.AuroraDb.Npgsql
{
    /// <summary>
    /// Converts between <see cref="NpgsqlDbType"/> and <see cref="AuroraDbType"/>.
    /// Only the <see cref="NpgsqlDbType"/> values that correspond to a scalar Aurora PostgreSQL type are convertible;
    /// all others (e.g. array, generic range, multirange and internal types) throw a <see cref="NotSupportedException"/>.
    /// </summary>
    public static class AuroraDbTypeConverter
    {
        private static readonly Dictionary<NpgsqlDbType, AuroraDbType> NpgsqlToAurora = new Dictionary<NpgsqlDbType, AuroraDbType>
        {
            { NpgsqlDbType.Smallint, AuroraDbType.SmallInt },
            { NpgsqlDbType.Integer, AuroraDbType.Integer },
            { NpgsqlDbType.Bigint, AuroraDbType.BigInt },
            { NpgsqlDbType.Numeric, AuroraDbType.Decimal },
            { NpgsqlDbType.Real, AuroraDbType.Real },
            { NpgsqlDbType.Double, AuroraDbType.Double },
            { NpgsqlDbType.Money, AuroraDbType.Money },
            { NpgsqlDbType.Boolean, AuroraDbType.Boolean },
            { NpgsqlDbType.Char, AuroraDbType.Char },
            { NpgsqlDbType.Varchar, AuroraDbType.VarChar },
            { NpgsqlDbType.Text, AuroraDbType.Text },
            { NpgsqlDbType.Name, AuroraDbType.Name },
            { NpgsqlDbType.Citext, AuroraDbType.Citext },
            { NpgsqlDbType.Bytea, AuroraDbType.Bytea },
            { NpgsqlDbType.Date, AuroraDbType.Date },
            { NpgsqlDbType.Time, AuroraDbType.Time },
            { NpgsqlDbType.TimeTz, AuroraDbType.TimeTz },
            { NpgsqlDbType.Timestamp, AuroraDbType.Timestamp },
            { NpgsqlDbType.TimestampTz, AuroraDbType.TimestampTz },
            { NpgsqlDbType.Interval, AuroraDbType.Interval },
            { NpgsqlDbType.Inet, AuroraDbType.Inet },
            { NpgsqlDbType.Cidr, AuroraDbType.Cidr },
            { NpgsqlDbType.MacAddr, AuroraDbType.MacAddr },
            { NpgsqlDbType.MacAddr8, AuroraDbType.MacAddr8 },
            { NpgsqlDbType.Bit, AuroraDbType.Bit },
            { NpgsqlDbType.Varbit, AuroraDbType.VarBit },
            { NpgsqlDbType.Json, AuroraDbType.Json },
            { NpgsqlDbType.Jsonb, AuroraDbType.Jsonb },
            { NpgsqlDbType.JsonPath, AuroraDbType.JsonPath },
            { NpgsqlDbType.Xml, AuroraDbType.Xml },
            { NpgsqlDbType.Point, AuroraDbType.Point },
            { NpgsqlDbType.Line, AuroraDbType.Line },
            { NpgsqlDbType.LSeg, AuroraDbType.LSeg },
            { NpgsqlDbType.Box, AuroraDbType.Box },
            { NpgsqlDbType.Path, AuroraDbType.Path },
            { NpgsqlDbType.Polygon, AuroraDbType.Polygon },
            { NpgsqlDbType.Circle, AuroraDbType.Circle },
            { NpgsqlDbType.IntegerRange, AuroraDbType.IntegerRange },
            { NpgsqlDbType.BigIntRange, AuroraDbType.BigIntRange },
            { NpgsqlDbType.NumericRange, AuroraDbType.NumericRange },
            { NpgsqlDbType.TimestampRange, AuroraDbType.TimestampRange },
            { NpgsqlDbType.TimestampTzRange, AuroraDbType.TimestampTzRange },
            { NpgsqlDbType.DateRange, AuroraDbType.DateRange },
            { NpgsqlDbType.Uuid, AuroraDbType.Uuid },
            { NpgsqlDbType.Oid, AuroraDbType.Oid },
            { NpgsqlDbType.Hstore, AuroraDbType.Hstore },
            { NpgsqlDbType.LTree, AuroraDbType.LTree },
            { NpgsqlDbType.TsVector, AuroraDbType.TsVector },
            { NpgsqlDbType.TsQuery, AuroraDbType.TsQuery },
            { NpgsqlDbType.Geometry, AuroraDbType.Geometry },
            { NpgsqlDbType.Geography, AuroraDbType.Geography },
        };

        private static readonly Dictionary<AuroraDbType, NpgsqlDbType> AuroraToNpgsql = BuildReverseMap();

        #region Methods

        /// <summary>
        /// Converts the given <see cref="NpgsqlDbType"/> into its corresponding <see cref="AuroraDbType"/>.
        /// </summary>
        /// <param name="npgsqlDbType">The <see cref="NpgsqlDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="AuroraDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="npgsqlDbType"/> has no corresponding <see cref="AuroraDbType"/>.</exception>
        public static AuroraDbType ToAuroraDbType(
            NpgsqlDbType npgsqlDbType)
        {
            if (NpgsqlToAurora.TryGetValue(npgsqlDbType, out var auroraDbType))
            {
                return auroraDbType;
            }
            throw new NotSupportedException($"The NpgsqlDbType '{npgsqlDbType}' has no corresponding AuroraDbType.");
        }

        /// <summary>
        /// Converts the given <see cref="AuroraDbType"/> into its corresponding <see cref="NpgsqlDbType"/>.
        /// </summary>
        /// <param name="auroraDbType">The <see cref="AuroraDbType"/> to convert.</param>
        /// <returns>The corresponding <see cref="NpgsqlDbType"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="auroraDbType"/> has no corresponding <see cref="NpgsqlDbType"/>.</exception>
        public static NpgsqlDbType ToNpgsqlDbType(
            AuroraDbType auroraDbType)
        {
            if (AuroraToNpgsql.TryGetValue(auroraDbType, out var npgsqlDbType))
            {
                return npgsqlDbType;
            }
            throw new NotSupportedException($"The AuroraDbType '{auroraDbType}' has no corresponding NpgsqlDbType.");
        }

        #endregion

        #region Helpers

        private static Dictionary<AuroraDbType, NpgsqlDbType> BuildReverseMap()
        {
            var map = new Dictionary<AuroraDbType, NpgsqlDbType>(NpgsqlToAurora.Count);
            foreach (var pair in NpgsqlToAurora)
            {
                map.Add(pair.Value, pair.Key);
            }
            return map;
        }

        #endregion
    }
}
