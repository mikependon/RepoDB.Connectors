#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.MariaDb.IntegrationTests.Setup;

namespace RepoDb.Connector.MariaDb.IntegrationTests.Operations
{
    /// <summary>
    /// Integration tests that round-trip every column type declared on the <c>InsertModel</c> table
    /// (see <see cref="Setup.Database.CreateInsertTable"/>) through <see cref="MariaDbCommand"/> /
    /// <see cref="MariaDbDataReader"/>, verifying both the value and the CLR type reported for each column.
    /// </summary>
    [TestClass]
    public class MariaDbTypeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region Helpers

        private static MariaDbDataReader InsertAndRead(
            MariaDbConnection connection,
            string columnName,
            object value)
        {
            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText =
                    $"INSERT INTO `InsertModel` (`RowGuid`, `{columnName}`) VALUES (UUID(), @value);";
                insertCommand.Parameters.AddWithValue("@value", value);
                insertCommand.ExecuteNonQuery();
            }

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText = $"SELECT `{columnName}` FROM `InsertModel` LIMIT 1;";
            var reader = selectCommand.ExecuteReader();
            reader.Read();
            return reader;
        }

        #endregion

        #region Id (BIGINT NOT NULL AUTO_INCREMENT)

        [TestMethod]
        public void TestMariaDbTypeForIdColumn()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO `InsertModel` (`RowGuid`) VALUES (UUID());";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT `Id` FROM `InsertModel` LIMIT 1;";
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        // Assert
                        Assert.AreEqual(typeof(long), reader.GetFieldType(0));
                        Assert.IsGreaterThan(0L, reader.GetInt64(0));
                        Assert.IsFalse(reader.IsDBNull(0));
                    }
                }
            }
        }

        #endregion

        #region RowGuid (CHAR(36) NOT NULL)

        [TestMethod]
        public void TestMariaDbTypeForRowGuidColumn()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = Guid.NewGuid();
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO `InsertModel` (`RowGuid`) VALUES (@value);";
                    insertCommand.Parameters.AddWithValue("@value", expected.ToString("D"));
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT `RowGuid` FROM `InsertModel` LIMIT 1;";
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        // Assert
                        Assert.AreEqual(typeof(Guid), reader.GetFieldType(0));
                        Assert.AreEqual(expected, reader.GetGuid(0));
                        Assert.IsFalse(reader.IsDBNull(0));
                    }
                }
            }
        }

        #endregion

        #region ColumnBit (TINYINT UNSIGNED NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnBit()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnBit", (byte)7))
                {
                    // Assert
                    Assert.AreEqual((byte)7, reader.GetByte(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnBitWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnBit", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnDateTime (DATETIME NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnDateTime()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = new DateTime(2023, 1, 15, 9, 30, 0);
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDateTime", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(DateTime), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetDateTime(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnDateTimeWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDateTime", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnDateTime2 (DATETIME(6) NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnDateTime2()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup - a fractional-second component that is exactly representable at microsecond
                // precision (1234560 ticks = 123456 microseconds), since DATETIME(6) cannot preserve
                // the sub-microsecond (100ns) precision that a .NET DateTime tick can express.
                var expected = new DateTime(2024, 6, 15, 13, 45, 30).AddTicks(1234560);
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDateTime2", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(DateTime), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetDateTime(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnDateTime2WhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDateTime2", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnDecimal (DECIMAL(18,2) NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnDecimal()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = 12345.67M;
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDecimal", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(decimal), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetDecimal(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnDecimalWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnDecimal", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnFloat (DOUBLE NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnFloat()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = 3.14159265358979D;
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnFloat", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(double), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetDouble(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnFloatWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnFloat", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnInt (INT NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnInt()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = -123456789;
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnInt", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(int), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetInt32(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnIntWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnInt", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region ColumnNVarChar (NVARCHAR(2000) NULL)

        [TestMethod]
        public void TestMariaDbTypeForColumnNVarChar()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expected = "MariaDbTypeTest 日本語 Ünïcödé";
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnNVarChar", expected))
                {
                    // Assert
                    Assert.AreEqual(typeof(string), reader.GetFieldType(0));
                    Assert.AreEqual(expected, reader.GetString(0));
                    Assert.IsFalse(reader.IsDBNull(0));
                }
            }
        }

        [TestMethod]
        public void TestMariaDbTypeForColumnNVarCharWhenNull()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();

                // Act
                using (var reader = InsertAndRead(connection, "ColumnNVarChar", DBNull.Value))
                {
                    // Assert
                    Assert.IsTrue(reader.IsDBNull(0));
                }
            }
        }

        #endregion

        #region All columns together

        [TestMethod]
        public void TestMariaDbTypeForAllInsertModelColumnsRoundTrip()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var expectedRowGuid = Guid.NewGuid();
                byte expectedColumnBit = 1;
                var expectedColumnDateTime = new DateTime(2022, 11, 3, 8, 0, 0);
                var expectedColumnDateTime2 = new DateTime(2022, 11, 3, 8, 0, 0).AddTicks(7654320);
                var expectedColumnDecimal = 98765.43M;
                var expectedColumnFloat = 2.71828182845905D;
                var expectedColumnInt = 42;
                var expectedColumnNVarChar = "AllInsertModelColumnsRoundTrip";

                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` " +
                        "(`RowGuid`, `ColumnBit`, `ColumnDateTime`, `ColumnDateTime2`, `ColumnDecimal`, `ColumnFloat`, `ColumnInt`, `ColumnNVarChar`) " +
                        "VALUES " +
                        "(@rowGuid, @columnBit, @columnDateTime, @columnDateTime2, @columnDecimal, @columnFloat, @columnInt, @columnNVarChar);";
                    insertCommand.Parameters.AddWithValue("@rowGuid", expectedRowGuid.ToString("D"));
                    insertCommand.Parameters.AddWithValue("@columnBit", expectedColumnBit);
                    insertCommand.Parameters.AddWithValue("@columnDateTime", expectedColumnDateTime);
                    insertCommand.Parameters.AddWithValue("@columnDateTime2", expectedColumnDateTime2);
                    insertCommand.Parameters.AddWithValue("@columnDecimal", expectedColumnDecimal);
                    insertCommand.Parameters.AddWithValue("@columnFloat", expectedColumnFloat);
                    insertCommand.Parameters.AddWithValue("@columnInt", expectedColumnInt);
                    insertCommand.Parameters.AddWithValue("@columnNVarChar", expectedColumnNVarChar);
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT `Id`, `RowGuid`, `ColumnBit`, `ColumnDateTime`, `ColumnDateTime2`, `ColumnDecimal`, `ColumnFloat`, `ColumnInt`, `ColumnNVarChar` " +
                        "FROM `InsertModel` LIMIT 1;";
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        // Assert
                        Assert.IsGreaterThan(0L, reader.GetInt64(reader.GetOrdinal("Id")));
                        Assert.AreEqual(expectedRowGuid, reader.GetGuid(reader.GetOrdinal("RowGuid")));
                        Assert.AreEqual(expectedColumnBit, reader.GetByte(reader.GetOrdinal("ColumnBit")));
                        Assert.AreEqual(expectedColumnDateTime, reader.GetDateTime(reader.GetOrdinal("ColumnDateTime")));
                        Assert.AreEqual(expectedColumnDateTime2, reader.GetDateTime(reader.GetOrdinal("ColumnDateTime2")));
                        Assert.AreEqual(expectedColumnDecimal, reader.GetDecimal(reader.GetOrdinal("ColumnDecimal")));
                        Assert.AreEqual(expectedColumnFloat, reader.GetDouble(reader.GetOrdinal("ColumnFloat")));
                        Assert.AreEqual(expectedColumnInt, reader.GetInt32(reader.GetOrdinal("ColumnInt")));
                        Assert.AreEqual(expectedColumnNVarChar, reader.GetString(reader.GetOrdinal("ColumnNVarChar")));
                    }
                }
            }
        }

        #endregion
    }
}
