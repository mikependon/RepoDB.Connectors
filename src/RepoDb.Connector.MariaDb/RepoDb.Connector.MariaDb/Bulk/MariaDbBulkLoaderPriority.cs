#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDb.Bulk
{
    /// <summary>
    /// Represents the priority set for bulk loading operations.
    /// </summary>
    public enum MariaDbBulkLoaderPriority
    {
        /// <summary>
        /// This is the default and indicates normal priority.
        /// </summary>
        None,

        /// <summary>
        /// Low priority causes the load operation to wait until all readers of the table have finished.
        /// </summary>
        Low,

        /// <summary>
        /// Concurrent priority allows other readers to retrieve data from the table while the load is in progress.
        /// </summary>
        Concurrent,
    }
}
