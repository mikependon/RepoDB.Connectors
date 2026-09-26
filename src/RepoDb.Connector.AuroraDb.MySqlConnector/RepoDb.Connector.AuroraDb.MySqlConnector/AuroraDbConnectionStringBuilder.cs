#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using AwsWrapperDataProvider;
using System.Globalization;
using System.Data.Common;

namespace RepoDb.Connector.AuroraDb.MySqlConnector
{
    /// <summary>
    /// Aids in the creation of connection strings by exposing the connection options as properties.
    /// It is backed by the <see cref="AwsWrapperConnectionStringBuilder"/>, so both the AWS wrapper (e.g. <see cref="Plugins"/>)
    /// and the underlying MySqlConnector connection properties are supported.
    /// </summary>
    public class AuroraDbConnectionStringBuilder : DbConnectionStringBuilder
    {
        private readonly AwsWrapperConnectionStringBuilder _builder;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbConnectionStringBuilder"/> class.
        /// </summary>
        public AuroraDbConnectionStringBuilder()
        {
            _builder = new AwsWrapperConnectionStringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraDbConnectionStringBuilder"/> class with the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AuroraDbConnectionStringBuilder(string connectionString)
        {
            _builder = new AwsWrapperConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// The default port of Aurora MySQL, used when no port is specified.
        /// </summary>
        public const uint DefaultPort = 3306;

        /// <summary>
        /// Gets or sets the connection string associated with this <see cref="AuroraDbConnectionStringBuilder"/>.
        /// </summary>
        public new string ConnectionString { get => _builder.ConnectionString; set => _builder.ConnectionString = value; }

        /// <summary>
        /// Gets or sets the name of the server to connect to.
        /// </summary>
        public string Server { get => GetValue("Server"); set => _builder["Server"] = value; }

        /// <summary>
        /// Gets or sets the port on which the server is listening.
        /// </summary>
        public uint Port { get => uint.TryParse(GetValue("Port"), NumberStyles.None, CultureInfo.InvariantCulture, out var port) ? port : DefaultPort; set => _builder["Port"] = value.ToString(CultureInfo.InvariantCulture); }

        /// <summary>
        /// Gets or sets the name of the database to use.
        /// </summary>
        public string Database { get => GetValue("Database"); set => _builder["Database"] = value; }

        /// <summary>
        /// Gets or sets the user id to be used when connecting.
        /// </summary>
        public string UserId { get => GetValue("User ID"); set => _builder["User ID"] = value; }

        /// <summary>
        /// Gets or sets the password to be used when connecting.
        /// </summary>
        public string Password { get => GetValue("Password"); set => _builder["Password"] = value; }

        /// <summary>
        /// Gets or sets the comma-separated list of the AWS wrapper plugin codes to load (e.g. <c>failover,efm</c>).
        /// </summary>
        public string Plugins { get => _builder.Plugins; set => _builder.Plugins = value; }

        #endregion

        #region Helpers

        private string GetValue(string key) =>
            _builder.TryGetValue(key, out var value) ? value?.ToString() : null;

        #endregion
    }
}
