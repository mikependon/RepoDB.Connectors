#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Specifies the native EnterpriseDB (PostgreSQL) column type of a <see cref="EDBParameter"/> or result column, in addition to the standard ADO.NET <see cref="System.Data.DbType"/>.
    /// </summary>
    public enum EDBType
    {
        #region Numeric

        /// <summary>
        /// A small-range integer. Corresponds to the PostgreSQL SMALLINT type.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A typical choice for integer. Corresponds to the PostgreSQL INTEGER type.
        /// </summary>
        Integer,

        /// <summary>
        /// A large-range integer. Corresponds to the PostgreSQL BIGINT type.
        /// </summary>
        BigInt,

        /// <summary>
        /// An exact numeric of selectable precision. Corresponds to the PostgreSQL NUMERIC/DECIMAL type.
        /// </summary>
        Decimal,

        /// <summary>
        /// A single-precision, variable-precision, inexact floating-point number. Corresponds to the PostgreSQL REAL type.
        /// </summary>
        Real,

        /// <summary>
        /// A double-precision, variable-precision, inexact floating-point number. Corresponds to the PostgreSQL DOUBLE PRECISION type.
        /// </summary>
        Double,

        /// <summary>
        /// A currency amount. Corresponds to the PostgreSQL MONEY type.
        /// </summary>
        Money,

        /// <summary>
        /// A logical Boolean value. Corresponds to the PostgreSQL BOOLEAN type.
        /// </summary>
        Boolean,

        #endregion

        #region String

        /// <summary>
        /// A fixed-length, blank-padded string. Corresponds to the PostgreSQL CHAR(n) type.
        /// </summary>
        Char,

        /// <summary>
        /// A variable-length string with an optional limit. Corresponds to the PostgreSQL VARCHAR type.
        /// </summary>
        VarChar,

        /// <summary>
        /// A variable, unlimited-length string. Corresponds to the PostgreSQL TEXT type.
        /// </summary>
        Text,

        /// <summary>
        /// An internal type for object names. Corresponds to the PostgreSQL NAME type.
        /// </summary>
        Name,

        /// <summary>
        /// A case-insensitive string. Corresponds to the PostgreSQL CITEXT type.
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
        /// A time of day, including a time zone. Corresponds to the PostgreSQL TIME WITH TIME ZONE type.
        /// </summary>
        TimeTz,

        /// <summary>
        /// A date and time, without a time zone. Corresponds to the PostgreSQL TIMESTAMP type.
        /// </summary>
        Timestamp,

        /// <summary>
        /// A date and time, including a time zone. Corresponds to the PostgreSQL TIMESTAMP WITH TIME ZONE type.
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
        /// A 6-byte MAC address. Corresponds to the PostgreSQL MACADDR type.
        /// </summary>
        MacAddr,

        /// <summary>
        /// An 8-byte MAC address, in EUI-64 format. Corresponds to the PostgreSQL MACADDR8 type.
        /// </summary>
        MacAddr8,

        #endregion

        #region Bit String

        /// <summary>
        /// A fixed-length bit string. Corresponds to the PostgreSQL BIT type.
        /// </summary>
        Bit,

        /// <summary>
        /// A variable-length bit string. Corresponds to the PostgreSQL BIT VARYING type.
        /// </summary>
        VarBit,

        #endregion

        #region JSON

        /// <summary>
        /// A textual JSON document. Corresponds to the PostgreSQL JSON type.
        /// </summary>
        Json,

        /// <summary>
        /// A binary JSON document. Corresponds to the PostgreSQL JSONB type.
        /// </summary>
        Jsonb,

        #endregion

        #region Other

        /// <summary>
        /// A universally unique identifier. Corresponds to the PostgreSQL UUID type.
        /// </summary>
        Uuid,

        /// <summary>
        /// An XML document. Corresponds to the PostgreSQL XML type.
        /// </summary>
        Xml,

        /// <summary>
        /// A set of key/value pairs. Corresponds to the PostgreSQL HSTORE type.
        /// </summary>
        Hstore,

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

        #region Geometric

        /// <summary>
        /// A single point in a plane. Corresponds to the PostgreSQL POINT type.
        /// </summary>
        Point,

        /// <summary>
        /// An infinite line. Corresponds to the PostgreSQL LINE type.
        /// </summary>
        Line,

        /// <summary>
        /// A finite line segment. Corresponds to the PostgreSQL LSEG type.
        /// </summary>
        LSeg,

        /// <summary>
        /// A rectangular box. Corresponds to the PostgreSQL BOX type.
        /// </summary>
        Box,

        /// <summary>
        /// An open or closed geometric path. Corresponds to the PostgreSQL PATH type.
        /// </summary>
        Path,

        /// <summary>
        /// A closed geometric path. Corresponds to the PostgreSQL POLYGON type.
        /// </summary>
        Polygon,

        /// <summary>
        /// A circle defined by a center point and a radius. Corresponds to the PostgreSQL CIRCLE type.
        /// </summary>
        Circle,

        #endregion
    }
}
