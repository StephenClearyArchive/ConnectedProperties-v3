﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    partial class EnlightenmentProvider
    {
        public sealed class ConcurrentDictionaryEnlightenment : IConcurrentDictionaryEnlightenment
        {
            public IConcurrentDictionary<TKey, TValue> Create<TKey, TValue>()
            {
                return new ConcurrentDictionary<TKey, TValue>();
            }

            private sealed class ConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
            {
                private readonly System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> _dictionary = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>();

                public TValue GetOrAdd(TKey key, Func<TValue> createCallback)
                {
                    return _dictionary.GetOrAdd(key, _ => createCallback());
                }

                public bool TryAdd(TKey key, TValue value)
                {
                    return _dictionary.TryAdd(key, value);
                }

                public bool TryRemove(TKey key)
                {
                    TValue value;
                    return _dictionary.TryRemove(key, out value);
                }

                public bool TryGet(TKey key, out TValue value)
                {
                    return _dictionary.TryGetValue(key, out value);
                }
            }
        }
    }
}