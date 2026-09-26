#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data.Common;

namespace RepoDb.Connector.AuroraDb
{
    /// <summary>
    /// Represents a set of methods for creating instances of the AuroraDB client implementation of the data source classes.
    /// </summary>
    public class AuroraDbFactory : DbProviderFactory
    {
        #region Properties

        /// <summary>
        /// Gets an instance of the <see cref="AuroraDbFactory"/>.
        /// </summary>
        public static readonly AuroraDbFactory Instance = new AuroraDbFactory();

        #endregion

        #region Methods

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance.
        /// </summary>
        public override DbConnection CreateConnection()
        {
            return new AuroraDbConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance.
        /// </summary>
        public override DbCommand CreateCommand()
        {
            return new AuroraDbCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance.
        /// </summary>
        public override DbParameter CreateParameter()
        {
            return new AuroraDbParameter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance.
        /// </summary>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new AuroraDbConnectionStringBuilder();
        }

        #endregion
    }
}
