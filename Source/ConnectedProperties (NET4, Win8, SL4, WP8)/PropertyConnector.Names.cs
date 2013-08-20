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
        /// <summary>
        /// The tag type for properties without a specified tag.
        /// </summary>
        private struct Untagged { }

        /// <summary>
        /// The name for properties without a specified name.
        /// </summary>
        private static readonly string Unnamed = Guid.NewGuid().ToString("N");

        /// <summary>
        /// A type that generates unique names for types.
        /// </summary>
        /// <typeparam name="TTag">The tag type.</typeparam>
        private static class TagName<TTag>
        {
            /// <summary>
            /// The name for the specified tag type.
            /// </summary>
            public static readonly string Name = Guid.NewGuid().ToString("N");
        }
    }
}
