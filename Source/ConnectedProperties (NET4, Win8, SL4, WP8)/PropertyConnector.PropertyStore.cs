// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
    partial class PropertyConnector
    {
        /// <summary>
        /// A wrapper around <c>ConditionalWeakTable</c>, which creates concurrent dictionaries on demand for carrier objects.
        /// </summary>
        private sealed class PropertyStore
        {
            /// <summary>
            /// The underlying table.
            /// </summary>
            private readonly ConditionalWeakTable<object, IConcurrentDictionary<string, object>> _table = new ConditionalWeakTable<object, IConcurrentDictionary<string, object>>();

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_table != null);
            }

            /// <summary>
            /// Gets the concurrent dictionary for the specified carrier object, creating it if necessary. No validation is done.
            /// </summary>
            /// <param name="carrier">The carrier object.</param>
            public IConcurrentDictionary<string, object> Get(object carrier)
            {
                Contract.Ensures(Contract.Result<IConcurrentDictionary<string, object>>() != null);
                return _table.GetValue(carrier, _ => Enlightenment.ConcurrentDictionary.Create<string, object>());
            }
        }
    }
}
