// <copyright file="ConnectiblePropertyExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides convenience methods for connectible properties.
    /// </summary>
    public static class ConnectiblePropertyExtensions
    {
        /// <summary>
        /// Disconnects the property, throwing an exception if the property was already disconnected.
        /// </summary>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        public static void Disconnect<TValue>(this IConnectibleProperty<TValue> property)
        {
            Contract.Requires(property != null);
            if (!property.TryDisconnect())
                throw new InvalidOperationException("Connectible property was already disconnected.");
        }

        /// <summary>
        /// Gets the value of the property, throwing an exception if the property was disconnected.
        /// </summary>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <returns>The value of the property.</returns>
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
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <param name="value">The value to set.</param>
        public static void Connect<TValue>(this IConnectibleProperty<TValue> property, TValue value)
        {
            Contract.Requires(property != null);
            if (!property.TryConnect(value))
                throw new InvalidOperationException("Connectible property was already connected.");
        }

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <param name="property">The connectible property. Must not be <c>null</c>.</param>
        /// <param name="connectValue">The new value of the property, if it is disconnected.</param>
        /// <returns>The value of the property.</returns>
        public static TValue GetOrConnect<TValue>(this IConnectibleProperty<TValue> property, TValue connectValue)
        {
            Contract.Requires(property != null);
            return property.GetOrConnect(() => connectValue);
        }
    }
}
