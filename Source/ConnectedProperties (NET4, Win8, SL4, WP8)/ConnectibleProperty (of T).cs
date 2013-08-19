// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
#pragma warning disable 618
    public sealed class ConnectibleProperty<T> : IConnectibleProperty<T>
#pragma warning restore 618
    {
        private readonly IConcurrentDictionary<string, object> _dictionary;
        private readonly string _key;

        public ConnectibleProperty(IConcurrentDictionary<string, object> dictionary, string key)
        {
            _dictionary = dictionary;
            _key = key;
        }

        public bool TryDisconnect()
        {
            return _dictionary.TryRemove(_key);
        }

        public bool TryGet(out T value)
        {
            object val;
            var ret = _dictionary.TryGet(_key, out val);
            value = ret ? (T)val : default(T);
            return ret;
        }

        public bool TryConnect(T value)
        {
            return _dictionary.TryAdd(_key, value);
        }

        public T GetOrCreate(Func<T> createCallback)
        {
            return (T)_dictionary.GetOrAdd(_key, () => createCallback());
        }

        /// <summary>
        /// Disconnects the property, throwing an exception if the property was already disconnected.
        /// </summary>
        /// <seealso cref="TryDisconnect"/>
        public void Disconnect()
        {
            if (!TryDisconnect())
                throw new InvalidOperationException("Connectible property was already disconnected.");
        }

        /// <summary>
        /// Gets the value of the property, throwing an exception if the property was disconnected.
        /// </summary>
        /// <returns>The value of the property.</returns>
        /// <seealso cref="TryGet"/>
        /// <seealso cref="GetOrCreate"/>
        /// <seealso cref="GetOrConnect"/>
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
        /// <seealso cref="TryConnect"/>
        /// <seealso cref="GetOrConnect"/>
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
        /// <seealso cref="Get"/>
        /// <seealso cref="GetOrCreate"/>
        public T GetOrConnect(T connectValue)
        {
            return GetOrCreate(() => connectValue);
        }

        /// <summary>
        /// Sets the value of the property, overwriting any existing value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(T value)
        {
            while (true)
            {
                if (TryConnect(value))
                    return;
                TryDisconnect();
            }
        }

        public ConnectibleProperty<TResult> Cast<TResult>()
        {
            return new ConnectibleProperty<TResult>(_dictionary, _key);
        }

        public static ConnectibleProperty<T> TryCastFrom<TSource>(ConnectibleProperty<TSource> source)
        {
            if (source == null)
                return null;
            return source.Cast<T>();
        }
    }
}
