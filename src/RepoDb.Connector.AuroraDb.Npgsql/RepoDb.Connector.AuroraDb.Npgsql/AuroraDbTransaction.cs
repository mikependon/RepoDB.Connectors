#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using System.Data;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Connector.AuroraDb.Npgsql
{
    /// <summary>
    /// Represents a SQL transaction to be made in a AuroraDB database.
    /// </summary>
    public class AuroraDbTransaction : DbTransaction
    {
        private readonly AwsWrapperTransaction _transaction;
        private readonly AuroraDbConnection _connection;
        private bool _completed;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbTransaction"/> class.
        /// </summary>
        /// <param name="transaction">The underlying <see cref="AwsWrapperTransaction"/> to wrap.</param>
        /// <param name="connection">The <see cref="AuroraDbConnection"/> associated with the transaction.</param>
        internal AuroraDbTransaction(
            AwsWrapperTransaction transaction,
            AuroraDbConnection connection)
        {
            _transaction = transaction;
            _connection = connection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="AwsWrapperTransaction"/>.
        /// </summary>
        internal AwsWrapperTransaction InnerTransaction => _transaction;

        /// <summary>
        /// Specifies the <see cref="IsolationLevel"/> for this transaction.
        /// </summary>
        public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;

        /// <summary>
        /// Gets the <see cref="AuroraDbConnection"/> object associated with the transaction.
        /// </summary>
        protected override DbConnection DbConnection => _connection;

        #endregion

        #region Methods

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public override void Commit()
        {
            EnsureNotCompleted();
            _transaction.Commit();
            _completed = true;
        }

        /// <summary>
        /// Asynchronously commits the database transaction.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task CommitAsync(
            CancellationToken cancellationToken = default)
        {
            EnsureNotCompleted();
            return CompleteAsync(_transaction.CommitAsync(cancellationToken));
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public override void Rollback()
        {
            EnsureNotCompleted();
            _transaction.Rollback();
            _completed = true;
        }

        /// <summary>
        /// Asynchronously rolls back a transaction from a pending state.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task RollbackAsync(
            CancellationToken cancellationToken = default)
        {
            EnsureNotCompleted();
            return CompleteAsync(_transaction.RollbackAsync(cancellationToken));
        }

        /// <summary>
        /// Releases the resources used by the <see cref="AuroraDbTransaction"/> and optionally releases the managed resources.
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

        #region Helpers

        /// <summary>
        /// Awaits the completion of the underlying commit/rollback, then flags this transaction as completed.
        /// </summary>
        private async Task CompleteAsync(Task task)
        {
            await task.ConfigureAwait(false);
            _completed = true;
        }

        /// <summary>
        /// Ensures that this transaction has not been committed or rolled back yet.
        /// </summary>
        /// <exception cref="InvalidOperationException">This transaction has already completed.</exception>
        private void EnsureNotCompleted()
        {
            if (_completed)
            {
                throw new InvalidOperationException("This transaction has completed; it is no longer usable.");
            }
        }

        #endregion
    }
}
