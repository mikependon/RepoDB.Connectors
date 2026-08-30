#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDb.IntegrationTests.Setup
{
    /// <summary>
    /// A class used as a startup setup for the RepoDb MariaDb bulk-operations test database.
    /// </summary>
    public static class Database
    {
        #region Properties

        /// <summary>
        /// Gets the connection string used to reach the MariaDB server itself (the <c>sys</c> database) -
        /// used only to create the <c>RepoDb</c> database if it does not already exist.
        /// </summary>
        public static string ConnectionStringForSystem { get; private set; }

        /// <summary>
        /// Gets the connection string to be used for the MariaDb database.
        /// </summary>
        public static string ConnectionString { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the creation of the database.
        /// </summary>
        public static void Initialize()
        {
            ConnectionStringForSystem =
                Environment.GetEnvironmentVariable("REPODB_MARIADB_CONSTR_SYSTEM") ??
                "Server=127.0.0.1;Port=3307;Database=sys;User ID=root;Password=RepoDB2026;";

            ConnectionString =
                Environment.GetEnvironmentVariable("REPODB_MARIADB_CONSTR") ??
                "Server=127.0.0.1;Port=3307;Database=RepoDb.Connector.MariaDb;User ID=root;Password=RepoDB2026;AllowLoadLocalInfile=True;AllowUserVariables=True;";

            // Enable server side local in file for bulk
            EnableServerLocalInfile();

            // Create the database first
            CreateDatabase();

            // Create the tables
            CreateTables();
        }

        /// <summary>
        /// Creates the <c>RepoDb</c> database if it does not already exist.
        /// </summary>
        public static void CreateDatabase()
        {
            using var connection = new MariaDbConnection(ConnectionStringForSystem);
            Helper.ExecuteNonQuery(connection, "CREATE DATABASE IF NOT EXISTS `RepoDb.Connector.MariaDb`;");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void EnableServerLocalInfile()
        {
            using var connection = new MariaDbConnection(ConnectionStringForSystem);
            Helper.ExecuteNonQuery(connection, "SET GLOBAL local_infile = 1;");
        }

        /// <summary>
        /// Clean up all the table.
        /// </summary>
        public static void Cleanup()
        {
            using var connection = new MariaDbConnection(ConnectionString);
            Helper.ExecuteNonQuery(connection, "TRUNCATE TABLE `InsertModel`;");
        }

        #endregion

        #region CreateTables

        /// <summary>
        /// Create the necessary tables for testing.
        /// </summary>
        public static void CreateTables()
        {
            CreateInsertTable();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateInsertTable()
        {
            var commandText = @"
                CREATE TABLE IF NOT EXISTS `InsertModel`
                (
                    `Id` BIGINT NOT NULL AUTO_INCREMENT,
                    `RowGuid` CHAR(36) NOT NULL,
                    `ColumnBit` TINYINT UNSIGNED NULL,
                    `ColumnDateTime` DATETIME NULL,
                    `ColumnDateTime2` DATETIME(6) NULL,
                    `ColumnDecimal` DECIMAL(18,2) NULL,
                    `ColumnFloat` DOUBLE NULL,
                    `ColumnInt` INT NULL,
                    `ColumnNVarChar` NVARCHAR(2000) NULL,
                    PRIMARY KEY (`Id`)
                ) ENGINE=InnoDB;";
            Helper.ExecuteNonQuery(new MariaDbConnection(ConnectionString), commandText);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateBulkOperationNonIdentityTable()
        {
            var commandText = @"
                CREATE TABLE IF NOT EXISTS `BulkOperationNonIdentityTable`
                (
                    `Id` BIGINT NOT NULL,
                    `RowGuid` CHAR(36) NOT NULL,
                    `ColumnBit` TINYINT UNSIGNED NULL,
                    `ColumnDateTime` DATETIME NULL,
                    `ColumnDateTime2` DATETIME(6) NULL,
                    `ColumnDecimal` DECIMAL(18,2) NULL,
                    `ColumnFloat` DOUBLE NULL,
                    `ColumnInt` INT NULL,
                    `ColumnNVarChar` NVARCHAR(2000) NULL,
                    PRIMARY KEY (`Id`)
                ) ENGINE=InnoDB;";
            using var connection = new MariaDbConnection(ConnectionString);
            Helper.ExecuteNonQuery(connection, commandText);
        }

        #endregion
    }
}
