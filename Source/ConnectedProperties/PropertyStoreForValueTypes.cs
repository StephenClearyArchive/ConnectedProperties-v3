// <copyright file="PropertyStoreForValueTypes.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A wrapper around <see cref="ConditionalWeakTable{TKey, TValue}"/> providing a level of indirection for <typeparamref name="TValue"/>. The property type may be a value type or reference type.
    /// </summary>
    /// <typeparam name="TKey">The type of carrier objects to which the property may be connected. This must be a reference type.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    internal sealed class PropertyStoreForValueTypes<TKey, TValue> : IPropertyStore<TKey, TValue>
        where TKey : class
    {
        /// <summary>
        /// The underlying property store.
        /// </summary>
        private readonly ConditionalWeakTable<TKey, ValueHolder<TValue>> store;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyStoreForValueTypes&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public PropertyStoreForValueTypes()
        {
            this.store = new ConditionalWeakTable<TKey, ValueHolder<TValue>>();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.store != null);
        }

        /// <summary>
        /// Adds a key/value pair to the property store, throwing <see cref="ArgumentException"/> if the key already exists (i.e., the property has already been connected).
        /// </summary>
        /// <param name="key">The carrier object to which to connect the property. May not be <c>null</c>.</param>
        /// <param name="value">The property value to connect.</param>
        public void Add(TKey key, TValue value)
        {
            this.store.Add(key, new ValueHolder<TValue> { Value = value });
        }

        /// <summary>
        /// Removes a key/value pair from the property store, returning <c>false</c> if the key did not exist (i.e., there was no property connected).
        /// </summary>
        /// <param name="key">The carrier object from which to disconnect the property. May not be <c>null</c>.</param>
        /// <returns><c>true</c> if the property was disconnected; <c>false</c> if the property already was disconnected.</returns>
        public bool Remove(TKey key)
        {
            return this.store.Remove(key);
        }

        /// <summary>
        /// Retrieves the value of the property, returning <c>false</c> if there is no property connected.
        /// </summary>
        /// <param name="key">The carrier object on which to look up the value of the connected property. May not be <c>null</c>.</param>
        /// <param name="value">If this method returns <c>true</c>, the value of the property is returned in this parameter.</param>
        /// <returns><c>true</c> if the property was connected; <c>false</c> if the property was disconnected.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            ValueHolder<TValue> holder;
            var ret = this.store.TryGetValue(key, out holder);
            Contract.Assume(!ret || holder != null);
            value = ret ? holder.Value : default(TValue);
            return ret;
        }

        /// <summary>
        /// Retrieves the value of the property, creating a new value if there is no property connected.
        /// </summary>
        /// <param name="key">The carrier object on which to look up the value of the property. May not be <c>null</c>.</param>
        /// <param name="createCallback">The delegate which is invoked to create the value of the property if there is no property connected. May not be <c>null</c>.</param>
        /// <returns>The value of the property.</returns>
        public TValue GetValue(TKey key, Func<TValue> createCallback)
        {
            var ret = this.store.GetValue(key, _ => new ValueHolder<TValue> { Value = createCallback() });
            Contract.Assume(ret != null);
            return ret.Value;
        }
    }
}
