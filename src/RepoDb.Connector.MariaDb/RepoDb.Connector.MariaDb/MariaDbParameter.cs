#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace RepoDb.Connector.MariaDb
{
    /// <summary>
    /// Represents a parameter to a <see cref="MariaDbCommand"/>.
    /// </summary>
    public class MariaDbParameter : DbParameter
    {
        private readonly MySqlParameter _parameter;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbParameter"/> class.
        /// </summary>
        public MariaDbParameter()
        {
            _parameter = new MySqlParameter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbParameter"/> class from an existing <see cref="MySqlParameter"/>.
        /// </summary>
        /// <param name="parameter">The underlying <see cref="MySqlParameter"/> to wrap.</param>
        internal MariaDbParameter(MySqlParameter parameter)
        {
            _parameter = parameter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="MySqlParameter"/>.
        /// </summary>
        internal MySqlParameter InnerParameter => _parameter;

        /// <summary>
        /// Gets or sets the <see cref="DbType"/> of the parameter.
        /// </summary>
        public override DbType DbType { get => _parameter.DbType; set => _parameter.DbType = value; }

        /// <summary>
        /// Gets or sets the <see cref="MariaDbType"/> of the parameter.
        /// </summary>
        public MariaDbType MariaDbType
        {
            get => MariaDbTypeConverter.ToMariaDbType(_parameter.MySqlDbType);
            set => _parameter.MySqlDbType = MariaDbTypeConverter.ToMySqlDbType(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// </summary>
        public override ParameterDirection Direction { get => _parameter.Direction; set => _parameter.Direction = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter accepts null values.
        /// </summary>
        public override bool IsNullable { get => _parameter.IsNullable; set => _parameter.IsNullable = value; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public override string ParameterName { get => _parameter.ParameterName; set => _parameter.ParameterName = value; }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        public override int Size { get => _parameter.Size; set => _parameter.Size = value; }

        /// <summary>
        /// Gets or sets the name of the source column mapped to the <see cref="DataSet"/>.
        /// </summary>
        public override string SourceColumn { get => _parameter.SourceColumn; set => _parameter.SourceColumn = value; }

        /// <summary>
        /// Gets or sets a value which indicates whether the source column is nullable.
        /// </summary>
        public override bool SourceColumnNullMapping { get => _parameter.SourceColumnNullMapping; set => _parameter.SourceColumnNullMapping = value; }

        /// <summary>
        /// Gets or sets the <see cref="DataRowVersion"/> to use when loading <see cref="Value"/>.
        /// </summary>
        public override DataRowVersion SourceVersion { get => _parameter.SourceVersion; set => _parameter.SourceVersion = value; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public override object Value { get => _parameter.Value; set => _parameter.Value = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the <see cref="DbType"/> property to its original settings.
        /// </summary>
        public override void ResetDbType()
        {
            _parameter.ResetDbType();
        }

        #endregion
    }
}
