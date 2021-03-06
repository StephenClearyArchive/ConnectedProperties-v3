﻿// Copyright (c) 2013 Nito Programs.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Nito.ConnectedProperties.Internal.PlatformEnlightenment
{
    /// <summary>
    /// Provides static members to access enlightenments.
    /// </summary>
    public static class Enlightenment
    {
        /// <summary>
        /// Cached instance of the platform enlightenment provider. This is an instance of the default enlightenment provider if the platform couldn't be found.
        /// </summary>
        private static IEnlightenmentProvider _platform;

        /// <summary>
        /// Cached instance of the concurrent dictionary enlightenment provider.
        /// </summary>
        private static IConcurrentDictionaryEnlightenment _concurrentDictionary;

        /// <summary>
        /// Loads the <c>PlatformEnlightenmentProvider</c> if it can be found; otherwise, returns an instance of <see cref="DefaultEnlightenmentProvider"/>.
        /// </summary>
        private static IEnlightenmentProvider CreateProvider()
        {
            Contract.Ensures(Contract.Result<IEnlightenmentProvider>() != null);
            var enlightenmentAssemblyName = new AssemblyName(typeof(IEnlightenmentProvider).Assembly.FullName)
            {
                Name = "ConnectedProperties.Enlightenment",
            };
            var enlightenmentProviderType = Type.GetType("Nito.ConnectedProperties.Internal.PlatformEnlightenment.EnlightenmentProvider, " + enlightenmentAssemblyName.FullName, false);
            if (enlightenmentProviderType == null)
                return new DefaultEnlightenmentProvider();
            return (IEnlightenmentProvider)Activator.CreateInstance(enlightenmentProviderType);
        }

        /// <summary>
        /// Returns the platform enlightenment provider, if it could be found; otherwise, returns the default enlightenment provider.
        /// </summary>
        public static IEnlightenmentProvider Platform
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnlightenmentProvider>() != null);
                if (_platform == null)
                    Interlocked.CompareExchange(ref _platform, CreateProvider(), null);
                return _platform;
            }
        }

        /// <summary>
        /// Returns the concurrent dictionary enlightenment.
        /// </summary>
        public static IConcurrentDictionaryEnlightenment ConcurrentDictionary
        {
            get
            {
                Contract.Ensures(Contract.Result<IConcurrentDictionaryEnlightenment>() != null);
                if (_concurrentDictionary == null)
                    Interlocked.CompareExchange(ref _concurrentDictionary, Platform.CreateEnlightenment<IConcurrentDictionaryEnlightenment>(), null);
                return _concurrentDictionary;
            }
        }
    }
}
