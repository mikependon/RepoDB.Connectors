#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.MariaDbConnector
{
    /// <summary>
    /// Represents a SQL statement to execute against a MariaDB database.
    /// </summary>
    public class MariaDbCommand : DbCommand
    {
        private readonly MySqlCommand _command;
        private readonly MariaDbParameterCollection _parameters;
        private MariaDbConnection _connection;
        private MariaDbTransaction _transaction;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbCommand"/> class.
        /// </summary>
        public MariaDbCommand()
        {
            _command = new MySqlCommand();
            _parameters = new MariaDbParameterCollection(_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbCommand"/> class with the text of the query.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        public MariaDbCommand(
            string commandText)
        {
            _command = new MySqlCommand(commandText);
            _parameters = new MariaDbParameterCollection(_command.Parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbCommand"/> class with the text of the query and a <see cref="MariaDbConnection"/>.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        /// <param name="connection">A <see cref="MariaDbConnection"/> that represents the connection to a MariaDB server.</param>
        public MariaDbCommand(
            string commandText,
            MariaDbConnection connection)
        {
            _command = new MySqlCommand(commandText, connection.InnerConnection);
            _parameters = new MariaDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbCommand"/> class from an existing <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="command">The underlying <see cref="MySqlCommand"/> to wrap.</param>
        /// <param name="connection">A <see cref="MariaDbConnection"/> that represents the connection to a MariaDB server.</param>
        internal MariaDbCommand(
            MySqlCommand command,
            MariaDbConnection connection)
        {
            _command = command;
            _parameters = new MariaDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="MariaDbParameterCollection"/>.
        /// </summary>
        public new MariaDbParameterCollection Parameters => _parameters;

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
        /// Gets or sets the <see cref="MariaDbConnection"/> used by this command.
        /// </summary>
        protected override DbConnection DbConnection
        {
            get => _connection;
            set
            {
                _connection = (MariaDbConnection)value;
                _command.Connection = _connection?.InnerConnection;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="MariaDbParameter"/> objects.
        /// </summary>
        protected override DbParameterCollection DbParameterCollection => _parameters;

        /// <summary>
        /// Gets or sets the <see cref="MariaDbTransaction"/> within which this command executes.
        /// </summary>
        protected override DbTransaction DbTransaction
        {
            get => _transaction;
            set
            {
                _transaction = (MariaDbTransaction)value;
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
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL statement against the connection and returns the instance of <see cref="MariaDbDataReader"/> object.
        /// </summary>
        /// <returns>The instance of <see cref="MariaDbDataReader"/>.</returns>
        public new MariaDbDataReader ExecuteReader()
        {
            try
            {
                return new MariaDbDataReader(_command.ExecuteReader());
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes a SQL statement against the connection and returns the instance of <see cref="MariaDbDataReader"/> object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the instance of <see cref="MariaDbDataReader"/>.</returns>
        public new async Task<MariaDbDataReader> ExecuteReaderAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return new MariaDbDataReader(await _command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken).ConfigureAwait(false));
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
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
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
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
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
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
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
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
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a prepared version of the command on an instance of MariaDB server.
        /// </summary>
        public override void Prepare()
        {
            try
            {
                _command.Prepare();
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="MariaDbParameter"/> object.
        /// </summary>
        /// <returns>A <see cref="MariaDbParameter"/> object.</returns>
        protected override DbParameter CreateDbParameter()
        {
            try
            {
                return new MariaDbParameter(_command.CreateParameter());
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the <see cref="CommandText"/> to the connection and builds a <see cref="MariaDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <returns>A <see cref="MariaDbDataReader"/> object.</returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                return new MariaDbDataReader(_command.ExecuteReader(behavior));
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously sends the <see cref="CommandText"/> to the connection and builds a <see cref="MariaDbDataReader"/> using one of the <see cref="CommandBehavior"/> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="MariaDbDataReader"/> object.</returns>
        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(
            CommandBehavior behavior,
            CancellationToken cancellationToken)
        {
            try
            {
                return new MariaDbDataReader(await _command.ExecuteReaderAsync(behavior, cancellationToken).ConfigureAwait(false));
            }
            catch (MySqlException exception)
            {
                throw new MariaDbException(exception);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbCommand"/> and optionally releases the managed resources.
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
