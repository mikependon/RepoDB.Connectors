#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySqlConnector;
using System.Data.Common;

namespace RepoDb.Connector.MariaDbConnector
{
    /// <summary>
    /// The exception that is thrown when MariaDB returns an error.
    /// </summary>
    public class MariaDbException : DbException
    {
        private readonly MySqlException _exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbException"/> class.
        /// </summary>
        /// <param name="exception">The underlying <see cref="MySqlException"/> to wrap.</param>
        internal MariaDbException(MySqlException exception)
            : base(exception.Message, exception)
        {
            _exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public override int ErrorCode => _exception.Number;

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public int Number => _exception.Number;

        /// <summary>
        /// Gets the SQL state.
        /// </summary>
#if NET8_0_OR_GREATER
        public override string SqlState => _exception.SqlState;
#else
        public string SqlState => _exception.SqlState;
#endif

        #endregion
    }
}
