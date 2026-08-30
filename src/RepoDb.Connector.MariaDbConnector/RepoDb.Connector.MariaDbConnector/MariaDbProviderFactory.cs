#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data.Common;

namespace RepoDb.Connector.MariaDbConnector
{
    /// <summary>
    /// Represents a set of methods for creating instances of the MariaDB client implementation of the data source classes.
    /// </summary>
    public class MariaDbProviderFactory : DbProviderFactory
    {
        #region Properties

        /// <summary>
        /// Gets an instance of the <see cref="MariaDbProviderFactory"/>.
        /// </summary>
        public static readonly MariaDbProviderFactory Instance = new MariaDbProviderFactory();

        #endregion

        #region Methods

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance.
        /// </summary>
        public override DbConnection CreateConnection()
        {
            return new MariaDbConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance.
        /// </summary>
        public override DbCommand CreateCommand()
        {
            return new MariaDbCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance.
        /// </summary>
        public override DbParameter CreateParameter()
        {
            return new MariaDbParameter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance.
        /// </summary>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new MariaDbConnectionStringBuilder();
        }

        #endregion
    }
}
