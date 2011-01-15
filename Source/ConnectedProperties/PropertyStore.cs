// <copyright file="PropertyStore.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A minimal wrapper around <see cref="System.Runtime.CompilerServices.ConditionalWeakTable{TKey,TValue}"/>. This interface is exposed by property store implementations and consumed by <see cref="IConnectibleProperty{TValue}"/> implementations.
    /// </summary>
    /// <typeparam name="TKey">The type of carrier objects to which the property may be connected.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    [ContractClass(typeof(PropertyStoreContracts<,>))]
    internal interface IPropertyStore<TKey, TValue>
        where TKey : class
    {
        /// <summary>
        /// Adds a key/value pair to the property store, throwing <see cref="ArgumentException"/> if the key already exists (i.e., the property has already been connected).
        /// </summary>
        /// <param name="key">The carrier object to which to connect the property. May not be <c>null</c>.</param>
        /// <param name="value">The property value to connect.</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Removes a key/value pair from the property store, returning <c>false</c> if the key did not exist (i.e., there was no property connected).
        /// </summary>
        /// <param name="key">The carrier object from which to disconnect the property. May not be <c>null</c>.</param>
        /// <returns><c>true</c> if the property was disconnected; <c>false</c> if the property already was disconnected.</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Retrieves the value of the property, returning <c>false</c> if there is no property connected.
        /// </summary>
        /// <param name="key">The carrier object on which to look up the value of the connected property. May not be <c>null</c>.</param>
        /// <param name="value">If this method returns <c>true</c>, the value of the property is returned in this parameter.</param>
        /// <returns><c>true</c> if the property was connected; <c>false</c> if the property was disconnected.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Retrieves the value of the property, creating a new value if there is no property connected.
        /// </summary>
        /// <param name="key">The carrier object on which to look up the value of the property. May not be <c>null</c>.</param>
        /// <param name="createCallback">The delegate which is invoked to create the value of the property if there is no property connected. May not be <c>null</c>.</param>
        /// <returns>The value of the property.</returns>
        TValue GetValue(TKey key, Func<TValue> createCallback);
    }

    [ContractClassFor(typeof(IPropertyStore<,>))]
    internal abstract class PropertyStoreContracts<TKey, TValue> : IPropertyStore<TKey, TValue>
        where TKey : class
    {
        public void Add(TKey key, TValue value)
        {
            Contract.Requires(key != null);
        }

        public bool Remove(TKey key)
        {
            Contract.Requires(key != null);
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Contract.Requires(key != null);
            value = default(TValue);
            return false;
        }

        public TValue GetValue(TKey key, Func<TValue> createCallback)
        {
            Contract.Requires(key != null);
            Contract.Requires(createCallback != null);
            return default(TValue);
        }
    }
}
