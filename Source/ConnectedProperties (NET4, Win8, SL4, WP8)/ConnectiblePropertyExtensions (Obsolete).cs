// Copyright (c) 2011-2013 Nito Programs.

namespace Nito.ConnectedProperties
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides convenience methods for connectible properties.
    /// </summary>
    [Obsolete("Use Nito.ConnectedProperties.ConnectibleProperty instead.")]
    public static class ConnectiblePropertyExtensions
    {
        /// <summary>
        /// Disconnects the property, throwing an exception if the property was already disconnected.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <seealso cref="IConnectibleProperty{TValue}.TryDisconnect"/>
        public static void Disconnect<TValue>(this IConnectibleProperty<TValue> property)
        {
            Contract.Requires(property != null);
            if (!property.TryDisconnect())
                throw new InvalidOperationException("Connectible property was already disconnected.");
        }

        /// <summary>
        /// Gets the value of the property, throwing an exception if the property was disconnected.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <returns>The value of the property.</returns>
        /// <seealso cref="IConnectibleProperty{TValue}.TryGet"/>
        /// <seealso cref="IConnectibleProperty{TValue}.GetOrCreate"/>
        /// <seealso cref="GetOrConnect{TValue}"/>
        public static TValue Get<TValue>(this IConnectibleProperty<TValue> property)
        {
            Contract.Requires(property != null);
            TValue ret;
            if (!property.TryGet(out ret))
                throw new InvalidOperationException("Connectible property is disconnected.");
            return ret;
        }

        /// <summary>
        /// Sets the value of the property, throwing an exception if the property was already connected.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <param name="value">The value to set.</param>
        /// <seealso cref="IConnectibleProperty{TValue}.TryConnect"/>
        /// <seealso cref="GetOrConnect{TValue}"/>
        public static void Connect<TValue>(this IConnectibleProperty<TValue> property, TValue value)
        {
            Contract.Requires(property != null);
            if (!property.TryConnect(value))
                throw new InvalidOperationException("Connectible property was already connected.");
        }

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <param name="connectValue">The new value of the property, if it is disconnected.</param>
        /// <returns>The value of the property.</returns>
        /// <seealso cref="Get{TValue}"/>
        /// <seealso cref="IConnectibleProperty{TValue}.GetOrCreate"/>
        public static TValue GetOrConnect<TValue>(this IConnectibleProperty<TValue> property, TValue connectValue)
        {
            Contract.Requires(property != null);
            return property.GetOrCreate(() => connectValue);
        }

        /// <summary>
        /// Sets the value of the property, overwriting any existing value.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<TValue>(this IConnectibleProperty<TValue> property, TValue value)
        {
            Contract.Requires(property != null);
            while (true)
            {
                if (property.TryConnect(value))
                    return;
                property.TryDisconnect();
            }
        }
    }
}
