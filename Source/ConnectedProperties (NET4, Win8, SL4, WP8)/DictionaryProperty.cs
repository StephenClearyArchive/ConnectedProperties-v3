using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Nito.ConnectedProperties.Internal.PlatformEnlightenment;

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// A connected property that represents a value in a thread-safe dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the dictionary.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    public sealed class DictionaryProperty<TKey, TValue> : IConnectibleProperty<TValue>
    {
        /// <summary>
        /// The dictionary containing property values.
        /// </summary>
        private readonly IConcurrentDictionary<TKey, TValue> properties;

        /// <summary>
        /// The key for this property.
        /// </summary>
        private readonly TKey key;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryProperty&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="properties">The dictionary containing property values. May not be <c>null</c>.</param>
        /// <param name="key">The key for this property.</param>
        public DictionaryProperty(IConcurrentDictionary<TKey, TValue> properties, TKey key)
        {
            Contract.Requires(properties != null);
            this.properties = properties;
            this.key = key;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.properties != null);
        }

        TValue IConnectibleProperty<TValue>.GetOrCreate(Func<TValue> createCallback)
        {
            return this.properties.GetOrAdd(this.key, createCallback);
        }

        bool IConnectibleProperty<TValue>.TryConnect(TValue value)
        {
            return this.properties.TryAdd(this.key, value);
        }

        bool IConnectibleProperty<TValue>.TryDisconnect()
        {
            return this.properties.TryRemove(this.key);
        }

        bool IConnectibleProperty<TValue>.TryGet(out TValue value)
        {
            return this.properties.TryGet(this.key, out value);
        }
    }
}
