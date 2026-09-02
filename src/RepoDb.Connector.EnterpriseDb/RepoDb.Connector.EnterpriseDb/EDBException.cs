#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System.Data.Common;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// The exception that is thrown when EnterpriseDB returns an error.
    /// </summary>
    public class EDBException : DbException
    {
        private readonly NpgsqlException _exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBException"/> class.
        /// </summary>
        /// <param name="exception">The underlying <see cref="NpgsqlException"/> to wrap.</param>
        internal EDBException(NpgsqlException exception)
            : base(exception.Message, exception)
        {
            _exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public override int ErrorCode => _exception.ErrorCode;

        /// <summary>
        /// Gets the SQL state.
        /// </summary>
        public override string SqlState => _exception.SqlState;

        #endregion
    }
}
