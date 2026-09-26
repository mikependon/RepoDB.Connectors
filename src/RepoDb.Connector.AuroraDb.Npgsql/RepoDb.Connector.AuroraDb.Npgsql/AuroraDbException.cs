#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider.Driver.Exceptions;
using Npgsql;
using System.Data.Common;

namespace RepoDb.Connector.AuroraDb.Npgsql
{
    /// <summary>
    /// The exception that is thrown when Aurora returns an error.
    /// It wraps the <see cref="NpgsqlException"/> or the <see cref="AwsWrapperDbException"/> raised by the underlying provider.
    /// Failover-related exceptions of the AWS wrapper (e.g. <c>FailoverSuccessException</c>) are not wrapped and are surfaced as-is.
    /// </summary>
    public class AuroraDbException : DbException
    {
        private readonly DbException _exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbException"/> class.
        /// </summary>
        /// <param name="exception">The underlying <see cref="NpgsqlException"/> or <see cref="AwsWrapperDbException"/> to wrap.</param>
        internal AuroraDbException(DbException exception)
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
        public override string SqlState => _exception is AwsWrapperDbException wrapperException ? wrapperException.SqlState : _exception.SqlState;

        #endregion
    }
}
