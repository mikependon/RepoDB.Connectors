#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.AuroraDb.MySqlConnector
{
    /// <summary>
    /// Specifies the native Aurora MySQL column type of a <see cref="AuroraDbParameter"/> or result column, in addition to the standard ADO.NET <see cref="System.Data.DbType"/>.
    /// </summary>
    public enum AuroraDbType
    {
        #region Numeric

        /// <summary>
        /// A very small integer. Corresponds to the Aurora MySQL TINYINT type.
        /// </summary>
        TinyInt,

        /// <summary>
        /// A small integer. Corresponds to the Aurora MySQL SMALLINT type.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A medium-sized integer. Corresponds to the Aurora MySQL MEDIUMINT type.
        /// </summary>
        MediumInt,

        /// <summary>
        /// A standard integer. Corresponds to the Aurora MySQL INT/INTEGER type.
        /// </summary>
        Int,

        /// <summary>
        /// A large integer. Corresponds to the Aurora MySQL BIGINT type.
        /// </summary>
        BigInt,

        /// <summary>
        /// A fixed-point, exact-value number. Corresponds to the Aurora MySQL DECIMAL/NUMERIC type.
        /// </summary>
        Decimal,

        /// <summary>
        /// A single-precision floating-point number. Corresponds to the Aurora MySQL FLOAT type.
        /// </summary>
        Float,

        /// <summary>
        /// A double-precision floating-point number. Corresponds to the Aurora MySQL DOUBLE/DOUBLE PRECISION/REAL type.
        /// </summary>
        Double,

        /// <summary>
        /// A bit-field value. Corresponds to the Aurora MySQL BIT type.
        /// </summary>
        Bit,

        #endregion

        #region String

        /// <summary>
        /// A fixed-length string. Corresponds to the Aurora MySQL CHAR type.
        /// </summary>
        Char,

        /// <summary>
        /// A variable-length string. Corresponds to the Aurora MySQL VARCHAR type.
        /// </summary>
        VarChar,

        /// <summary>
        /// A very small text value. Corresponds to the Aurora MySQL TINYTEXT type.
        /// </summary>
        TinyText,

        /// <summary>
        /// A text value. Corresponds to the Aurora MySQL TEXT type.
        /// </summary>
        Text,

        /// <summary>
        /// A medium-sized text value. Corresponds to the Aurora MySQL MEDIUMTEXT type.
        /// </summary>
        MediumText,

        /// <summary>
        /// A large text value. Corresponds to the Aurora MySQL LONGTEXT type.
        /// </summary>
        LongText,

        /// <summary>
        /// A string object that can have one value chosen from a list of allowed values. Corresponds to the Aurora MySQL ENUM type.
        /// </summary>
        Enum,

        /// <summary>
        /// A string object that can have zero or more values chosen from a list of allowed values. Corresponds to the Aurora MySQL SET type.
        /// </summary>
        Set,

        #endregion

        #region Binary

        /// <summary>
        /// A fixed-length binary string. Corresponds to the Aurora MySQL BINARY type.
        /// </summary>
        Binary,

        /// <summary>
        /// A variable-length binary string. Corresponds to the Aurora MySQL VARBINARY type.
        /// </summary>
        VarBinary,

        /// <summary>
        /// A very small binary large object. Corresponds to the Aurora MySQL TINYBLOB type.
        /// </summary>
        TinyBlob,

        /// <summary>
        /// A binary large object. Corresponds to the Aurora MySQL BLOB type.
        /// </summary>
        Blob,

        /// <summary>
        /// A medium-sized binary large object. Corresponds to the Aurora MySQL MEDIUMBLOB type.
        /// </summary>
        MediumBlob,

        /// <summary>
        /// A large binary large object. Corresponds to the Aurora MySQL LONGBLOB type.
        /// </summary>
        LongBlob,

        #endregion

        #region Date and Time

        /// <summary>
        /// A date value. Corresponds to the Aurora MySQL DATE type.
        /// </summary>
        Date,

        /// <summary>
        /// A time value. Corresponds to the Aurora MySQL TIME type.
        /// </summary>
        Time,

        /// <summary>
        /// A date and time value. Corresponds to the Aurora MySQL DATETIME type.
        /// </summary>
        DateTime,

        /// <summary>
        /// A date and time value that is automatically updated. Corresponds to the Aurora MySQL TIMESTAMP type.
        /// </summary>
        Timestamp,

        /// <summary>
        /// A year value. Corresponds to the Aurora MySQL YEAR type.
        /// </summary>
        Year,

        #endregion

        #region JSON

        /// <summary>
        /// A JSON document. Corresponds to the Aurora MySQL JSON type.
        /// </summary>
        Json,

        #endregion

        #region Spatial

        /// <summary>
        /// A generic spatial value. Corresponds to the Aurora MySQL GEOMETRY type.
        /// </summary>
        Geometry,

        /// <summary>
        /// A single location in coordinate space. Corresponds to the Aurora MySQL POINT type.
        /// </summary>
        Point,

        /// <summary>
        /// A curve made of a sequence of connected points. Corresponds to the Aurora MySQL LINESTRING type.
        /// </summary>
        LineString,

        /// <summary>
        /// A planar surface defined by a boundary of one or more linear rings. Corresponds to the Aurora MySQL POLYGON type.
        /// </summary>
        Polygon,

        /// <summary>
        /// A collection of points. Corresponds to the Aurora MySQL MULTIPOINT type.
        /// </summary>
        MultiPoint,

        /// <summary>
        /// A collection of line strings. Corresponds to the Aurora MySQL MULTILINESTRING type.
        /// </summary>
        MultiLineString,

        /// <summary>
        /// A collection of polygons. Corresponds to the Aurora MySQL MULTIPOLYGON type.
        /// </summary>
        MultiPolygon,

        /// <summary>
        /// A collection of geometry values of any type. Corresponds to the Aurora MySQL GEOMETRYCOLLECTION type.
        /// </summary>
        GeometryCollection,

        #endregion
    }
}
