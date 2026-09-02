#region Copyright Attributions

// Copyright (c) 2026 Michael Camara Pendon.
// Licensed under the Apache License, Version 2.0.
// See the LICENSE file in the project root for full license information.

#endregion

using Npgsql;
using System;
using System.Collections;
using System.Data.Common;

namespace RepoDb.Connector.EnterpriseDb
{
    /// <summary>
    /// Represents a collection of parameters relevant to a <see cref="EDBCommand"/>.
    /// </summary>
    public class EDBParameterCollection : DbParameterCollection
    {
        private readonly NpgsqlParameterCollection _parameters;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EDBParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">The underlying <see cref="NpgsqlParameterCollection"/> to wrap.</param>
        internal EDBParameterCollection(NpgsqlParameterCollection parameters)
        {
            _parameters = parameters;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of <see cref="EDBParameter"/> objects in the collection.
        /// </summary>
        public override int Count => _parameters.Count;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="EDBParameterCollection"/> has a fixed size.
        /// </summary>
        public override bool IsFixedSize => _parameters.IsFixedSize;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="EDBParameterCollection"/> is read-only.
        /// </summary>
        public override bool IsReadOnly => _parameters.IsReadOnly;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="EDBParameterCollection"/> is synchronized.
        /// </summary>
        public override bool IsSynchronized => _parameters.IsSynchronized;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="EDBParameterCollection"/>.
        /// </summary>
        public override object SyncRoot => _parameters.SyncRoot;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a parameter and its value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>A <see cref="EDBParameter"/> object representing the provided values.</returns>
        public EDBParameter AddWithValue(
            string parameterName,
            object value)
        {
            return new EDBParameter(_parameters.AddWithValue(parameterName, value));
        }

        /// <summary>
        /// Adds the specified <see cref="EDBParameter"/> object to the collection.
        /// </summary>
        /// <param name="value">The <see cref="EDBParameter"/> to add to the collection.</param>
        /// <returns>The index of the new <see cref="EDBParameter"/> object.</returns>
        public override int Add(object value)
        {
            var parameter = ((EDBParameter)value).InnerParameter;
            _parameters.Add(parameter);
            return _parameters.IndexOf(parameter);
        }

        /// <summary>
        /// Adds an array of values to the end of the <see cref="EDBParameterCollection"/>.
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
        /// Gets a value indicating whether a <see cref="EDBParameter"/> exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="EDBParameter"/> to look for.</param>
        /// <returns>true if the <see cref="EDBParameter"/> exists in the collection; otherwise false.</returns>
        public override bool Contains(object value)
        {
            return _parameters.Contains(((EDBParameter)value).InnerParameter);
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="EDBParameter"/> with the specified parameter name exists in the collection.
        /// </summary>
        /// <param name="value">The name of the <see cref="EDBParameter"/> to look for.</param>
        /// <returns>true if the <see cref="EDBParameter"/> exists in the collection; otherwise false.</returns>
        public override bool Contains(string value)
        {
            return _parameters.Contains(value);
        }

        /// <summary>
        /// Copies <see cref="EDBParameter"/> objects from the collection to the specified array.
        /// </summary>
        /// <param name="array">The array into which to copy the parameters.</param>
        /// <param name="index">The starting index of the array.</param>
        public override void CopyTo(
            Array array,
            int index)
        {
            foreach (NpgsqlParameter parameter in _parameters)
            {
                array.SetValue(new EDBParameter(parameter), index++);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="EDBParameterCollection"/>.
        /// </summary>
        public override IEnumerator GetEnumerator()
        {
            foreach (NpgsqlParameter parameter in _parameters)
            {
                yield return new EDBParameter(parameter);
            }
        }

        /// <summary>
        /// Gets the location of a <see cref="EDBParameter"/> in the collection.
        /// </summary>
        /// <param name="value">The <see cref="EDBParameter"/> to locate.</param>
        /// <returns>The zero-based location of the <see cref="EDBParameter"/> in the collection.</returns>
        public override int IndexOf(object value)
        {
            return _parameters.IndexOf(((EDBParameter)value).InnerParameter);
        }

        /// <summary>
        /// Gets the location of the <see cref="EDBParameter"/> in the collection with a specific parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="EDBParameter"/> to retrieve.</param>
        /// <returns>The zero-based location of the <see cref="EDBParameter"/> in the collection.</returns>
        public override int IndexOf(string parameterName)
        {
            return _parameters.IndexOf(parameterName);
        }

        /// <summary>
        /// Inserts a <see cref="EDBParameter"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the value should be inserted.</param>
        /// <param name="value">The <see cref="EDBParameter"/> to insert.</param>
        public override void Insert(
            int index,
            object value)
        {
            _parameters.Insert(index, ((EDBParameter)value).InnerParameter);
        }

        /// <summary>
        /// Removes the specified <see cref="EDBParameter"/> from the collection.
        /// </summary>
        /// <param name="value">The <see cref="EDBParameter"/> to remove.</param>
        public override void Remove(object value)
        {
            _parameters.Remove(((EDBParameter)value).InnerParameter);
        }

        /// <summary>
        /// Removes the <see cref="EDBParameter"/> from the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to remove.</param>
        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        /// <summary>
        /// Removes the specified <see cref="EDBParameter"/> from the collection using the parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="EDBParameter"/> to remove.</param>
        public override void RemoveAt(string parameterName)
        {
            _parameters.RemoveAt(parameterName);
        }

        /// <summary>
        /// Gets the <see cref="EDBParameter"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to retrieve.</param>
        /// <returns>The <see cref="EDBParameter"/> at the specified index.</returns>
        protected override DbParameter GetParameter(int index)
        {
            return new EDBParameter(_parameters[index]);
        }

        /// <summary>
        /// Gets the <see cref="EDBParameter"/> with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to retrieve.</param>
        /// <returns>The <see cref="EDBParameter"/> with the specified name.</returns>
        protected override DbParameter GetParameter(string parameterName)
        {
            return new EDBParameter(_parameters[parameterName]);
        }

        /// <summary>
        /// Sets the <see cref="EDBParameter"/> at the specified index to a new value.
        /// </summary>
        /// <param name="index">The zero-based index at which to set the parameter.</param>
        /// <param name="value">The new <see cref="EDBParameter"/> value.</param>
        protected override void SetParameter(
            int index,
            DbParameter value)
        {
            _parameters[index] = ((EDBParameter)value).InnerParameter;
        }

        /// <summary>
        /// Sets the <see cref="EDBParameter"/> with the specified name to a new value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="value">The new <see cref="EDBParameter"/> value.</param>
        protected override void SetParameter(
            string parameterName,
            DbParameter value)
        {
            _parameters[parameterName] = ((EDBParameter)value).InnerParameter;
        }

        #endregion
    }
}
