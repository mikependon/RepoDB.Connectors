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
    /// Represents a SQL transaction to be made in a MariaDB database.
    /// </summary>
    public class MariaDbTransaction : DbTransaction
    {
        private readonly MySqlTransaction _transaction;
        private readonly MariaDbConnection _connection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbTransaction"/> class.
        /// </summary>
        /// <param name="transaction">The underlying <see cref="MySqlTransaction"/> to wrap.</param>
        /// <param name="connection">The <see cref="MariaDbConnection"/> associated with the transaction.</param>
        internal MariaDbTransaction(
            MySqlTransaction transaction,
            MariaDbConnection connection)
        {
            _transaction = transaction;
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="MySqlTransaction"/>.
        /// </summary>
        internal MySqlTransaction InnerTransaction => _transaction;

        /// <summary>
        /// Specifies the <see cref="IsolationLevel"/> for this transaction.
        /// </summary>
        public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;

        /// <summary>
        /// Gets the <see cref="MariaDbConnection"/> object associated with the transaction.
        /// </summary>
        protected override DbConnection DbConnection => _connection;

        #endregion

        #region Methods

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public override void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public override void Rollback()
        {
            _transaction.Rollback();
        }

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbTransaction"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release managed resources; otherwise, false.</param>
        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                _transaction.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
