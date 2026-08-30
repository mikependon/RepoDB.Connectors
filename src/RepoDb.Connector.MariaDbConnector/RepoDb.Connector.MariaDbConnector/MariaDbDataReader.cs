#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.MariaDbConnector
{
    /// <summary>
    /// Provides a means of reading a forward-only stream of rows from a MariaDB database.
    /// </summary>
    public class MariaDbDataReader : DbDataReader
    {
        private readonly MySqlDataReader _reader;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbDataReader"/> class.
        /// </summary>
        /// <param name="reader">The underlying <see cref="MySqlDataReader"/> to wrap.</param>
        internal MariaDbDataReader(MySqlDataReader reader)
        {
            _reader = reader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        public override int Depth => _reader.Depth;

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        public override int FieldCount => _reader.FieldCount;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MariaDbDataReader"/> contains one or more rows.
        /// </summary>
        public override bool HasRows => _reader.HasRows;

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        public override bool IsClosed => _reader.IsClosed;

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public override int RecordsAffected => _reader.RecordsAffected;

        /// <summary>
        /// Gets the number of fields in the <see cref="MariaDbDataReader"/> that are not hidden.
        /// </summary>
        public override int VisibleFieldCount => _reader.VisibleFieldCount;

        /// <summary>
        /// Gets the value of the specified column in its native format given the column ordinal.
        /// </summary>
        public override object this[int ordinal] => _reader[ordinal];

        /// <summary>
        /// Gets the value of the specified column in its native format given the column name.
        /// </summary>
        public override object this[string name] => _reader[name];

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        public override bool GetBoolean(int ordinal) => _reader.GetBoolean(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        public override byte GetByte(int ordinal) => _reader.GetByte(ordinal);

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array starting at the given buffer offset.
        /// </summary>
        public override long GetBytes(
            int ordinal,
            long dataOffset,
            byte[] buffer,
            int bufferOffset,
            int length) =>
            _reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

        /// <summary>
        /// Gets the value of the specified column as a single character.
        /// </summary>
        public override char GetChar(int ordinal) => _reader.GetChar(ordinal);

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array starting at the given buffer offset.
        /// </summary>
        public override long GetChars(
            int ordinal,
            long dataOffset,
            char[] buffer,
            int bufferOffset,
            int length) =>
            _reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

        /// <summary>
        /// Gets the name of the source data type.
        /// </summary>
        public override string GetDataTypeName(int ordinal) => _reader.GetDataTypeName(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/> object.
        /// </summary>
        public override DateTime GetDateTime(int ordinal) => _reader.GetDateTime(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a <see cref="decimal"/> object.
        /// </summary>
        public override decimal GetDecimal(int ordinal) => _reader.GetDecimal(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a double-precision floating point number.
        /// </summary>
        public override double GetDouble(int ordinal) => _reader.GetDouble(ordinal);

        /// <summary>
        /// Gets the <see cref="Type"/> that is the data type of the object.
        /// </summary>
        public override Type GetFieldType(int ordinal) => _reader.GetFieldType(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a single-precision floating point number.
        /// </summary>
        public override float GetFloat(int ordinal) => _reader.GetFloat(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a globally-unique identifier (GUID).
        /// </summary>
        public override Guid GetGuid(int ordinal) => _reader.GetGuid(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a 16-bit signed integer.
        /// </summary>
        public override short GetInt16(int ordinal) => _reader.GetInt16(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a 32-bit signed integer.
        /// </summary>
        public override int GetInt32(int ordinal) => _reader.GetInt32(ordinal);

        /// <summary>
        /// Gets the value of the specified column as a 64-bit signed integer.
        /// </summary>
        public override long GetInt64(int ordinal) => _reader.GetInt64(ordinal);

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        public override string GetName(int ordinal) => _reader.GetName(ordinal);

        /// <summary>
        /// Gets the column ordinal, given the name of the column.
        /// </summary>
        public override int GetOrdinal(string name) => _reader.GetOrdinal(name);

        /// <summary>
        /// Returns a <see cref="DataTable"/> that describes the column metadata of the <see cref="MariaDbDataReader"/>.
        /// </summary>
        public override DataTable GetSchemaTable() => _reader.GetSchemaTable();

        /// <summary>
        /// Gets the value of the specified column as a <see cref="string"/> object.
        /// </summary>
        public override string GetString(int ordinal) => _reader.GetString(ordinal);

        /// <summary>
        /// Gets the value of the specified column in its native format.
        /// </summary>
        public override object GetValue(int ordinal) => _reader.GetValue(ordinal);

        /// <summary>
        /// Gets all the attribute columns in the collection for the current row.
        /// </summary>
        public override int GetValues(object[] values) => _reader.GetValues(values);

        /// <summary>
        /// Gets a value indicating whether the column contains non-existent or missing values.
        /// </summary>
        public override bool IsDBNull(int ordinal) => _reader.IsDBNull(ordinal);

        /// <summary>
        /// Asynchronously gets a value indicating whether the column contains non-existent or missing values.
        /// </summary>
        public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken) =>
            _reader.IsDBNullAsync(ordinal, cancellationToken);

        /// <summary>
        /// Advances the data reader to the next result when reading the results of batch SQL statements.
        /// </summary>
        public override bool NextResult() => _reader.NextResult();

        /// <summary>
        /// Asynchronously advances the data reader to the next result when reading the results of batch SQL statements.
        /// </summary>
        public override Task<bool> NextResultAsync(CancellationToken cancellationToken) =>
            _reader.NextResultAsync(cancellationToken);

        /// <summary>
        /// Advances the <see cref="MariaDbDataReader"/> to the next record.
        /// </summary>
        public override bool Read() => _reader.Read();

        /// <summary>
        /// Asynchronously advances the <see cref="MariaDbDataReader"/> to the next record.
        /// </summary>
        public override Task<bool> ReadAsync(CancellationToken cancellationToken) =>
            _reader.ReadAsync(cancellationToken);

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MariaDbDataReader"/>.
        /// </summary>
        public override IEnumerator GetEnumerator() => _reader.GetEnumerator();

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbDataReader"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release managed resources; otherwise, false.</param>
        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _reader.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
