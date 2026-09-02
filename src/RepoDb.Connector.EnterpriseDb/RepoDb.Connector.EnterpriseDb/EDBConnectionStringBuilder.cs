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
    /// Aids in the creation of connection strings by exposing the connection options as properties.
    /// </summary>
    public class EDBConnectionStringBuilder : DbConnectionStringBuilder
    {
        private readonly NpgsqlConnectionStringBuilder _builder;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBConnectionStringBuilder"/> class.
        /// </summary>
        public EDBConnectionStringBuilder()
        {
            _builder = new NpgsqlConnectionStringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBConnectionStringBuilder"/> class with the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public EDBConnectionStringBuilder(string connectionString)
        {
            _builder = new NpgsqlConnectionStringBuilder(connectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection string associated with this <see cref="EDBConnectionStringBuilder"/>.
        /// </summary>
        public new string ConnectionString { get => _builder.ConnectionString; set => _builder.ConnectionString = value; }

        /// <summary>
        /// Gets or sets the name of the server to connect to.
        /// </summary>
        public string Host { get => _builder.Host; set => _builder.Host = value; }

        /// <summary>
        /// Gets or sets the port on which the server is listening.
        /// </summary>
        public int Port { get => _builder.Port; set => _builder.Port = value; }

        /// <summary>
        /// Gets or sets the name of the database to use.
        /// </summary>
        public string Database { get => _builder.Database; set => _builder.Database = value; }

        /// <summary>
        /// Gets or sets the user name to be used when connecting.
        /// </summary>
        public string Username { get => _builder.Username; set => _builder.Username = value; }

        /// <summary>
        /// Gets or sets the password to be used when connecting.
        /// </summary>
        public string Password { get => _builder.Password; set => _builder.Password = value; }

        #endregion
    }
}
