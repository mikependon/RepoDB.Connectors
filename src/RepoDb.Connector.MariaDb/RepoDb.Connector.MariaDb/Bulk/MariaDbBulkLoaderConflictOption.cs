#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

namespace RepoDb.Connector.MariaDb.Bulk
{
    /// <summary>
    /// Represents the behavior when conflicts arise during bulk loading operations.
    /// </summary>
    public enum MariaDbBulkLoaderConflictOption
    {
        /// <summary>
        /// This is the default and indicates normal operation. A key conflict will raise an error and the load operation is aborted.
        /// </summary>
        None,

        /// <summary>
        /// Replace column values when a key conflict occurs.
        /// </summary>
        Replace,

        /// <summary>
        /// Ignore any rows where the primary key conflicts.
        /// </summary>
        Ignore,
    }
}
