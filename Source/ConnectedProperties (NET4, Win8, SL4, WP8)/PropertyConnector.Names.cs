// Copyright (c) 2011-2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nito.ConnectedProperties
{
    partial class PropertyConnector
    {
        private struct Untagged { }
        private static readonly string Unnamed = Guid.NewGuid().ToString("N");

        private static class TagName<TTag>
        {
            public static readonly string Name = Guid.NewGuid().ToString("N");
        }
    }
}
