#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDbConnector
{
    /// <summary>
    /// Specifies the native MariaDB column type of a <see cref="MariaDbParameter"/> or result column, in addition to the standard ADO.NET <see cref="System.Data.DbType"/>.
    /// </summary>
    public enum MariaDbType
    {
        #region Numeric

        /// <summary>
        /// A very small integer. Corresponds to the MariaDB TINYINT type.
        /// </summary>
        TinyInt,

        /// <summary>
        /// A small integer. Corresponds to the MariaDB SMALLINT type.
        /// </summary>
        SmallInt,

        /// <summary>
        /// A medium-sized integer. Corresponds to the MariaDB MEDIUMINT type.
        /// </summary>
        MediumInt,

        /// <summary>
        /// A standard integer. Corresponds to the MariaDB INT/INTEGER type.
        /// </summary>
        Int,

        /// <summary>
        /// A large integer. Corresponds to the MariaDB BIGINT type.
        /// </summary>
        BigInt,

        /// <summary>
        /// A fixed-point, exact-value number. Corresponds to the MariaDB DECIMAL/NUMERIC type.
        /// </summary>
        Decimal,

        /// <summary>
        /// A single-precision floating-point number. Corresponds to the MariaDB FLOAT type.
        /// </summary>
        Float,

        /// <summary>
        /// A double-precision floating-point number. Corresponds to the MariaDB DOUBLE/DOUBLE PRECISION/REAL type.
        /// </summary>
        Double,

        /// <summary>
        /// A bit-field value. Corresponds to the MariaDB BIT type.
        /// </summary>
        Bit,

        #endregion

        #region String

        /// <summary>
        /// A fixed-length string. Corresponds to the MariaDB CHAR type.
        /// </summary>
        Char,

        /// <summary>
        /// A variable-length string. Corresponds to the MariaDB VARCHAR type.
        /// </summary>
        VarChar,

        /// <summary>
        /// A very small text value. Corresponds to the MariaDB TINYTEXT type.
        /// </summary>
        TinyText,

        /// <summary>
        /// A text value. Corresponds to the MariaDB TEXT type.
        /// </summary>
        Text,

        /// <summary>
        /// A medium-sized text value. Corresponds to the MariaDB MEDIUMTEXT type.
        /// </summary>
        MediumText,

        /// <summary>
        /// A large text value. Corresponds to the MariaDB LONGTEXT type.
        /// </summary>
        LongText,

        /// <summary>
        /// A string object that can have one value chosen from a list of allowed values. Corresponds to the MariaDB ENUM type.
        /// </summary>
        Enum,

        /// <summary>
        /// A string object that can have zero or more values chosen from a list of allowed values. Corresponds to the MariaDB SET type.
        /// </summary>
        Set,

        #endregion

        #region Binary

        /// <summary>
        /// A fixed-length binary string. Corresponds to the MariaDB BINARY type.
        /// </summary>
        Binary,

        /// <summary>
        /// A variable-length binary string. Corresponds to the MariaDB VARBINARY type.
        /// </summary>
        VarBinary,

        /// <summary>
        /// A very small binary large object. Corresponds to the MariaDB TINYBLOB type.
        /// </summary>
        TinyBlob,

        /// <summary>
        /// A binary large object. Corresponds to the MariaDB BLOB type.
        /// </summary>
        Blob,

        /// <summary>
        /// A medium-sized binary large object. Corresponds to the MariaDB MEDIUMBLOB type.
        /// </summary>
        MediumBlob,

        /// <summary>
        /// A large binary large object. Corresponds to the MariaDB LONGBLOB type.
        /// </summary>
        LongBlob,

        #endregion

        #region Date and Time

        /// <summary>
        /// A date value. Corresponds to the MariaDB DATE type.
        /// </summary>
        Date,

        /// <summary>
        /// A time value. Corresponds to the MariaDB TIME type.
        /// </summary>
        Time,

        /// <summary>
        /// A date and time value. Corresponds to the MariaDB DATETIME type.
        /// </summary>
        DateTime,

        /// <summary>
        /// A date and time value that is automatically updated. Corresponds to the MariaDB TIMESTAMP type.
        /// </summary>
        Timestamp,

        /// <summary>
        /// A year value. Corresponds to the MariaDB YEAR type.
        /// </summary>
        Year,

        #endregion

        #region JSON

        /// <summary>
        /// A JSON document. Corresponds to the MariaDB JSON type.
        /// </summary>
        Json,

        #endregion

        #region Spatial

        /// <summary>
        /// A generic spatial value. Corresponds to the MariaDB GEOMETRY type.
        /// </summary>
        Geometry,

        /// <summary>
        /// A single location in coordinate space. Corresponds to the MariaDB POINT type.
        /// </summary>
        Point,

        /// <summary>
        /// A curve made of a sequence of connected points. Corresponds to the MariaDB LINESTRING type.
        /// </summary>
        LineString,

        /// <summary>
        /// A planar surface defined by a boundary of one or more linear rings. Corresponds to the MariaDB POLYGON type.
        /// </summary>
        Polygon,

        /// <summary>
        /// A collection of points. Corresponds to the MariaDB MULTIPOINT type.
        /// </summary>
        MultiPoint,

        /// <summary>
        /// A collection of line strings. Corresponds to the MariaDB MULTILINESTRING type.
        /// </summary>
        MultiLineString,

        /// <summary>
        /// A collection of polygons. Corresponds to the MariaDB MULTIPOLYGON type.
        /// </summary>
        MultiPolygon,

        /// <summary>
        /// A collection of geometry values of any type. Corresponds to the MariaDB GEOMETRYCOLLECTION type.
        /// </summary>
        GeometryCollection,

        #endregion
    }
}
