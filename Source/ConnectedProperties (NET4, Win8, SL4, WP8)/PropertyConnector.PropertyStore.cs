// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
    partial class PropertyConnector
    {
        private sealed class PropertyStore
        {
            private readonly ConditionalWeakTable<object, IConcurrentDictionary<string, object>> Table = new ConditionalWeakTable<object, IConcurrentDictionary<string, object>>();

            public IConcurrentDictionary<string, object> Get(object carrier)
            {
                return Table.GetValue(carrier, _ => Enlightenment.ConcurrentDictionary.Create<string, object>());
            }
        }
    }
}
