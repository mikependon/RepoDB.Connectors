#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System;
using System.Collections;
using System.Data.Common;

namespace RepoDb.Connector.CockroachDb
{
    /// <summary>
    /// Represents a collection of parameters relevant to a <see cref="CockroachDbCommand"/>.
    /// </summary>
    public class CockroachDbParameterCollection : DbParameterCollection
    {
        private readonly NpgsqlParameterCollection _parameters;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CockroachDbParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">The underlying <see cref="NpgsqlParameterCollection"/> to wrap.</param>
        internal CockroachDbParameterCollection(NpgsqlParameterCollection parameters)
        {
            _parameters = parameters;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of <see cref="CockroachDbParameter"/> objects in the collection.
        /// </summary>
        public override int Count => _parameters.Count;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CockroachDbParameterCollection"/> has a fixed size.
        /// </summary>
        public override bool IsFixedSize => _parameters.IsFixedSize;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CockroachDbParameterCollection"/> is read-only.
        /// </summary>
        public override bool IsReadOnly => _parameters.IsReadOnly;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CockroachDbParameterCollection"/> is synchronized.
        /// </summary>
        public override bool IsSynchronized => _parameters.IsSynchronized;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="CockroachDbParameterCollection"/>.
        /// </summary>
        public override object SyncRoot => _parameters.SyncRoot;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a parameter and its value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>A <see cref="CockroachDbParameter"/> object representing the provided values.</returns>
        public CockroachDbParameter AddWithValue(
            string parameterName,
            object value)
        {
            return new CockroachDbParameter(_parameters.AddWithValue(parameterName, value));
        }

        /// <summary>
        /// Adds the specified <see cref="CockroachDbParameter"/> object to the collection.
        /// </summary>
        /// <param name="value">The <see cref="CockroachDbParameter"/> to add to the collection.</param>
        /// <returns>The index of the new <see cref="CockroachDbParameter"/> object.</returns>
        public override int Add(object value)
        {
            var parameter = ((CockroachDbParameter)value).InnerParameter;
            _parameters.Add(parameter);
            return _parameters.IndexOf(parameter);
        }

        /// <summary>
        /// Adds an array of values to the end of the <see cref="CockroachDbParameterCollection"/>.
        /// </summary>
        /// <param name="values">The values to add.</param>
        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public override void Clear()
        {
            _parameters.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="CockroachDbParameter"/> exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="CockroachDbParameter"/> to look for.</param>
        /// <returns>true if the <see cref="CockroachDbParameter"/> exists in the collection; otherwise false.</returns>
        public override bool Contains(object value)
        {
            return _parameters.Contains(((CockroachDbParameter)value).InnerParameter);
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="CockroachDbParameter"/> with the specified parameter name exists in the collection.
        /// </summary>
        /// <param name="value">The name of the <see cref="CockroachDbParameter"/> to look for.</param>
        /// <returns>true if the <see cref="CockroachDbParameter"/> exists in the collection; otherwise false.</returns>
        public override bool Contains(string value)
        {
            return _parameters.Contains(value);
        }

        /// <summary>
        /// Copies <see cref="CockroachDbParameter"/> objects from the collection to the specified array.
        /// </summary>
        /// <param name="array">The array into which to copy the parameters.</param>
        /// <param name="index">The starting index of the array.</param>
        public override void CopyTo(
            Array array,
            int index)
        {
            foreach (NpgsqlParameter parameter in _parameters)
            {
                array.SetValue(new CockroachDbParameter(parameter), index++);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CockroachDbParameterCollection"/>.
        /// </summary>
        public override IEnumerator GetEnumerator()
        {
            foreach (NpgsqlParameter parameter in _parameters)
            {
                yield return new CockroachDbParameter(parameter);
            }
        }

        /// <summary>
        /// Gets the location of a <see cref="CockroachDbParameter"/> in the collection.
        /// </summary>
        /// <param name="value">The <see cref="CockroachDbParameter"/> to locate.</param>
        /// <returns>The zero-based location of the <see cref="CockroachDbParameter"/> in the collection.</returns>
        public override int IndexOf(object value)
        {
            return _parameters.IndexOf(((CockroachDbParameter)value).InnerParameter);
        }

        /// <summary>
        /// Gets the location of the <see cref="CockroachDbParameter"/> in the collection with a specific parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="CockroachDbParameter"/> to retrieve.</param>
        /// <returns>The zero-based location of the <see cref="CockroachDbParameter"/> in the collection.</returns>
        public override int IndexOf(string parameterName)
        {
            return _parameters.IndexOf(parameterName);
        }

        /// <summary>
        /// Inserts a <see cref="CockroachDbParameter"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the value should be inserted.</param>
        /// <param name="value">The <see cref="CockroachDbParameter"/> to insert.</param>
        public override void Insert(
            int index,
            object value)
        {
            _parameters.Insert(index, ((CockroachDbParameter)value).InnerParameter);
        }

        /// <summary>
        /// Removes the specified <see cref="CockroachDbParameter"/> from the collection.
        /// </summary>
        /// <param name="value">The <see cref="CockroachDbParameter"/> to remove.</param>
        public override void Remove(object value)
        {
            _parameters.Remove(((CockroachDbParameter)value).InnerParameter);
        }

        /// <summary>
        /// Removes the <see cref="CockroachDbParameter"/> from the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to remove.</param>
        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        /// <summary>
        /// Removes the specified <see cref="CockroachDbParameter"/> from the collection using the parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="CockroachDbParameter"/> to remove.</param>
        public override void RemoveAt(string parameterName)
        {
            _parameters.RemoveAt(parameterName);
        }

        /// <summary>
        /// Gets the <see cref="CockroachDbParameter"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to retrieve.</param>
        /// <returns>The <see cref="CockroachDbParameter"/> at the specified index.</returns>
        protected override DbParameter GetParameter(int index)
        {
            return new CockroachDbParameter(_parameters[index]);
        }

        /// <summary>
        /// Gets the <see cref="CockroachDbParameter"/> with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to retrieve.</param>
        /// <returns>The <see cref="CockroachDbParameter"/> with the specified name.</returns>
        protected override DbParameter GetParameter(string parameterName)
        {
            return new CockroachDbParameter(_parameters[parameterName]);
        }

        /// <summary>
        /// Sets the <see cref="CockroachDbParameter"/> at the specified index to a new value.
        /// </summary>
        /// <param name="index">The zero-based index at which to set the parameter.</param>
        /// <param name="value">The new <see cref="CockroachDbParameter"/> value.</param>
        protected override void SetParameter(
            int index,
            DbParameter value)
        {
            _parameters[index] = ((CockroachDbParameter)value).InnerParameter;
        }

        /// <summary>
        /// Sets the <see cref="CockroachDbParameter"/> with the specified name to a new value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="value">The new <see cref="CockroachDbParameter"/> value.</param>
        protected override void SetParameter(
            string parameterName,
            DbParameter value)
        {
            _parameters[parameterName] = ((CockroachDbParameter)value).InnerParameter;
        }

        #endregion
    }
}
