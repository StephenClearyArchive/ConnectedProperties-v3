// <copyright file="ConnectibleProperty.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A property that may be connected to a carrier object at runtime. The property is either connected or disconnected. A disconnected property is different than a connected property value of <c>null</c>. All members are threadsafe.
    /// </summary>
    /// <typeparam name="TValue">The property type.</typeparam>
    [ContractClass(typeof(ConnectiblePropertyContracts<>))]
    public interface IConnectibleProperty<TValue>
    {
        /// <summary>
        /// Attempts to disconnect the property. Returns <c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.
        /// </summary>
        /// <returns><c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.</returns>
        bool TryDisconnect();

        /// <summary>
        /// Gets the value of the property, if it is connected. Returns <c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.
        /// </summary>
        /// <param name="value">Receives the value of the property, if this method returns <c>true</c>.</param>
        /// <returns><c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.</returns>
        bool TryGet(out TValue value);

        /// <summary>
        /// Sets the value of the property, if it is disconnected. Otherwise, does nothing. Returns <c>true</c> if the property value was set; <c>false</c> if the property was already connected.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns><c>true</c> if the property value was set; <c>false</c> if the property was already connected.</returns>
        bool TryConnect(TValue value);

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <param name="connectCallback">The delegate invoked to create the value of the property, if it is disconnected. May not be <c>null</c>. If there is a multithreaded race condition, each thread's delegate may be invoked, but all values except one will be discarded.</param>
        /// <returns>The value of the property.</returns>
        TValue GetOrConnect(Func<TValue> connectCallback);
    }

    [ContractClassFor(typeof(IConnectibleProperty<>))]
    internal abstract class ConnectiblePropertyContracts<TValue> : IConnectibleProperty<TValue>
    {
        public bool TryDisconnect()
        {
            return false;
        }

        public bool TryGet(out TValue value)
        {
            value = default(TValue);
            return false;
        }

        public bool TryConnect(TValue value)
        {
            return false;
        }

        public TValue GetOrConnect(Func<TValue> connectCallback)
        {
            Contract.Requires(connectCallback != null);
            return default(TValue);
        }
    }

    /// <summary>
    /// A property that may be connected to a carrier object at runtime. The property is either connected or disconnected. A disconnected property is different than a connected property value of <c>null</c>. All members are threadsafe.
    /// </summary>
    /// <typeparam name="TKey">The type of carrier objects to which the property may be connected. This must be a reference type.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    internal sealed class ConnectibleProperty<TKey, TValue> : IConnectibleProperty<TValue>
        where TKey : class
    {
        /// <summary>
        /// The property store.
        /// </summary>
        private readonly IPropertyStore<TKey, TValue> store;

        /// <summary>
        /// The carrier object for this property.
        /// </summary>
        private readonly TKey key;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectibleProperty&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="store">The property store. May not be <c>null</c>.</param>
        /// <param name="key">The carrier object for this property. May not be <c>null</c>.</param>
        public ConnectibleProperty(IPropertyStore<TKey, TValue> store, TKey key)
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
        /// Attempts to disconnect the property. Returns <c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.
        /// </summary>
        /// <returns><c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.</returns>
        public bool TryDisconnect()
        {
            return this.store.Remove(this.key);
        }

        /// <summary>
        /// Gets the value of the property, if it is connected. Returns <c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.
        /// </summary>
        /// <param name="value">Receives the value of the property, if this method returns <c>true</c>.</param>
        /// <returns><c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.</returns>
        public bool TryGet(out TValue value)
        {
            return this.store.TryGetValue(this.key, out value);
        }

        /// <summary>
        /// Sets the value of the property, if it is disconnected. Otherwise, does nothing. Returns <c>true</c> if the property value was set; <c>false</c> if the property was already connected.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns><c>true</c> if the property value was set; <c>false</c> if the property was already connected.</returns>
        public bool TryConnect(TValue value)
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
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <param name="connectCallback">The delegate invoked to create the value of the property, if it is disconnected. May not be <c>null</c>. If there is a multithreaded race condition, each thread's delegate may be invoked, but all values except one will be discarded.</param>
        /// <returns>The value of the property.</returns>
        public TValue GetOrConnect(Func<TValue> connectCallback)
        {
            return this.store.GetValue(this.key, connectCallback);
        }
    }
}
