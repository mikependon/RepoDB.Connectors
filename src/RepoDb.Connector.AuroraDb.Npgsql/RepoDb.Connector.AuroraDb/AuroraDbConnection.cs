#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using AwsWrapperDataProvider.Dialect.Npgsql;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.AuroraDb
{
    /// <summary>
    /// Represents a connection to an Amazon Aurora PostgreSQL database.
    /// The connection is backed by the AWS Advanced .NET Data Provider Wrapper (<see cref="AwsWrapperConnection{TConnection}"/>),
    /// which layers the AWS capabilities (failover, enhanced failure monitoring, read/write splitting, IAM authentication,
    /// Secrets Manager, ...) on top of the underlying <see cref="NpgsqlConnection"/>.
    /// </summary>
    public class AuroraDbConnection : DbConnection
    {
        private readonly AwsWrapperConnection<NpgsqlConnection> _connection;

        #region Constructors

        /// <summary>
        /// Registers the AWS Npgsql dialect, required by the AWS wrapper before Npgsql can be used with it.
        /// </summary>
        static AuroraDbConnection()
        {
            NpgsqlDialectLoader.Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbConnection"/> class.
        /// </summary>
        public AuroraDbConnection()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbConnection"/> class when given a string containing the connection string.
        /// The connection string can hold both the Npgsql and the AWS wrapper (e.g. <c>Plugins</c>) connection properties.
        /// </summary>
        /// <param name="connectionString">The connection used to open the database.</param>
        public AuroraDbConnection(string connectionString)
        {
            _connection = new AwsWrapperConnection<NpgsqlConnection>(connectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="AwsWrapperConnection{TConnection}"/>.
        /// </summary>
        internal AwsWrapperConnection<NpgsqlConnection> InnerConnection => _connection;

        /// <summary>
        /// Gets the underlying <see cref="AwsWrapperConnection"/>, for advanced scenarios that require the AWS wrapper functionality
        /// which cannot be represented by the standard ADO.NET surface (e.g. <see cref="IWrapper.Unwrap{T}"/>).
        /// </summary>
        public AwsWrapperConnection WrappedConnection => _connection;

        /// <summary>
        /// Gets or sets the string used to connect to an Aurora database.
        /// </summary>
        public override string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        public override int ConnectionTimeout => _connection.ConnectionTimeout;

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        public override string Database => _connection.Database;

        /// <summary>
        /// Gets the name of the Aurora server to which to connect.
        /// </summary>
        public override string DataSource => _connection.DataSource;

        /// <summary>
        /// Gets a string containing the version of the Aurora server to which the client is connected.
        /// </summary>
        public override string ServerVersion => _connection.ServerVersion;

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        public override ConnectionState State => _connection.State;

        #endregion

        #region Methods

        /// <summary>
        /// Changes the current database for an open <see cref="AuroraDbConnection"/>.
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
        /// Asynchronously closes the connection to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task CloseAsync()
        {
            return _connection.CloseAsync();
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
            // The AWS wrapper does not honor an already-cancelled token before attempting (and retrying) the connection.
            cancellationToken.ThrowIfCancellationRequested();

            return _connection.OpenAsync(cancellationToken);
        }

        /// <summary>
        /// Creates and returns a <see cref="AuroraDbCommand"/> associated with this connection.
        /// </summary>
        /// <returns>A <see cref="AuroraDbCommand"/> object.</returns>
        public new AuroraDbCommand CreateCommand()
        {
            return (AuroraDbCommand)CreateDbCommand();
        }

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level under which the transaction should run.</param>
        /// <returns>A <see cref="AuroraDbTransaction"/> representing the new transaction.</returns>
        protected override DbTransaction BeginDbTransaction(
            IsolationLevel isolationLevel)
        {
            EnsureOpen();
            return new AuroraDbTransaction((AwsWrapperTransaction)_connection.BeginTransaction(isolationLevel), this);
        }

        /// <summary>
        /// Creates and returns a <see cref="AuroraDbCommand"/> associated with this connection.
        /// </summary>
        /// <returns>A <see cref="AuroraDbCommand"/> object.</returns>
        protected override DbCommand CreateDbCommand()
        {
            return new AuroraDbCommand((AwsWrapperCommand)_connection.CreateCommand(), this);
        }

        /// <summary>
        /// Releases the resources used by the <see cref="AuroraDbConnection"/> and optionally releases the managed resources.
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

        #region Helpers

        /// <summary>
        /// Ensures that the connection is open, so that the AWS wrapper does not attempt (and report a failover for) an unopened connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        internal void EnsureOpen()
        {
            if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            {
                throw new InvalidOperationException("Connection is not open.");
            }
        }

        #endregion
    }
}
