using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    public sealed partial class DefaultEnlightenmentProvider
    {
        /// <summary>
        /// The default concurrent dictionary enlightenment, which uses a regular dictionary protected by a lock.
        /// </summary>
        public sealed class ConcurrentDictionaryEnlightenment : IConcurrentDictionaryEnlightenment
        {
            public IConcurrentDictionary<TKey, TValue> Create<TKey, TValue>()
            {
                return new ConcurrentDictionary<TKey, TValue>();
            }

            private sealed class ConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
            {
                private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

                public TValue GetOrAdd(TKey key, Func<TValue> createCallback)
                {
                    lock (_dictionary)
                    {
                        TValue ret;
                        if (_dictionary.TryGetValue(key, out ret))
                            return ret;
                    }
                    TValue created = createCallback();
                    lock (_dictionary)
                    {
                        TValue ret;
                        if (_dictionary.TryGetValue(key, out ret))
                            return ret;
                        _dictionary.Add(key, created);
                        return created;
                    }
                }

                public bool TryAdd(TKey key, TValue value)
                {
                    lock (_dictionary)
                    {
                        if (_dictionary.ContainsKey(key))
                            return false;
                        _dictionary.Add(key, value);
                        return true;
                    }
                }

                public bool TryRemove(TKey key)
                {
                    lock (_dictionary)
                    {
                        return _dictionary.Remove(key);
                    }
                }

                public bool TryGet(TKey key, out TValue value)
                {
                    lock (_dictionary)
                    {
                        return _dictionary.TryGetValue(key, out value);
                    }
                }
            }
        }
    }
}
