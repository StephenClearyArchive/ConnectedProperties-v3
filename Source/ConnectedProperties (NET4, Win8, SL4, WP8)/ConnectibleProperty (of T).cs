// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// A property that may be connected to a carrier object at runtime.
    /// The property is either connected or disconnected. A disconnected property is different than a connected property value of <c>null</c>.
    /// All members are threadsafe.
    /// You can create connectible properties by calling methods on the <see cref="PropertyConnector"/> type.
    /// </summary>
    /// <typeparam name="T">The property type. This may be <c>dynamic</c>.</typeparam>
#pragma warning disable 618
    public sealed class ConnectibleProperty<T> : IConnectibleProperty<T>
#pragma warning restore 618
    {
        /// <summary>
        /// The dictionary representing the properties for our carrier object.
        /// </summary>
        private readonly IConcurrentDictionary<string, object> _dictionary;

        /// <summary>
        /// The key for this particular property.
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// Creates a connectible property with the specified properties dictionary and property key.
        /// </summary>
        /// <param name="dictionary">The dictionary representing the properties for our carrier object.</param>
        /// <param name="key">The key for this particular property.</param>
        internal ConnectibleProperty(IConcurrentDictionary<string, object> dictionary, string key)
        {
            _dictionary = dictionary;
            _key = key;
        }

        /// <summary>
        /// Attempts to disconnect the property. Returns <c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.
        /// </summary>
        public bool TryDisconnect()
        {
            return _dictionary.TryRemove(_key);
        }

        /// <summary>
        /// Gets the value of the property, if it is connected. Returns <c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.
        /// </summary>
        /// <param name="value">Receives the value of the property, if this method returns <c>true</c>.</param>
        public bool TryGet(out T value)
        {
            object val;
            var ret = _dictionary.TryGet(_key, out val);
            value = ret ? (T)val : default(T);
            return ret;
        }

        /// <summary>
        /// Sets the value of the property, if it is disconnected. Otherwise, does nothing. Returns <c>true</c> if the property value was set; <c>false</c> if the property was already connected.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public bool TryConnect(T value)
        {
            return _dictionary.TryAdd(_key, value);
        }

        /// <summary>
        /// Updates the value of the property, if the existing value matches a comparision value. Otherwise, does nothing. Returns <c>true</c> if the property value was updated; <c>false</c> if the comparision failed. The comparision is done using the default object equality comparer.
        /// </summary>
        /// <param name="newValue">The value to set.</param>
        /// <param name="comparisonValue">The value to compare to the existing value.</param>
        public bool TryUpdate(T newValue, T comparisonValue)
        {
            return _dictionary.TryUpdate(_key, newValue, comparisonValue);
        }

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value. <paramref name="createCallback"/> may be invoked multiple times.
        /// </summary>
        /// <param name="createCallback">The delegate invoked to create the value of the property, if it is disconnected. May not be <c>null</c>. If there is a multithreaded race condition, each thread's delegate may be invoked, but all values except one will be discarded.</param>
        /// <returns>The value of the property.</returns>
        public T GetOrCreate(Func<T> createCallback)
        {
            return (T)_dictionary.GetOrAdd(_key, () => createCallback());
        }

        /// <summary>
        /// Sets the value of a property, connecting it if necessary. <paramref name="createCallback"/> and <paramref name="updateCallback"/> may be invoked multiple times. Returns the new value of the property.
        /// </summary>
        /// <param name="createCallback">The delegate invoked to create the value if the property is not connected.</param>
        /// <param name="updateCallback">The delegate invoked to update the value if the property is connected.</param>
        public T CreateOrUpdate(Func<T> createCallback, Func<T, T> updateCallback)
        {
            return (T)_dictionary.AddOrUpdate(_key, () => createCallback(), oldValue => updateCallback((T)oldValue));
        }

        /// <summary>
        /// Disconnects the property, throwing an exception if the property was already disconnected.
        /// </summary>
        public void Disconnect()
        {
            if (!TryDisconnect())
                throw new InvalidOperationException("Connectible property was already disconnected.");
        }

        /// <summary>
        /// Gets the value of the property, throwing an exception if the property was disconnected.
        /// </summary>
        /// <returns>The value of the property.</returns>
        public T Get()
        {
            T ret;
            if (!TryGet(out ret))
                throw new InvalidOperationException("Connectible property is disconnected.");
            return ret;
        }

        /// <summary>
        /// Sets the value of the property, throwing an exception if the property was already connected.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Connect(T value)
        {
            if (!TryConnect(value))
                throw new InvalidOperationException("Connectible property was already connected.");
        }

        /// <summary>
        /// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
        /// </summary>
        /// <param name="connectValue">The new value of the property, if it is disconnected.</param>
        /// <returns>The value of the property.</returns>
        public T GetOrConnect(T connectValue)
        {
            return GetOrCreate(() => connectValue);
        }

        /// <summary>
        /// Connects a new value or updates the existing value of the property. <paramref name="updateCallback"/> may be invoked multiple times. Returns the new value of the property.
        /// </summary>
        /// <param name="connectValue">The value to set if the property is not connected.</param>
        /// <param name="updateCallback">The delegate invoked to update the value if the property is connected.</param>
        public T ConnectOrUpdate(T connectValue, Func<T, T> updateCallback)
        {
            return CreateOrUpdate(() => connectValue, updateCallback);
        }

        /// <summary>
        /// Sets the value of the property, overwriting any existing value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(T value)
        {
            CreateOrUpdate(() => value, _ => value);
        }

        /// <summary>
        /// Creates a new instance that casts the property value to a specified type.
        /// </summary>
        /// <typeparam name="TResult">The type of the property value.</typeparam>
        public ConnectibleProperty<TResult> Cast<TResult>()
        {
            return new ConnectibleProperty<TResult>(_dictionary, _key);
        }

        /// <summary>
        /// Creates a new instance that casts the property value from a specified type.
        /// </summary>
        /// <typeparam name="TSource">The original type of the property value.</typeparam>
        /// <param name="source">The source instance. This may be <c>null</c>.</param>
        public static ConnectibleProperty<T> TryCastFrom<TSource>(ConnectibleProperty<TSource> source)
        {
            if (source == null)
                return null;
            return source.Cast<T>();
        }
    }
}
