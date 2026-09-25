#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data.Common;

namespace RepoDb.Connector.CockcroachDb
{
    /// <summary>
    /// Represents a set of methods for creating instances of the CockroachDB client implementation of the data source classes.
    /// </summary>
    public class CockcroachDbFactory : DbProviderFactory
    {
        #region Properties

        /// <summary>
        /// Gets an instance of the <see cref="CockcroachDbFactory"/>.
        /// </summary>
        public static readonly CockcroachDbFactory Instance = new CockcroachDbFactory();

        #endregion

        #region Methods

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance.
        /// </summary>
        public override DbConnection CreateConnection()
        {
            return new CockcroachDbConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance.
        /// </summary>
        public override DbCommand CreateCommand()
        {
            return new CockcroachDbCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance.
        /// </summary>
        public override DbParameter CreateParameter()
        {
            return new CockcroachDbParameter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance.
        /// </summary>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new CockcroachDbConnectionStringBuilder();
        }

        #endregion
    }
}
