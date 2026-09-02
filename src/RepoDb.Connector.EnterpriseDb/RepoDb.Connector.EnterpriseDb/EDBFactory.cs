#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Data.Common;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Represents a set of methods for creating instances of the EnterpriseDB client implementation of the data source classes.
    /// </summary>
    public class EDBFactory : DbProviderFactory
    {
        #region Properties

        /// <summary>
        /// Gets an instance of the <see cref="EDBFactory"/>.
        /// </summary>
        public static readonly EDBFactory Instance = new EDBFactory();

        #endregion

        #region Methods

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance.
        /// </summary>
        public override DbConnection CreateConnection()
        {
            return new EDBConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance.
        /// </summary>
        public override DbCommand CreateCommand()
        {
            return new EDBCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance.
        /// </summary>
        public override DbParameter CreateParameter()
        {
            return new EDBParameter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance.
        /// </summary>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new EDBConnectionStringBuilder();
        }

        #endregion
    }
}
