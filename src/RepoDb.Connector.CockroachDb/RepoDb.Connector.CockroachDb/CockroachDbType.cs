#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.CockroachDb
{
    /// <summary>
    /// Specifies the native CockroachDB column type of a <see cref="CockroachDbParameter"/> or result column, in addition to the standard ADO.NET <see cref="System.Data.DbType"/>.
    /// Only the types that CockroachDB actually supports are listed - PostgreSQL types such as MONEY, CIDR, MACADDR, XML, HSTORE,
    /// the geometric types (POINT, LINE, BOX, ...) and the range types are intentionally absent, since CockroachDB does not implement them.
    /// </summary>
    public enum CockroachDbType
    {
        #region Numeric

        /// <summary>
        /// A 2-byte signed integer. Corresponds to the CockroachDB INT2 (SMALLINT) type.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A 4-byte signed integer. Corresponds to the CockroachDB INT4 type.
        /// Note that, unlike PostgreSQL, a plain INT/INTEGER column in CockroachDB is an INT8 (<see cref="BigInt"/>) by default.
        /// </summary>
        Integer,

        /// <summary>
        /// An 8-byte signed integer. Corresponds to the CockroachDB INT8 (INT, INTEGER, BIGINT) type.
        /// </summary>
        BigInt,

        /// <summary>
        /// An exact numeric of selectable precision. Corresponds to the CockroachDB DECIMAL (NUMERIC) type.
        /// </summary>
        Decimal,

        /// <summary>
        /// A single-precision, inexact floating-point number. Corresponds to the CockroachDB FLOAT4 (REAL) type.
        /// </summary>
        Real,

        /// <summary>
        /// A double-precision, inexact floating-point number. Corresponds to the CockroachDB FLOAT8 (FLOAT, DOUBLE PRECISION) type.
        /// </summary>
        Double,

        /// <summary>
        /// A logical Boolean value. Corresponds to the CockroachDB BOOL (BOOLEAN) type.
        /// </summary>
        Boolean,

        #endregion

        #region String

        /// <summary>
        /// A fixed-length string. Corresponds to the CockroachDB CHAR(n) type.
        /// </summary>
        Char,

        /// <summary>
        /// A variable-length string with an optional limit. Corresponds to the CockroachDB VARCHAR(n) type.
        /// </summary>
        VarChar,

        /// <summary>
        /// A variable, unlimited-length string. Corresponds to the CockroachDB STRING (TEXT) type.
        /// </summary>
        Text,

        /// <summary>
        /// An internal type for object names, as returned by the <c>pg_catalog</c> tables. Corresponds to the CockroachDB NAME type.
        /// </summary>
        Name,

        /// <summary>
        /// A case-insensitive string. Corresponds to the CockroachDB CITEXT type.
        /// </summary>
        Citext,

        #endregion

        #region Binary

        /// <summary>
        /// A variable-length binary string. Corresponds to the CockroachDB BYTES (BYTEA) type.
        /// </summary>
        Bytea,

        #endregion

        #region Date and Time

        /// <summary>
        /// A calendar date (year, month, day). Corresponds to the CockroachDB DATE type.
        /// </summary>
        Date,

        /// <summary>
        /// A time of day, without a time zone. Corresponds to the CockroachDB TIME type.
        /// </summary>
        Time,

        /// <summary>
        /// A time of day, including a time zone. Corresponds to the CockroachDB TIMETZ type.
        /// </summary>
        TimeTz,

        /// <summary>
        /// A date and time, without a time zone. Corresponds to the CockroachDB TIMESTAMP type.
        /// </summary>
        Timestamp,

        /// <summary>
        /// A date and time, including a time zone. Corresponds to the CockroachDB TIMESTAMPTZ type.
        /// </summary>
        TimestampTz,

        /// <summary>
        /// A time span. Corresponds to the CockroachDB INTERVAL type.
        /// </summary>
        Interval,

        #endregion

        #region Network Address

        /// <summary>
        /// An IPv4 or IPv6 host address, optionally with a subnet. Corresponds to the CockroachDB INET type.
        /// </summary>
        Inet,

        #endregion

        #region Bit String

        /// <summary>
        /// A fixed-length bit string. Corresponds to the CockroachDB BIT type.
        /// </summary>
        Bit,

        /// <summary>
        /// A variable-length bit string. Corresponds to the CockroachDB VARBIT (BIT VARYING) type.
        /// </summary>
        VarBit,

        #endregion

        #region JSON

        /// <summary>
        /// A binary JSON document. Corresponds to the CockroachDB JSONB type (JSON is an alias of JSONB in CockroachDB).
        /// </summary>
        Jsonb,

        #endregion

        #region Other

        /// <summary>
        /// A universally unique identifier. Corresponds to the CockroachDB UUID type.
        /// </summary>
        Uuid,

        /// <summary>
        /// An object identifier. Corresponds to the CockroachDB OID type.
        /// </summary>
        Oid,

        /// <summary>
        /// A label path in a hierarchical tree-like structure. Corresponds to the CockroachDB LTREE type.
        /// </summary>
        LTree,

        #endregion

        #region Text Search

        /// <summary>
        /// A pre-processed document for full-text search. Corresponds to the CockroachDB TSVECTOR type.
        /// </summary>
        TsVector,

        /// <summary>
        /// A processed full-text search query. Corresponds to the CockroachDB TSQUERY type.
        /// </summary>
        TsQuery,

        #endregion

        #region Spatial

        /// <summary>
        /// A planar spatial object (point, line string, polygon, ...). Corresponds to the CockroachDB GEOMETRY type.
        /// Reading and writing values of this type requires the Npgsql.NetTopologySuite (or Npgsql.GeoJSON) plugin.
        /// </summary>
        Geometry,

        /// <summary>
        /// A spatial object on the surface of the earth (spheroid). Corresponds to the CockroachDB GEOGRAPHY type.
        /// Reading and writing values of this type requires the Npgsql.NetTopologySuite (or Npgsql.GeoJSON) plugin.
        /// </summary>
        Geography,

        #endregion
    }
}
