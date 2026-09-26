#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using RepoDb.Connector.AuroraDb.IntegrationTests.Setup;
using System.Data;

namespace RepoDb.Connector.AuroraDb.IntegrationTests.Operations
{
    [TestClass]
    public class TransactionTest
    {
        private const string InsertCommandText =
            "INSERT INTO \"InsertModel\" (\"RowGuid\", \"ColumnNVarChar\") VALUES (gen_random_uuid(), 'TransactionTest');";

        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Database.Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        private static int CountTransactionRows()
        {
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            return Helper.CountRowsWhere(connection, "InsertModel", "\"ColumnNVarChar\" = 'TransactionTest'");
        }

        #region Positive

        [TestMethod]
        public void TestAuroraDbTransactionCommit()
        {
            // Setup
            using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = InsertCommandText;
                command.ExecuteNonQuery();

                // Act
                transaction.Commit();
            }

            // Assert
            Assert.AreEqual(1, CountTransactionRows());
        }

        [TestMethod]
        public void TestAuroraDbTransactionRollback()
        {
            // Setup
            using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = InsertCommandText;
                command.ExecuteNonQuery();

                // Act
                transaction.Rollback();
            }

            // Assert
            Assert.AreEqual(0, CountTransactionRows());
        }

        [TestMethod]
        public async Task TestAuroraDbTransactionCommitAsync()
        {
            // Setup
            await using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();
                await using var command = connection.CreateCommand();
                command.Transaction = (AuroraDbTransaction)transaction;
                command.CommandText = InsertCommandText;
                await command.ExecuteNonQueryAsync();

                // Act
                await transaction.CommitAsync();
            }

            // Assert
            Assert.AreEqual(1, CountTransactionRows());
        }

        [TestMethod]
        public async Task TestAuroraDbTransactionRollbackAsync()
        {
            // Setup
            await using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();
                await using var command = connection.CreateCommand();
                command.Transaction = (AuroraDbTransaction)transaction;
                command.CommandText = InsertCommandText;
                await command.ExecuteNonQueryAsync();

                // Act
                await transaction.RollbackAsync();
            }

            // Assert
            Assert.AreEqual(0, CountTransactionRows());
        }

        [TestMethod]
        public void TestAuroraDbTransactionDisposeWithoutCommitRollsBack()
        {
            // Setup
            using (var connection = new AuroraDbConnection(Database.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = InsertCommandText;
                    command.ExecuteNonQuery();
                }
            }

            // Assert
            Assert.AreEqual(0, CountTransactionRows());
        }

        [TestMethod]
        [DataRow(IsolationLevel.ReadCommitted)]
        [DataRow(IsolationLevel.RepeatableRead)]
        [DataRow(IsolationLevel.Serializable)]
        public void TestAuroraDbTransactionIsolationLevel(
            IsolationLevel isolationLevel)
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();

            // Act
            using var transaction = connection.BeginTransaction(isolationLevel);

            // Assert
            Assert.AreEqual(isolationLevel, transaction.IsolationLevel);
            Assert.AreSame(connection, transaction.Connection);
        }

        #endregion

        #region Negative

        [TestMethod]
        public void ThrowOnAuroraDbTransactionCommitAfterFailedCommand()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "SELECT * FROM \"InvalidTable\";";

            // Act
            Assert.Throws<AuroraDbException>(() => command.ExecuteNonQuery());

            // Assert - the failed statement aborts the transaction, hence nothing is persisted
            transaction.Rollback();
            Assert.AreEqual(0, CountTransactionRows());
        }

        [TestMethod]
        public void ThrowOnAuroraDbTransactionCommitTwice()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            transaction.Commit();

            // Act
            void Act() => transaction.Commit();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbTransactionRollbackAfterCommit()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            transaction.Commit();

            // Act
            void Act() => transaction.Rollback();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbTransactionBeginOnClosedConnection()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);

            // Act
            void Act() => connection.BeginTransaction();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        [TestMethod]
        public void ThrowOnAuroraDbTransactionBeginTwice()
        {
            // Setup
            using var connection = new AuroraDbConnection(Database.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            // Act
            void Act() => connection.BeginTransaction();

            // Assert
            Assert.Throws<InvalidOperationException>(Act);
        }

        #endregion
    }
}
