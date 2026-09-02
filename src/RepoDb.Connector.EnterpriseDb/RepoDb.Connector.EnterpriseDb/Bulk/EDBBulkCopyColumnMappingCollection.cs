#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using System.Collections;

namespace RepoDb.Connector.EnterpriseDb.Bulk
{
    /// <summary>
    /// Contains a collection of <see cref="EDBBulkColumnMapping"/> objects.
    /// </summary>
    public class EDBBulkCopyColumnMappingCollection : CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBBulkCopyColumnMappingCollection"/> class.
        /// </summary>
        internal EDBBulkCopyColumnMappingCollection()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="EDBBulkColumnMapping"/> object at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the mapping to retrieve.</param>
        public EDBBulkColumnMapping this[int index]
        {
            get { return (EDBBulkColumnMapping)InnerList[index]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified <see cref="EDBBulkColumnMapping"/> to the collection.
        /// </summary>
        /// <param name="bulkCopyColumnMapping">The mapping to add to the collection.</param>
        /// <returns>The <see cref="EDBBulkColumnMapping"/> that was added to the collection.</returns>
        public EDBBulkColumnMapping Add(
            EDBBulkColumnMapping bulkCopyColumnMapping)
        {
            InnerList.Add(bulkCopyColumnMapping);
            return bulkCopyColumnMapping;
        }

        /// <summary>
        /// Creates and adds a <see cref="EDBBulkColumnMapping"/> using column ordinals to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="EDBBulkColumnMapping"/>.</returns>
        public EDBBulkColumnMapping Add(
            int sourceColumnIndex,
            int destinationColumnIndex)
        {
            return Add(new EDBBulkColumnMapping(sourceColumnIndex, destinationColumnIndex));
        }

        /// <summary>
        /// Creates and adds a <see cref="EDBBulkColumnMapping"/> using a column ordinal for the source column and a column name for the destination column.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="EDBBulkColumnMapping"/>.</returns>
        public EDBBulkColumnMapping Add(
            int sourceColumnIndex,
            string destinationColumn)
        {
            return Add(new EDBBulkColumnMapping(sourceColumnIndex, destinationColumn));
        }

        /// <summary>
        /// Creates and adds a <see cref="EDBBulkColumnMapping"/> using a column name for the source column and a column ordinal for the destination column.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="EDBBulkColumnMapping"/>.</returns>
        public EDBBulkColumnMapping Add(
            string sourceColumn,
            int destinationColumnIndex)
        {
            return Add(new EDBBulkColumnMapping(sourceColumn, destinationColumnIndex));
        }

        /// <summary>
        /// Creates and adds a <see cref="EDBBulkColumnMapping"/> using column names to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="EDBBulkColumnMapping"/>.</returns>
        public EDBBulkColumnMapping Add(
            string sourceColumn,
            string destinationColumn)
        {
            return Add(new EDBBulkColumnMapping(sourceColumn, destinationColumn));
        }

        /// <summary>
        /// Removes all <see cref="EDBBulkColumnMapping"/> items from the collection.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="EDBBulkColumnMapping"/> exists in the collection.
        /// </summary>
        /// <param name="value">The mapping to look for.</param>
        /// <returns>true if the mapping exists in the collection; otherwise false.</returns>
        public bool Contains(
            EDBBulkColumnMapping value)
        {
            return InnerList.Contains(value);
        }

        /// <summary>
        /// Copies all items from the collection into the specified array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(
            EDBBulkColumnMapping[] array,
            int index)
        {
            InnerList.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index of the specified <see cref="EDBBulkColumnMapping"/> within the collection.
        /// </summary>
        /// <param name="value">The mapping to locate.</param>
        /// <returns>The zero-based index of the mapping within the collection.</returns>
        public int IndexOf(
            EDBBulkColumnMapping value)
        {
            return InnerList.IndexOf(value);
        }

        /// <summary>
        /// Inserts a <see cref="EDBBulkColumnMapping"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the mapping should be inserted.</param>
        /// <param name="value">The mapping to insert.</param>
        public void Insert(
            int index,
            EDBBulkColumnMapping value)
        {
            InnerList.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified <see cref="EDBBulkColumnMapping"/> from the collection.
        /// </summary>
        /// <param name="value">The mapping to remove.</param>
        public void Remove(
            EDBBulkColumnMapping value)
        {
            InnerList.Remove(value);
        }

        /// <summary>
        /// Removes the <see cref="EDBBulkColumnMapping"/> at the specified index from the collection.
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
