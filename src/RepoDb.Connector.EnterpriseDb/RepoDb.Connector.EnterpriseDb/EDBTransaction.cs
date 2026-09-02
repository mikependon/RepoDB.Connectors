#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System.Data;
using System.Data.Common;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Represents a SQL transaction to be made in an EnterpriseDB database.
    /// </summary>
    public class EDBTransaction : DbTransaction
    {
        private readonly NpgsqlTransaction _transaction;
        private readonly EDBConnection _connection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBTransaction"/> class.
        /// </summary>
        /// <param name="transaction">The underlying <see cref="NpgsqlTransaction"/> to wrap.</param>
        /// <param name="connection">The <see cref="EDBConnection"/> associated with the transaction.</param>
        internal EDBTransaction(
            NpgsqlTransaction transaction,
            EDBConnection connection)
        {
            _transaction = transaction;
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="NpgsqlTransaction"/>.
        /// </summary>
        internal NpgsqlTransaction InnerTransaction => _transaction;

        /// <summary>
        /// Specifies the <see cref="IsolationLevel"/> for this transaction.
        /// </summary>
        public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;

        /// <summary>
        /// Gets the <see cref="EDBConnection"/> object associated with the transaction.
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
        /// Releases the resources used by the <see cref="EDBTransaction"/> and optionally releases the managed resources.
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
