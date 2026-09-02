#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Represents a connection to an EnterpriseDB (PostgreSQL) database.
    /// </summary>
    public class EDBConnection : DbConnection
    {
        private readonly NpgsqlConnection _connection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBConnection"/> class.
        /// </summary>
        public EDBConnection()
        {
            _connection = new NpgsqlConnection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBConnection"/> class when given a string containing the connection string.
        /// </summary>
        /// <param name="connectionString">The connection used to open the database.</param>
        public EDBConnection(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="NpgsqlConnection"/>.
        /// </summary>
        internal NpgsqlConnection InnerConnection => _connection;

        /// <summary>
        /// Gets or sets the string used to connect to an EnterpriseDB database.
        /// </summary>
        public override string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        public override string Database => _connection.Database;

        /// <summary>
        /// Gets the name of the EnterpriseDB server to which to connect.
        /// </summary>
        public override string DataSource => _connection.DataSource;

        /// <summary>
        /// Gets a string containing the version of the EnterpriseDB server to which the client is connected.
        /// </summary>
        public override string ServerVersion => _connection.ServerVersion;

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        public override ConnectionState State => _connection.State;

        #endregion

        #region Methods

        /// <summary>
        /// Changes the current database for an open <see cref="EDBConnection"/>.
        /// </summary>
        /// <param name="databaseName">The name of the database to use.</param>
        public override void ChangeDatabase(
            string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public override void Close()
        {
            _connection.Close();
        }

        /// <summary>
        /// Opens a database connection with the property settings specified by the <see cref="ConnectionString"/>.
        /// </summary>
        public override void Open()
        {
            _connection.Open();
        }

        /// <summary>
        /// Asynchronously opens a database connection with the property settings specified by the <see cref="ConnectionString"/>.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task OpenAsync(
            CancellationToken cancellationToken)
        {
            return _connection.OpenAsync(cancellationToken);
        }

        /// <summary>
        /// Creates and returns a <see cref="EDBCommand"/> associated with this connection.
        /// </summary>
        /// <returns>A <see cref="EDBCommand"/> object.</returns>
        public new EDBCommand CreateCommand()
        {
            return (EDBCommand)CreateDbCommand();
        }

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level under which the transaction should run.</param>
        /// <returns>A <see cref="EDBTransaction"/> representing the new transaction.</returns>
        protected override DbTransaction BeginDbTransaction(
            IsolationLevel isolationLevel)
        {
            return new EDBTransaction((NpgsqlTransaction)_connection.BeginTransaction(isolationLevel), this);
        }

        /// <summary>
        /// Creates and returns a <see cref="EDBCommand"/> associated with this connection.
        /// </summary>
        /// <returns>A <see cref="EDBCommand"/> object.</returns>
        protected override DbCommand CreateDbCommand()
        {
            return new EDBCommand((NpgsqlCommand)_connection.CreateCommand(), this);
        }

        /// <summary>
        /// Releases the resources used by the <see cref="EDBConnection"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release managed resources; otherwise, false.</param>
        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _connection.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
