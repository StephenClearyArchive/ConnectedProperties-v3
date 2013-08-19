// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    /// <summary>
    /// The default enlightenment provider, used when the platform enlightenment provider could not be found.
    /// </summary>
    public sealed partial class DefaultEnlightenmentProvider : IEnlightenmentProvider
    {
        T IEnlightenmentProvider.CreateEnlightenment<T>()
        {
            var type = typeof(T);
            if (type == typeof(IConcurrentDictionaryEnlightenment))
                return (T)(object)new ConcurrentDictionaryEnlightenment();

            throw new NotImplementedException();
        }
    }
}
