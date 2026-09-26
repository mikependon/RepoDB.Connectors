#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using AwsWrapperDataProvider.Driver.Exceptions;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.AuroraDb.Npgsql
{
    /// <summary>
    /// Represents a SQL statement to execute against a AuroraDB database.
    /// </summary>
    public class AuroraDbCommand : DbCommand
    {
        private readonly AwsWrapperCommand _command;
        private readonly AuroraDbParameterCollection _parameters;
        private AuroraDbConnection _connection;
        private AuroraDbTransaction _transaction;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbCommand"/> class.
        /// </summary>
        public AuroraDbCommand()
        {
            _command = new AwsWrapperCommand<NpgsqlCommand>();
            _parameters = new AuroraDbParameterCollection((NpgsqlParameterCollection)_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbCommand"/> class with the text of the query.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        public AuroraDbCommand(
            string commandText)
        {
            _command = new AwsWrapperCommand<NpgsqlCommand>(commandText);
            _parameters = new AuroraDbParameterCollection((NpgsqlParameterCollection)_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbCommand"/> class with the text of the query and a <see cref="AuroraDbConnection"/>.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        /// <param name="connection">A <see cref="AuroraDbConnection"/> that represents the connection to a AuroraDB server.</param>
        public AuroraDbCommand(
            string commandText,
            AuroraDbConnection connection)
        {
            _command = new AwsWrapperCommand<NpgsqlCommand>(commandText, connection.InnerConnection);
            _parameters = new AuroraDbParameterCollection((NpgsqlParameterCollection)_command.Parameters);
            _connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbCommand"/> class from an existing <see cref="AwsWrapperCommand"/>.
        /// </summary>
        /// <param name="command">The underlying <see cref="AwsWrapperCommand"/> to wrap.</param>
        /// <param name="connection">A <see cref="AuroraDbConnection"/> that represents the connection to a AuroraDB server.</param>
        internal AuroraDbCommand(
            AwsWrapperCommand command,
            AuroraDbConnection connection)
        {
            _command = command;
            _parameters = new AuroraDbParameterCollection((NpgsqlParameterCollection)_command.Parameters);
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="AuroraDbParameterCollection"/>.
        /// </summary>
        public new AuroraDbParameterCollection Parameters => _parameters;

        /// <summary>
        /// Gets or sets the SQL statement to execute at the data source.
        /// </summary>
        public override string CommandText { get => _command.CommandText; set => _command.CommandText = value; }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public override int CommandTimeout { get => _command.CommandTimeout; set => _command.CommandTimeout = value; }

        /// <summary>
        /// Gets or sets a value indicating how the <see cref="CommandText"/> property is to be interpreted.
        /// </summary>
        public override CommandType CommandType { get => _command.CommandType; set => _command.CommandType = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a design component.
        /// </summary>
        public override bool DesignTimeVisible { get => _command.DesignTimeVisible; set => _command.DesignTimeVisible = value; }

        /// <summary>
        /// Gets or sets how command results are applied to the row being updated.
        /// </summary>
        public override UpdateRowSource UpdatedRowSource { get => _command.UpdatedRowSource; set => _command.UpdatedRowSource = value; }

        /// <summary>
        /// Gets or sets the <see cref="AuroraDbConnection"/> used by this command.
        /// </summary>
        protected override DbConnection DbConnection
        {
            get => _connection;
            set
            {
                _connection = (AuroraDbConnection)value;
                _command.Connection = _connection?.InnerConnection;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="AuroraDbParameter"/> objects.
        /// </summary>
        protected override DbParameterCollection DbParameterCollection => _parameters;

        /// <summary>
        /// Gets or sets the <see cref="AuroraDbTransaction"/> within which this command executes.
        /// </summary>
        protected override DbTransaction DbTransaction
        {
            get => _transaction;
            set
            {
                _transaction = (AuroraDbTransaction)value;
                _command.Transaction = _transaction?.InnerTransaction;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to cancel the execution of a currently active command.
        /// </summary>
        public override void Cancel()
        {
            try
            {
                _command.Cancel();
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the instance of <see cref="AuroraDbDataReader"/> object.
        /// </summary>
        /// <returns>The instance of <see cref="AuroraDbDataReader"/>.</returns>
        public new AuroraDbDataReader ExecuteReader()
        {
            EnsureConnection();

            try
            {
                return new AuroraDbDataReader(_command.ExecuteReader());
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes a SQL statement against the connection and returns the instance of <see cref="AuroraDbDataReader"/> object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the instance of <see cref="AuroraDbDataReader"/>.</returns>
        public new async Task<AuroraDbDataReader> ExecuteReaderAsync(
            CancellationToken cancellationToken = default)
        {
            EnsureConnection();

            try
            {
                return new AuroraDbDataReader(await _command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken).ConfigureAwait(false));
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public override int ExecuteNonQuery()
        {
            EnsureConnection();

            try
            {
                return _command.ExecuteNonQuery();
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes a SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the number of rows affected.</returns>
        public override async Task<int> ExecuteNonQueryAsync(
            CancellationToken cancellationToken)
        {
            EnsureConnection();

            try
            {
                return await _command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        public override object ExecuteScalar()
        {
            EnsureConnection();

            try
            {
                return _command.ExecuteScalar();
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes the query, and returns the first column of the first row in the result set. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the first column of the first row in the result set.</returns>
        public override async Task<object> ExecuteScalarAsync(
            CancellationToken cancellationToken)
        {
            EnsureConnection();

            try
            {
                return await _command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a prepared version of the command on an instance of AuroraDB server.
        /// </summary>
        public override void Prepare()
        {
            EnsureConnection();

            try
            {
                _command.Prepare();
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="AuroraDbParameter"/> object.
        /// </summary>
        /// <returns>A <see cref="AuroraDbParameter"/> object.</returns>
        protected override DbParameter CreateDbParameter()
        {
            try
            {
                return new AuroraDbParameter((NpgsqlParameter)_command.CreateParameter());
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the <see cref="CommandText"/> to the connection and builds a <see cref="AuroraDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="AuroraDbDataReader"/> object.</returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            EnsureConnection();

            try
            {
                return new AuroraDbDataReader(_command.ExecuteReader(behavior));
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously sends the <see cref="CommandText"/> to the connection and builds a <see cref="AuroraDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="AuroraDbDataReader"/> object.</returns>
        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(
            CommandBehavior behavior,
            CancellationToken cancellationToken)
        {
            EnsureConnection();

            try
            {
                return new AuroraDbDataReader(await _command.ExecuteReaderAsync(behavior, cancellationToken).ConfigureAwait(false));
            }
            catch (DbException exception) when (exception is NpgsqlException || exception is AwsWrapperDbException)
            {
                throw new AuroraDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="AuroraDbCommand"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release managed resources; otherwise, false.</param>
        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _command.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Ensures that a <see cref="AuroraDbConnection"/> has been assigned to this command before it gets executed.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="DbConnection"/> property has not been initialized or the connection is not open.</exception>
        private void EnsureConnection()
        {
            if (_connection is null)
            {
                throw new InvalidOperationException("The Connection property has not been initialized.");
            }

            _connection.EnsureOpen();
        }

        #endregion
    }
}
