// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    /// <summary>
    /// The platform enlightenment provider for .NET 4.5 and Windows 8.
    /// </summary>
    public sealed partial class EnlightenmentProvider : IEnlightenmentProvider
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
