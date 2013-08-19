// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties
{
    public static class ConnectibleProperty
    {
        // TODO: CopyAll
        public static ConnectibleProperty<dynamic> Get(object carrier, string name, bool bypassValidation = false)
        {
            return PropertyConnector.Default.Get(carrier, name, bypassValidation);
        }

        public static ConnectibleProperty<dynamic> TryGet(object carrier, string name, bool bypassValidation = false)
        {
            return PropertyConnector.Default.TryGet(carrier, name, bypassValidation);
        }

        public static ConnectibleProperty<dynamic> Get<TTag>(object carrier, bool bypassValidation = false)
        {
            return PropertyConnector.Default.Get<TTag>(carrier, bypassValidation);
        }

        public static ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, bool bypassValidation = false)
        {
            return PropertyConnector.Default.TryGet<TTag>(carrier, bypassValidation);
        }

        public static ConnectibleProperty<dynamic> Get<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            return PropertyConnector.Default.Get<TTag>(carrier, name, bypassValidation);
        }

        public static ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            return PropertyConnector.Default.TryGet<TTag>(carrier, name, bypassValidation);
        }
    }
}
