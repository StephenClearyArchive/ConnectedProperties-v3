// Copyright (c) 2013 Nito Programs.

using Nito.ConnectedProperties.Internal.PlatformEnlightenment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// Verifies platform enlightenment.
    /// </summary>
    public static class EnlightenmentVerification
    {
        /// <summary>
        /// Returns a value indicating whether the correct platform enlightenment provider has been loaded.
        /// </summary>
        public static bool EnsureLoaded()
        {
            return Enlightenment.Platform is EnlightenmentProvider;
        }
    }
}
