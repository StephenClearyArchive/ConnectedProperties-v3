// <copyright file="AttachedPropertyAccessor.cs" company="Nito Programs">
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
    /// An accessor for a property that may be attached to a carrier object at runtime. The attached property is "detached" if it is not attached; this is different than having an attached property value of <c>null</c>. All members are threadsafe.
    /// </summary>
    /// <typeparam name="TValue">The attached property type.</typeparam>
    [ContractClass(typeof(AttachedPropertyAccessorContracts<>))]
    public interface IAttachedPropertyAccessor<TValue>
    {
        /// <summary>
        /// Attempts to detach the attached property. Returns <c>true</c> if the attached property was detached by this method; <c>false</c> if the attached property was already detached.
        /// </summary>
        /// <returns><c>true</c> if the attached property was detached by this method; <c>false</c> if the attached property was already detached.</returns>
        bool TryDetach();

        /// <summary>
        /// Gets the value of the attached property, if it is attached. Returns <c>true</c> if the attached property was returned; <c>false</c> if the attached property was detached.
        /// </summary>
        /// <param name="value">The object receiving the value of the attached property, if it was attached.</param>
        /// <returns><c>true</c> if the attached property was returned; <c>false</c> if the attached property was detached.</returns>
        bool TryGet(out TValue value);

        /// <summary>
        /// Sets the value of the attached property, if it is detached. Otherwise, does nothing.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns><c>true</c> if the attached property value was set; <c>false</c> if the attached property was already attached.</returns>
        bool TryAttach(TValue value);

        /// <summary>
        /// Sets the value of the attached property, overwriting any existing value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void Set(TValue value);

        /// <summary>
        /// Gets the value of the attached property if it is attached; otherwise, sets the value of the attached property and returns the new value.
        /// </summary>
        /// <param name="attachCallback">The delegate invoked to create the value of the attached property, if it is detached. May not be <c>null</c>.</param>
        /// <returns>The value of the attached property.</returns>
        TValue GetOrAttach(Func<TValue> attachCallback);
    }

    [ContractClassFor(typeof(IAttachedPropertyAccessor<>))]
    internal abstract class AttachedPropertyAccessorContracts<TValue> : IAttachedPropertyAccessor<TValue>
    {
        public bool TryDetach()
        {
            return false;
        }

        public bool TryGet(out TValue value)
        {
            value = default(TValue);
            return false;
        }

        public bool TryAttach(TValue value)
        {
            return false;
        }

        public void Set(TValue value)
        {
        }

        public TValue GetOrAttach(Func<TValue> attachCallback)
        {
            Contract.Requires(attachCallback != null);
            return default(TValue);
        }
    }

    /// <summary>
    /// An accessor for a property that may be attached to a carrier object at runtime. The attached property is "detached" if it is not attached; this is different than having an attached property value of <c>null</c>. All members are threadsafe.
    /// </summary>
    /// <typeparam name="TKey">The type of carrier objects to which the property may be attached. This must be a reference type.</typeparam>
    /// <typeparam name="TValue">The attached property type.</typeparam>
    internal sealed class AttachedPropertyAccessor<TKey, TValue> : IAttachedPropertyAccessor<TValue>
        where TKey : class
    {
        /// <summary>
        /// The property store.
        /// </summary>
        private readonly IPropertyStore<TKey, TValue> store;

        /// <summary>
        /// The carrier object for this attached property.
        /// </summary>
        private readonly TKey key;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachedPropertyAccessor&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="store">The property store. May not be <c>null</c>.</param>
        /// <param name="key">The carrier object for this attached property. May not be <c>null</c>.</param>
        public AttachedPropertyAccessor(IPropertyStore<TKey, TValue> store, TKey key)
        {
            Contract.Requires(store != null);
            Contract.Requires(key != null);

            this.store = store;
            this.key = key;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.store != null);
            Contract.Invariant(this.key != null);
        }

        /// <summary>
        /// Attempts to detach the attached property. Returns <c>true</c> if the attached property was detached by this method; <c>false</c> if the attached property was already detached.
        /// </summary>
        /// <returns><c>true</c> if the attached property was detached by this method; <c>false</c> if the attached property was already detached.</returns>
        public bool TryDetach()
        {
            return this.store.Remove(this.key);
        }

        /// <summary>
        /// Gets the value of the attached property, if it is attached. Returns <c>true</c> if the attached property was returned; <c>false</c> if the attached property was detached.
        /// </summary>
        /// <param name="value">The object receiving the value of the attached property, if it was attached.</param>
        /// <returns><c>true</c> if the attached property was returned; <c>false</c> if the attached property was detached.</returns>
        public bool TryGet(out TValue value)
        {
            return this.store.TryGetValue(this.key, out value);
        }

        /// <summary>
        /// Sets the value of the attached property, if it is detached. Otherwise, does nothing.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns><c>true</c> if the attached property value was set; <c>false</c> if the attached property was already attached.</returns>
        public bool TryAttach(TValue value)
        {
            try
            {
                this.store.Add(this.key, value);
                return true;
            }
            catch (ArgumentException)
            {
                // Vexing exception.
                return false;
            }
        }

        /// <summary>
        /// Sets the value of the attached property, overwriting any existing value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(TValue value)
        {
            while (true)
            {
                if (this.TryAttach(value))
                    return;
                this.TryDetach();
            }
        }

        /// <summary>
        /// Gets the value of the attached property if it is attached; otherwise, sets the value of the attached property and returns the new value.
        /// </summary>
        /// <param name="attachCallback">The delegate invoked to create the value of the attached property, if it is detached. May not be <c>null</c>.</param>
        /// <returns>The value of the attached property.</returns>
        public TValue GetOrAttach(Func<TValue> attachCallback)
        {
            return this.store.GetValue(this.key, attachCallback);
        }
    }
}
