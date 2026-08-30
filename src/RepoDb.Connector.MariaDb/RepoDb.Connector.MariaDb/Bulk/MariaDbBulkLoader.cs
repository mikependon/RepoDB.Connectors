#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.MariaDb.Bulk
{
    /// <summary>
    /// Allows importing large amounts of data into a MariaDB database with bulk loading.
    /// </summary>
    public class MariaDbBulkLoader
    {
        private readonly MySqlBulkLoader _inner;
        private MariaDbConnection _connection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkLoader"/> class using the specified <see cref="MariaDbConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="MariaDbConnection"/> that will be used to perform the bulk operation.</param>
        public MariaDbBulkLoader(
            MariaDbConnection connection)
        {
            _inner = new MySqlBulkLoader(connection.InnerConnection);
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public MariaDbConnection Connection
        {
            get => _connection;
            set
            {
                _connection = value;
                _inner.Connection = value?.InnerConnection;
            }
        }

        /// <summary>
        /// Gets or sets the field terminator.
        /// </summary>
        public string FieldTerminator { get => _inner.FieldTerminator; set => _inner.FieldTerminator = value; }

        /// <summary>
        /// Gets or sets the line terminator.
        /// </summary>
        public string LineTerminator { get => _inner.LineTerminator; set => _inner.LineTerminator = value; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string TableName { get => _inner.TableName; set => _inner.TableName = value; }

        /// <summary>
        /// Gets or sets the character set.
        /// </summary>
        public string CharacterSet { get => _inner.CharacterSet; set => _inner.CharacterSet = value; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get => _inner.FileName; set => _inner.FileName = value; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout { get => _inner.Timeout; set => _inner.Timeout = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the file to be loaded is local to the client. The default value is false.
        /// </summary>
        public bool Local { get => _inner.Local; set => _inner.Local = value; }

        /// <summary>
        /// Gets or sets the number of lines to skip.
        /// </summary>
        public int NumberOfLinesToSkip { get => _inner.NumberOfLinesToSkip; set => _inner.NumberOfLinesToSkip = value; }

        /// <summary>
        /// Gets or sets the line prefix.
        /// </summary>
        public string LinePrefix { get => _inner.LinePrefix; set => _inner.LinePrefix = value; }

        /// <summary>
        /// Gets or sets the field quotation character.
        /// </summary>
        public char FieldQuotationCharacter { get => _inner.FieldQuotationCharacter; set => _inner.FieldQuotationCharacter = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the field quotation character is optional.
        /// </summary>
        public bool FieldQuotationOptional { get => _inner.FieldQuotationOptional; set => _inner.FieldQuotationOptional = value; }

        /// <summary>
        /// Gets or sets the escape character.
        /// </summary>
        public char EscapeCharacter { get => _inner.EscapeCharacter; set => _inner.EscapeCharacter = value; }

        /// <summary>
        /// Gets or sets the conflict option.
        /// </summary>
        public MariaDbBulkLoaderConflictOption ConflictOption
        {
            get => (MariaDbBulkLoaderConflictOption)_inner.ConflictOption;
            set => _inner.ConflictOption = (MySqlBulkLoaderConflictOption)value;
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public MariaDbBulkLoaderPriority Priority
        {
            get => (MariaDbBulkLoaderPriority)_inner.Priority;
            set => _inner.Priority = (MySqlBulkLoaderPriority)value;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public List<string> Columns => _inner.Columns;

        /// <summary>
        /// Gets the expressions.
        /// </summary>
        public List<string> Expressions => _inner.Expressions;

        #endregion

        #region Methods

        /// <summary>
        /// Executes the load operation.
        /// </summary>
        /// <returns>The number of rows inserted.</returns>
        public int Load()
        {
            return _inner.Load();
        }

        /// <summary>
        /// Executes the load operation using the given data stream.
        /// </summary>
        /// <param name="stream">The stream containing the data to load.</param>
        /// <returns>The number of rows inserted.</returns>
        public int Load(
            Stream stream)
        {
            return _inner.Load(stream);
        }

        /// <summary>
        /// Asynchronously executes the load operation.
        /// </summary>
        /// <returns>The number of rows inserted.</returns>
        public Task<int> LoadAsync()
        {
            return _inner.LoadAsync();
        }

        /// <summary>
        /// Asynchronously executes the load operation using the given data stream.
        /// </summary>
        /// <param name="stream">The stream containing the data to load.</param>
        /// <returns>The number of rows inserted.</returns>
        public Task<int> LoadAsync(
            Stream stream)
        {
            return _inner.LoadAsync(stream);
        }

        /// <summary>
        /// Asynchronously executes the load operation using the given data stream and cancellation token.
        /// </summary>
        /// <param name="stream">The stream containing the data to load.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of rows inserted.</returns>
        public Task<int> LoadAsync(
            Stream stream,
            CancellationToken cancellationToken)
        {
            return _inner.LoadAsync(stream, cancellationToken);
        }

        #endregion
    }
}
