// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            IConcurrentDictionary<TKey, TValue> IConcurrentDictionaryEnlightenment.Create<TKey, TValue>()
            {
                return new ConcurrentDictionary<TKey, TValue>();
            }

            private sealed class ConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
            {
                private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

                [ContractInvariantMethod]
                private void ObjectInvariant()
                {
                    Contract.Invariant(_dictionary != null);
                }

                public TValue AddOrUpdate(TKey key, Func<TValue> createCallback, Func<TValue, TValue> updateCallback)
                {
                    while (true)
                    {
                        var addResult = DoGetOrAdd(key, createCallback);
                        if (addResult.Item2)
                            return addResult.Item1;
                        var newValue = updateCallback(addResult.Item1);
                        if (TryUpdate(key, newValue, addResult.Item1))
                            return newValue;
                    }
                }

                private Tuple<TValue, bool> DoGetOrAdd(TKey key, Func<TValue> createCallback)
                {
                    lock (_dictionary)
                    {
                        TValue ret;
                        if (_dictionary.TryGetValue(key, out ret))
                            return Tuple.Create(ret, false);
                    }
                    TValue created = createCallback();
                    lock (_dictionary)
                    {
                        TValue ret;
                        if (_dictionary.TryGetValue(key, out ret))
                            return Tuple.Create(ret, false);
                        _dictionary.Add(key, created);
                        return Tuple.Create(created, true);
                    }
                }

                public TValue GetOrAdd(TKey key, Func<TValue> createCallback)
                {
                    return DoGetOrAdd(key, createCallback).Item1;
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

                public bool TryUpdate(TKey key, TValue value, TValue comparison)
                {
                    lock (_dictionary)
                    {
                        TValue comparand;
                        if (!_dictionary.TryGetValue(key, out comparand))
                            return false;
                        if (!EqualityComparer<TValue>.Default.Equals(comparand, comparison))
                            return false;
                        _dictionary[key] = value;
                        return true;
                    }
                }

                public IEnumerable<KeyValuePair<TKey, TValue>> Snapshot()
                {
                    lock (_dictionary)
                    {
                        return _dictionary.ToArray();
                    }
                }
            }
        }
    }
}
