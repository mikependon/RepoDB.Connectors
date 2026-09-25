#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data.Common;

namespace RepoDb.Connector.CockroachDb
{
    /// <summary>
    /// Represents a set of methods for creating instances of the CockroachDB client implementation of the data source classes.
    /// </summary>
    public class CockroachDbFactory : DbProviderFactory
    {
        #region Properties

        /// <summary>
        /// Gets an instance of the <see cref="CockroachDbFactory"/>.
        /// </summary>
        public static readonly CockroachDbFactory Instance = new CockroachDbFactory();

        #endregion

        #region Methods

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance.
        /// </summary>
        public override DbConnection CreateConnection()
        {
            return new CockroachDbConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance.
        /// </summary>
        public override DbCommand CreateCommand()
        {
            return new CockroachDbCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance.
        /// </summary>
        public override DbParameter CreateParameter()
        {
            return new CockroachDbParameter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance.
        /// </summary>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new CockroachDbConnectionStringBuilder();
        }

        #endregion
    }
}
