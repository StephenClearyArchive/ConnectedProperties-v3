// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    /// <summary>
    /// An enlightenment that is a factory for concurrent dictionaries.
    /// </summary>
    [ContractClass(typeof(ConcurrentDictionaryEnlightenmentContracts))]
    public interface IConcurrentDictionaryEnlightenment
    {
        /// <summary>
        /// Creates a concurrent dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        IConcurrentDictionary<TKey, TValue> Create<TKey, TValue>();
    }

    [ContractClassFor(typeof(IConcurrentDictionaryEnlightenment))]
    internal abstract class ConcurrentDictionaryEnlightenmentContracts : IConcurrentDictionaryEnlightenment
    {
        public IConcurrentDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            Contract.Ensures(Contract.Result<IConcurrentDictionary<TKey, TValue>>() != null);
            return null;
        }
    }

    /// <summary>
    /// A portable interface for a concurrent dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    [ContractClass(typeof(ConcurrentDictionaryContracts<,>))]
    public interface IConcurrentDictionary<TKey, TValue>
    {
        /// <summary>
        /// Adds or updates the value for a key. <paramref name="createCallback"/> and <paramref name="updateCallback"/> may be invoked multiple times. Returns the final value set for that key.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="createCallback">A callback to create the value if the key does not exist.</param>
        /// <param name="updateCallback">A callback to update the value if the key does exist.</param>
        TValue AddOrUpdate(TKey key, Func<TValue> createCallback, Func<TValue, TValue> updateCallback);

        /// <summary>
        /// Gets or adds the value for a key. <paramref name="createCallback"/> may be invoked multiple times. Returns the new value for the key.
        /// </summary>
        /// <param name="key">The key to get or add.</param>
        /// <param name="createCallback">A callback to create the value if the key does not exist.</param>
        TValue GetOrAdd(TKey key, Func<TValue> createCallback);

        /// <summary>
        /// Attempts to add a value for a key. Returns <c>true</c> if the value was added, or <c>false</c> if there was already a value for that key.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        bool TryAdd(TKey key, TValue value);

        /// <summary>
        /// Attempts to remove a key (and its value). Returns <c>true</c> if the key was found and removed, or <c>false</c> if the key did not exist.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        bool TryRemove(TKey key);

        /// <summary>
        /// Attempts to get the value for a key. Returns <c>true</c> if the key was found, or <c>false</c> if the key did not exist.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <param name="value">On return, contains the value corresponding to the key.</param>
        bool TryGet(TKey key, out TValue value);

        /// <summary>
        /// Attempts to update the value for a key. Returns <c>true</c> if the comparison passed and the value was updated, or <c>false</c> if the comparison failed. Comparison is done using the default equality comparer.
        /// </summary>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The value to set, if the comparison passes.</param>
        /// <param name="comparison">The value to compare against the existing key value.</param>
        bool TryUpdate(TKey key, TValue value, TValue comparison);

        /// <summary>
        /// Gets a snapshot of all the keys and values in this dictionary.
        /// </summary>
        IEnumerable<KeyValuePair<TKey, TValue>> Snapshot();
    }

    [ContractClassFor(typeof(IConcurrentDictionary<,>))]
    internal abstract class ConcurrentDictionaryContracts<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
    {
        public TValue AddOrUpdate(TKey key, Func<TValue> createCallback, Func<TValue, TValue> updateCallback)
        {
            Contract.Requires(createCallback != null);
            Contract.Requires(updateCallback != null);
            return default(TValue);
        }

        public TValue GetOrAdd(TKey key, Func<TValue> createCallback)
        {
            Contract.Requires(createCallback != null);
            return default(TValue);
        }

        public bool TryAdd(TKey key, TValue value)
        {
            return false;
        }

        public bool TryRemove(TKey key)
        {
            return false;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            value = default(TValue);
            return false;
        }

        public bool TryUpdate(TKey key, TValue value, TValue comparison)
        {
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Snapshot()
        {
            Contract.Ensures(Contract.Result<IEnumerable<KeyValuePair<TKey, TValue>>>() != null);
            return null;
        }
    }
}
