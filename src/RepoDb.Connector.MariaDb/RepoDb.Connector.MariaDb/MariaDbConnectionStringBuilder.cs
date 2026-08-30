#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using MySql.Data.MySqlClient;
using System.Data.Common;

namespace RepoDb.Connector.MariaDb
{
    /// <summary>
    /// Aids in the creation of connection strings by exposing the connection options as properties.
    /// </summary>
    public class MariaDbConnectionStringBuilder : DbConnectionStringBuilder
    {
        private readonly MySqlConnectionStringBuilder _builder;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbConnectionStringBuilder"/> class.
        /// </summary>
        public MariaDbConnectionStringBuilder()
        {
            _builder = new MySqlConnectionStringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbConnectionStringBuilder"/> class with the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MariaDbConnectionStringBuilder(string connectionString)
        {
            _builder = new MySqlConnectionStringBuilder(connectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection string associated with this <see cref="MariaDbConnectionStringBuilder"/>.
        /// </summary>
        public new string ConnectionString { get => _builder.ConnectionString; set => _builder.ConnectionString = value; }

        /// <summary>
        /// Gets or sets the name of the server to connect to.
        /// </summary>
        public string Server { get => _builder.Server; set => _builder.Server = value; }

        /// <summary>
        /// Gets or sets the port on which the server is listening.
        /// </summary>
        public uint Port { get => _builder.Port; set => _builder.Port = value; }

        /// <summary>
        /// Gets or sets the name of the database to use.
        /// </summary>
        public string Database { get => _builder.Database; set => _builder.Database = value; }

        /// <summary>
        /// Gets or sets the user id to be used when connecting.
        /// </summary>
        public string UserId { get => _builder.UserID; set => _builder.UserID = value; }

        /// <summary>
        /// Gets or sets the password to be used when connecting.
        /// </summary>
        public string Password { get => _builder.Password; set => _builder.Password = value; }

        #endregion
    }
}
