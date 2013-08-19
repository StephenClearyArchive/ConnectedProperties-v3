// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    public interface IConcurrentDictionaryEnlightenment
    {
        IConcurrentDictionary<TKey, TValue> Create<TKey, TValue>();
    }

    public interface IConcurrentDictionary<TKey, TValue>
    {
        TValue AddOrUpdate(TKey key, Func<TValue> createCallback, Func<TValue, TValue> updateCallback);
        TValue GetOrAdd(TKey key, Func<TValue> createCallback);
        bool TryAdd(TKey key, TValue value);
        bool TryRemove(TKey key);
        bool TryGet(TKey key, out TValue value);
        bool TryUpdate(TKey key, TValue value, TValue comparison);
    }
}
