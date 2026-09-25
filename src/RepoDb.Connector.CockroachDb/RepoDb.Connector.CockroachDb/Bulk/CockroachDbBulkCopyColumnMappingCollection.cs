#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Collections;

namespace RepoDb.Connector.CockroachDb.Bulk
{
    /// <summary>
    /// Contains a collection of <see cref="CockroachDbBulkColumnMapping"/> objects.
    /// </summary>
    public class CockroachDbBulkCopyColumnMappingCollection : CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbBulkCopyColumnMappingCollection"/> class.
        /// </summary>
        internal CockroachDbBulkCopyColumnMappingCollection()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="CockroachDbBulkColumnMapping"/> object at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the mapping to retrieve.</param>
        public CockroachDbBulkColumnMapping this[int index]
        {
            get { return (CockroachDbBulkColumnMapping)InnerList[index]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified <see cref="CockroachDbBulkColumnMapping"/> to the collection.
        /// </summary>
        /// <param name="bulkCopyColumnMapping">The mapping to add to the collection.</param>
        /// <returns>The <see cref="CockroachDbBulkColumnMapping"/> that was added to the collection.</returns>
        public CockroachDbBulkColumnMapping Add(
            CockroachDbBulkColumnMapping bulkCopyColumnMapping)
        {
            InnerList.Add(bulkCopyColumnMapping);
            return bulkCopyColumnMapping;
        }

        /// <summary>
        /// Creates and adds a <see cref="CockroachDbBulkColumnMapping"/> using column ordinals to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="CockroachDbBulkColumnMapping"/>.</returns>
        public CockroachDbBulkColumnMapping Add(
            int sourceColumnIndex,
            int destinationColumnIndex)
        {
            return Add(new CockroachDbBulkColumnMapping(sourceColumnIndex, destinationColumnIndex));
        }

        /// <summary>
        /// Creates and adds a <see cref="CockroachDbBulkColumnMapping"/> using a column ordinal for the source column and a column name for the destination column.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="CockroachDbBulkColumnMapping"/>.</returns>
        public CockroachDbBulkColumnMapping Add(
            int sourceColumnIndex,
            string destinationColumn)
        {
            return Add(new CockroachDbBulkColumnMapping(sourceColumnIndex, destinationColumn));
        }

        /// <summary>
        /// Creates and adds a <see cref="CockroachDbBulkColumnMapping"/> using a column name for the source column and a column ordinal for the destination column.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="CockroachDbBulkColumnMapping"/>.</returns>
        public CockroachDbBulkColumnMapping Add(
            string sourceColumn,
            int destinationColumnIndex)
        {
            return Add(new CockroachDbBulkColumnMapping(sourceColumn, destinationColumnIndex));
        }

        /// <summary>
        /// Creates and adds a <see cref="CockroachDbBulkColumnMapping"/> using column names to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="CockroachDbBulkColumnMapping"/>.</returns>
        public CockroachDbBulkColumnMapping Add(
            string sourceColumn,
            string destinationColumn)
        {
            return Add(new CockroachDbBulkColumnMapping(sourceColumn, destinationColumn));
        }

        /// <summary>
        /// Removes all <see cref="CockroachDbBulkColumnMapping"/> items from the collection.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="CockroachDbBulkColumnMapping"/> exists in the collection.
        /// </summary>
        /// <param name="value">The mapping to look for.</param>
        /// <returns>true if the mapping exists in the collection; otherwise false.</returns>
        public bool Contains(
            CockroachDbBulkColumnMapping value)
        {
            return InnerList.Contains(value);
        }

        /// <summary>
        /// Copies all items from the collection into the specified array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(
            CockroachDbBulkColumnMapping[] array,
            int index)
        {
            InnerList.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index of the specified <see cref="CockroachDbBulkColumnMapping"/> within the collection.
        /// </summary>
        /// <param name="value">The mapping to locate.</param>
        /// <returns>The zero-based index of the mapping within the collection.</returns>
        public int IndexOf(
            CockroachDbBulkColumnMapping value)
        {
            return InnerList.IndexOf(value);
        }

        /// <summary>
        /// Inserts a <see cref="CockroachDbBulkColumnMapping"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the mapping should be inserted.</param>
        /// <param name="value">The mapping to insert.</param>
        public void Insert(
            int index,
            CockroachDbBulkColumnMapping value)
        {
            InnerList.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified <see cref="CockroachDbBulkColumnMapping"/> from the collection.
        /// </summary>
        /// <param name="value">The mapping to remove.</param>
        public void Remove(
            CockroachDbBulkColumnMapping value)
        {
            InnerList.Remove(value);
        }

        /// <summary>
        /// Removes the <see cref="CockroachDbBulkColumnMapping"/> at the specified index from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the mapping to remove.</param>
        public new void RemoveAt(
            int index)
        {
            base.RemoveAt(index);
        }

        #endregion
    }
}
