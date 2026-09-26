#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.Npgsql
{
    /// <summary>
    /// Specifies the native Aurora PostgreSQL column type of a <see cref="AuroraDbParameter"/> or result column, in addition to the standard ADO.NET <see cref="System.Data.DbType"/>.
    /// Aurora PostgreSQL is PostgreSQL-compatible, therefore the type system mirrors that of PostgreSQL (and of the underlying Npgsql provider).
    /// </summary>
    public enum AuroraDbType
    {
        #region Numeric

        /// <summary>
        /// A 2-byte signed integer. Corresponds to the PostgreSQL INT2 (SMALLINT) type.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A 4-byte signed integer. Corresponds to the PostgreSQL INT4 (INTEGER) type.
        /// </summary>
        Integer,

        /// <summary>
        /// An 8-byte signed integer. Corresponds to the PostgreSQL INT8 (BIGINT) type.
        /// </summary>
        BigInt,

        /// <summary>
        /// An exact numeric of selectable precision. Corresponds to the PostgreSQL NUMERIC (DECIMAL) type.
        /// </summary>
        Decimal,

        /// <summary>
        /// A single-precision, inexact floating-point number. Corresponds to the PostgreSQL FLOAT4 (REAL) type.
        /// </summary>
        Real,

        /// <summary>
        /// A double-precision, inexact floating-point number. Corresponds to the PostgreSQL FLOAT8 (DOUBLE PRECISION) type.
        /// </summary>
        Double,

        /// <summary>
        /// A currency amount. Corresponds to the PostgreSQL MONEY type.
        /// </summary>
        Money,

        /// <summary>
        /// A logical Boolean value. Corresponds to the PostgreSQL BOOL (BOOLEAN) type.
        /// </summary>
        Boolean,

        #endregion

        #region String

        /// <summary>
        /// A fixed-length, blank-padded string. Corresponds to the PostgreSQL CHAR(n) type.
        /// </summary>
        Char,

        /// <summary>
        /// A variable-length string with an optional limit. Corresponds to the PostgreSQL VARCHAR(n) type.
        /// </summary>
        VarChar,

        /// <summary>
        /// A variable, unlimited-length string. Corresponds to the PostgreSQL TEXT type.
        /// </summary>
        Text,

        /// <summary>
        /// An internal type for object names, as returned by the <c>pg_catalog</c> tables. Corresponds to the PostgreSQL NAME type.
        /// </summary>
        Name,

        /// <summary>
        /// A case-insensitive string. Corresponds to the PostgreSQL CITEXT type (requires the <c>citext</c> extension).
        /// </summary>
        Citext,

        #endregion

        #region Binary

        /// <summary>
        /// A variable-length binary string. Corresponds to the PostgreSQL BYTEA type.
        /// </summary>
        Bytea,

        #endregion

        #region Date and Time

        /// <summary>
        /// A calendar date (year, month, day). Corresponds to the PostgreSQL DATE type.
        /// </summary>
        Date,

        /// <summary>
        /// A time of day, without a time zone. Corresponds to the PostgreSQL TIME type.
        /// </summary>
        Time,

        /// <summary>
        /// A time of day, including a time zone. Corresponds to the PostgreSQL TIMETZ type.
        /// </summary>
        TimeTz,

        /// <summary>
        /// A date and time, without a time zone. Corresponds to the PostgreSQL TIMESTAMP type.
        /// </summary>
        Timestamp,

        /// <summary>
        /// A date and time, including a time zone. Corresponds to the PostgreSQL TIMESTAMPTZ type.
        /// </summary>
        TimestampTz,

        /// <summary>
        /// A time span. Corresponds to the PostgreSQL INTERVAL type.
        /// </summary>
        Interval,

        #endregion

        #region Network Address

        /// <summary>
        /// An IPv4 or IPv6 host address, optionally with a subnet. Corresponds to the PostgreSQL INET type.
        /// </summary>
        Inet,

        /// <summary>
        /// An IPv4 or IPv6 network specification. Corresponds to the PostgreSQL CIDR type.
        /// </summary>
        Cidr,

        /// <summary>
        /// A MAC address. Corresponds to the PostgreSQL MACADDR type.
        /// </summary>
        MacAddr,

        /// <summary>
        /// A MAC address in EUI-64 format. Corresponds to the PostgreSQL MACADDR8 type.
        /// </summary>
        MacAddr8,

        #endregion

        #region Bit String

        /// <summary>
        /// A fixed-length bit string. Corresponds to the PostgreSQL BIT type.
        /// </summary>
        Bit,

        /// <summary>
        /// A variable-length bit string. Corresponds to the PostgreSQL VARBIT (BIT VARYING) type.
        /// </summary>
        VarBit,

        #endregion

        #region JSON and XML

        /// <summary>
        /// A textual JSON document. Corresponds to the PostgreSQL JSON type.
        /// </summary>
        Json,

        /// <summary>
        /// A binary JSON document. Corresponds to the PostgreSQL JSONB type.
        /// </summary>
        Jsonb,

        /// <summary>
        /// A JSON path expression. Corresponds to the PostgreSQL JSONPATH type.
        /// </summary>
        JsonPath,

        /// <summary>
        /// An XML document. Corresponds to the PostgreSQL XML type.
        /// </summary>
        Xml,

        #endregion

        #region Geometric

        /// <summary>
        /// A point on a plane. Corresponds to the PostgreSQL POINT type.
        /// </summary>
        Point,

        /// <summary>
        /// An infinite line on a plane. Corresponds to the PostgreSQL LINE type.
        /// </summary>
        Line,

        /// <summary>
        /// A finite line segment on a plane. Corresponds to the PostgreSQL LSEG type.
        /// </summary>
        LSeg,

        /// <summary>
        /// A rectangular box on a plane. Corresponds to the PostgreSQL BOX type.
        /// </summary>
        Box,

        /// <summary>
        /// An open or closed path on a plane. Corresponds to the PostgreSQL PATH type.
        /// </summary>
        Path,

        /// <summary>
        /// A closed polygon on a plane. Corresponds to the PostgreSQL POLYGON type.
        /// </summary>
        Polygon,

        /// <summary>
        /// A circle on a plane. Corresponds to the PostgreSQL CIRCLE type.
        /// </summary>
        Circle,

        #endregion

        #region Range

        /// <summary>
        /// A range of INT4 values. Corresponds to the PostgreSQL INT4RANGE type.
        /// </summary>
        IntegerRange,

        /// <summary>
        /// A range of INT8 values. Corresponds to the PostgreSQL INT8RANGE type.
        /// </summary>
        BigIntRange,

        /// <summary>
        /// A range of NUMERIC values. Corresponds to the PostgreSQL NUMRANGE type.
        /// </summary>
        NumericRange,

        /// <summary>
        /// A range of TIMESTAMP values. Corresponds to the PostgreSQL TSRANGE type.
        /// </summary>
        TimestampRange,

        /// <summary>
        /// A range of TIMESTAMPTZ values. Corresponds to the PostgreSQL TSTZRANGE type.
        /// </summary>
        TimestampTzRange,

        /// <summary>
        /// A range of DATE values. Corresponds to the PostgreSQL DATERANGE type.
        /// </summary>
        DateRange,

        #endregion

        #region Other

        /// <summary>
        /// A universally unique identifier. Corresponds to the PostgreSQL UUID type.
        /// </summary>
        Uuid,

        /// <summary>
        /// An object identifier. Corresponds to the PostgreSQL OID type.
        /// </summary>
        Oid,

        /// <summary>
        /// A set of key/value pairs. Corresponds to the PostgreSQL HSTORE type (requires the <c>hstore</c> extension).
        /// </summary>
        Hstore,

        /// <summary>
        /// A label path in a hierarchical tree-like structure. Corresponds to the PostgreSQL LTREE type (requires the <c>ltree</c> extension).
        /// </summary>
        LTree,

        #endregion

        #region Spatial

        /// <summary>
        /// A planar spatial object. Corresponds to the PostGIS GEOMETRY type (requires the <c>postgis</c> extension).
        /// Reading and writing values of this type requires the Npgsql.NetTopologySuite (or Npgsql.GeoJSON) plugin.
        /// </summary>
        Geometry,

        /// <summary>
        /// A spatial object on the surface of the earth (spheroid). Corresponds to the PostGIS GEOGRAPHY type (requires the <c>postgis</c> extension).
        /// Reading and writing values of this type requires the Npgsql.NetTopologySuite (or Npgsql.GeoJSON) plugin.
        /// </summary>
        Geography,

        #endregion

        #region Text Search

        /// <summary>
        /// A pre-processed document for full-text search. Corresponds to the PostgreSQL TSVECTOR type.
        /// </summary>
        TsVector,

        /// <summary>
        /// A processed full-text search query. Corresponds to the PostgreSQL TSQUERY type.
        /// </summary>
        TsQuery,

        #endregion
    }
}
