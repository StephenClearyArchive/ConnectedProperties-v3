// <copyright file="PropertyStoreForReferenceTypes.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Nito.AttachedProperties
{
    /// <summary>
    /// Simple wrapper around <see cref="ConditionalWeakTable{TKey, TValue}"/>. The attached property type must be a reference type.
    /// </summary>
    /// <typeparam name="TKey">The type of objects to which the property may be attached. This must be a reference type.</typeparam>
    /// <typeparam name="TValue">The attached property type. This must be a reference type.</typeparam>
    internal sealed class PropertyStoreForReferenceTypes<TKey, TValue> : IPropertyStore<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// The underlying property store.
        /// </summary>
        private readonly ConditionalWeakTable<TKey, TValue> store;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyStoreForReferenceTypes&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public PropertyStoreForReferenceTypes()
        {
            this.store = new ConditionalWeakTable<TKey, TValue>();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.store != null);
        }

        /// <summary>
        /// Adds a key/value pair to the property store, throwing <see cref="ArgumentException"/> if the key already exists (i.e., the property has already been attached).
        /// </summary>
        /// <param name="key">The object to which to attach the property. May not be <c>null</c>.</param>
        /// <param name="value">The property value to attach.</param>
        public void Add(TKey key, TValue value)
        {
            this.store.Add(key, value);
        }

        /// <summary>
        /// Removes a key/value pair from the property store, returning <c>false</c> if the key did not exist (i.e., there was no property attached).
        /// </summary>
        /// <param name="key">The object from which to detach the property. May not be <c>null</c>.</param>
        /// <returns><c>true</c> if the property was detached; <c>false</c> if the property already was detached.</returns>
        public bool Remove(TKey key)
        {
            return this.store.Remove(key);
        }

        /// <summary>
        /// Retrieves the value of the attached property, returning <c>false</c> if there is no property attached.
        /// </summary>
        /// <param name="key">The object on which to look up the value of the attached property. May not be <c>null</c>.</param>
        /// <param name="value">If this method returns <c>true</c>, the value of the attached property is returned in this parameter.</param>
        /// <returns><c>true</c> if the property was attached; <c>false</c> if the property was detached.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.store.TryGetValue(key, out value);
        }

        /// <summary>
        /// Retrieves the value of the attached property, creating a new value if there is no property attached.
        /// </summary>
        /// <param name="key">The object on which to look up the value of the attached property. May not be <c>null</c>.</param>
        /// <param name="createCallback">The delegate which is invoked to create the value of the attached property if there is no property attached. May not be <c>null</c>.</param>
        /// <returns>The value of the attached property.</returns>
        public TValue GetValue(TKey key, Func<TValue> createCallback)
        {
            return this.store.GetValue(key, _ => createCallback());
        }
    }
}
