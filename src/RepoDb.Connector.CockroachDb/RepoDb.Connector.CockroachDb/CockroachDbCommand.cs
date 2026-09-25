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

namespace RepoDb.Connector.CockroachDb
{
    /// <summary>
    /// Represents a SQL statement to execute against a CockroachDB database.
    /// </summary>
    public class CockroachDbCommand : DbCommand
    {
        private readonly NpgsqlCommand _command;
        private readonly CockroachDbParameterCollection _parameters;
        private CockroachDbConnection _connection;
        private CockroachDbTransaction _transaction;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbCommand"/> class.
        /// </summary>
        public CockroachDbCommand()
        {
            _command = new NpgsqlCommand();
            _parameters = new CockroachDbParameterCollection(_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbCommand"/> class with the text of the query.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        public CockroachDbCommand(
            string commandText)
        {
            _command = new NpgsqlCommand(commandText);
            _parameters = new CockroachDbParameterCollection(_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbCommand"/> class with the text of the query and a <see cref="CockroachDbConnection"/>.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        /// <param name="connection">A <see cref="CockroachDbConnection"/> that represents the connection to a CockroachDB server.</param>
        public CockroachDbCommand(
            string commandText,
            CockroachDbConnection connection)
        {
            _command = new NpgsqlCommand(commandText, connection.InnerConnection);
            _parameters = new CockroachDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbCommand"/> class from an existing <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="command">The underlying <see cref="NpgsqlCommand"/> to wrap.</param>
        /// <param name="connection">A <see cref="CockroachDbConnection"/> that represents the connection to a CockroachDB server.</param>
        internal CockroachDbCommand(
            NpgsqlCommand command,
            CockroachDbConnection connection)
        {
            _command = command;
            _parameters = new CockroachDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="CockroachDbParameterCollection"/>.
        /// </summary>
        public new CockroachDbParameterCollection Parameters => _parameters;

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
        /// Gets or sets the <see cref="CockroachDbConnection"/> used by this command.
        /// </summary>
        protected override DbConnection DbConnection
        {
            get => _connection;
            set
            {
                _connection = (CockroachDbConnection)value;
                _command.Connection = _connection?.InnerConnection;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CockroachDbParameter"/> objects.
        /// </summary>
        protected override DbParameterCollection DbParameterCollection => _parameters;

        /// <summary>
        /// Gets or sets the <see cref="CockroachDbTransaction"/> within which this command executes.
        /// </summary>
        protected override DbTransaction DbTransaction
        {
            get => _transaction;
            set
            {
                _transaction = (CockroachDbTransaction)value;
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
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the instance of <see cref="CockroachDbDataReader"/> object.
        /// </summary>
        /// <returns>The instance of <see cref="CockroachDbDataReader"/>.</returns>
        public new CockroachDbDataReader ExecuteReader()
        {
            try
            {
                return new CockroachDbDataReader(_command.ExecuteReader());
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes a SQL statement against the connection and returns the instance of <see cref="CockroachDbDataReader"/> object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the instance of <see cref="CockroachDbDataReader"/>.</returns>
        public new async Task<CockroachDbDataReader> ExecuteReaderAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return new CockroachDbDataReader(await _command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken).ConfigureAwait(false));
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
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
            try
            {
                return _command.ExecuteNonQuery();
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
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
            try
            {
                return await _command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
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
            try
            {
                return _command.ExecuteScalar();
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
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
            try
            {
                return await _command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a prepared version of the command on an instance of CockroachDB server.
        /// </summary>
        public override void Prepare()
        {
            try
            {
                _command.Prepare();
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="CockroachDbParameter"/> object.
        /// </summary>
        /// <returns>A <see cref="CockroachDbParameter"/> object.</returns>
        protected override DbParameter CreateDbParameter()
        {
            try
            {
                return new CockroachDbParameter(_command.CreateParameter());
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the <see cref="CommandText"/> to the connection and builds a <see cref="CockroachDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="CockroachDbDataReader"/> object.</returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                return new CockroachDbDataReader(_command.ExecuteReader(behavior));
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously sends the <see cref="CommandText"/> to the connection and builds a <see cref="CockroachDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="CockroachDbDataReader"/> object.</returns>
        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(
            CommandBehavior behavior,
            CancellationToken cancellationToken)
        {
            try
            {
                return new CockroachDbDataReader(await _command.ExecuteReaderAsync(behavior, cancellationToken).ConfigureAwait(false));
            }
            catch (NpgsqlException exception)
            {
                throw new CockroachDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="CockroachDbCommand"/> and optionally releases the managed resources.
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
    }
}
