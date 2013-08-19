// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
    public sealed partial class PropertyConnector
    {
        private readonly PropertyStore _propertyStore;

        static PropertyConnector() { } // TODO: necessary?

        public PropertyConnector()
        {
            _propertyStore = new PropertyStore();
        }

        private static readonly PropertyConnector _default = new PropertyConnector();
        public static PropertyConnector Default { get { return _default; } }

        // TODO: CopyAll
        public ConnectibleProperty<dynamic> Get(object carrier, string name, bool bypassValidation = false)
        {
            return Get<Untagged>(carrier, name, bypassValidation);
        }

        public ConnectibleProperty<dynamic> TryGet(object carrier, string name, bool bypassValidation = false)
        {
            return TryGet<Untagged>(carrier, name, bypassValidation);
        }

        public ConnectibleProperty<dynamic> Get<TTag>(object carrier, bool bypassValidation = false)
        {
            return Get<TTag>(carrier, Unnamed, bypassValidation);
        }

        public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, bool bypassValidation = false)
        {
            return TryGet<TTag>(carrier, Unnamed, bypassValidation);
        }

        public ConnectibleProperty<dynamic> Get<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            if (!bypassValidation && !TryVerify(carrier))
                throw new InvalidOperationException("Object of type \"" + carrier.GetType() + "\" may not have connected properties. Only reference types that use reference equality may have connected properties.");
            return TryGet<TTag>(carrier, name, bypassValidation: true);
        }

        public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            if (!bypassValidation && !TryVerify(carrier))
                return null;
            var dictionary = _propertyStore.Get(carrier);
            return new ConnectibleProperty<dynamic>(dictionary, TagName<TTag>.Name + name);
        }
    }
}
