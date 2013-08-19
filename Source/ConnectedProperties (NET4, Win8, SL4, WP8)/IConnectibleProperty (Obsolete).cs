// Copyright (c) 2011-2013 Nito Programs.

namespace Nito.ConnectedProperties
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A property that may be connected to a carrier object at runtime. The property is either connected or disconnected. A disconnected property is different than a connected property value of <c>null</c>. All members are threadsafe.
    /// </summary>
    /// <typeparam name="TValue">The property type.</typeparam>
    [ContractClass(typeof(ConnectiblePropertyContracts<>))]
    [Obsolete("Use ConnectibleProperty instead.")]
    public interface IConnectibleProperty<TValue>
    {
        /// <summary>
        /// Attempts to disconnect the property. Returns <c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.
        /// </summary>
        /// <returns><c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.</returns>
        /// <seealso cref="ConnectiblePropertyExtensions.Disconnect{TValue}"/>
        bool TryDisconnect();

        /// <summary>
        /// Gets the value of the property, if it is connected. Returns <c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.
        /// </summary>
        /// <param name="value">Receives the value of the property, if this method returns <c>true</c>.</param>
        /// <returns><c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.</returns>
        /// <seealso cref="ConnectiblePropertyExtensions.Get{TValue}"/>
        /// <seealso cref="GetOrCreate"/>
        /// <seealso cref="ConnectiblePropertyExtensions.GetOrConnect{TValue}"/>
        bool TryGet(out TValue value);

        /// <summary>
        /// Sets the value of the property, if it is disconnected. Otherwise, does nothing. Returns <c>true</c> if the property value was set; <c>false</c> if the property was already connected.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns><c>true</c> if the property value was set; <c>false</c> if the property was already connected.</returns>
        /// <seealso cref="ConnectiblePropertyExtensions.Connect{TValue}"/>
        /// <seealso cref="ConnectiblePropertyExtensions.GetOrConnect{TValue}"/>
        bool TryConnect(TValue value);

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <param name="createCallback">The delegate invoked to create the value of the property, if it is disconnected. May not be <c>null</c>. If there is a multithreaded race condition, each thread's delegate may be invoked, but all values except one will be discarded.</param>
        /// <returns>The value of the property.</returns>
        /// <seealso cref="ConnectiblePropertyExtensions.Get{TValue}"/>
        /// <seealso cref="ConnectiblePropertyExtensions.GetOrConnect{TValue}"/>
        TValue GetOrCreate(Func<TValue> createCallback);
    }

    [ContractClassFor(typeof(IConnectibleProperty<>))]
    [Obsolete]
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

        public TValue GetOrCreate(Func<TValue> createCallback)
        {
            Contract.Requires(createCallback != null);
            return default(TValue);
        }
    }
}
