// <copyright file="ObjectExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011-2012 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties.Named
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Collections.Concurrent;
    using Nito.ConnectedProperties.Implicit;

    /// <summary>
    /// Extensions to allow accessing named connected properties from any reference object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// The tag type for named properties.
        /// </summary>
        private sealed class NamedPropertyTag { }

        /// <summary>
        /// The tag type for named properties within a namespace tag.
        /// </summary>
        /// <typeparam name="T">The tag namespace.</typeparam>
        private sealed class NamedPropertyTag<T> { }

        /// <summary>
        /// Gets a named connectible property for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        public static IConnectibleProperty<dynamic> GetConnectedProperty(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            var properties = carrier.GetConnectedProperty<ConcurrentDictionary<string, object>, NamedPropertyTag>(bypassValidation).GetOrCreate(() => new ConcurrentDictionary<string, object>());
            return new DictionaryProperty<string, object>(properties, name);
        }

        /// <summary>
        /// Gets a named connectible property for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            var implicitProperty = carrier.TryGetConnectedProperty<ConcurrentDictionary<string, object>, NamedPropertyTag>(bypassValidation);
            if (implicitProperty == null)
                return null;
            return new DictionaryProperty<string, object>(implicitProperty.GetOrCreate(() => new ConcurrentDictionary<string, object>()), name);
        }

        /// <summary>
        /// Gets a named connectible property from a given namespace for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">The namespace tag.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        public static IConnectibleProperty<dynamic> GetConnectedProperty<TTag>(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            var properties = carrier.GetConnectedProperty<ConcurrentDictionary<string, object>, NamedPropertyTag<TTag>>(bypassValidation).GetOrCreate(() => new ConcurrentDictionary<string, object>());
            return new DictionaryProperty<string, object>(properties, name);
        }

        /// <summary>
        /// Gets a named connectible property from a given namespace for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">The namespace tag.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty<TTag>(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            var implicitProperty = carrier.TryGetConnectedProperty<ConcurrentDictionary<string, object>, NamedPropertyTag<TTag>>(bypassValidation);
            if (implicitProperty == null)
                return null;
            return new DictionaryProperty<string, object>(implicitProperty.GetOrCreate(() => new ConcurrentDictionary<string, object>()), name);
        }
    }
}
