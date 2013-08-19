// Copyright (c) 2011-2013 Nito Programs.

namespace Nito.ConnectedProperties.Named
{
    using System;
    using System.Diagnostics.Contracts;
    using Nito.ConnectedProperties.Implicit;
    using Nito.ConnectedProperties.Internal.PlatformEnlightenment;

    /// <summary>
    /// Extensions to allow accessing named connected properties from any reference object.
    /// </summary>
    public static class ObjectExtensions
    {
        private static readonly PropertyConnector Connector = new PropertyConnector();

        /// <summary>
        /// Gets a named connectible property for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        [Obsolete("Use ConnectibleProperty.Get(carrier, name, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> GetConnectedProperty(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return Connector.Get(carrier, name, bypassValidation);
        }

        /// <summary>
        /// Gets a named connectible property for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet(carrier, name, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            return Connector.TryGet(carrier, name, bypassValidation);
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
        [Obsolete("Use ConnectibleProperty.Get<TTag>(carrier, name, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> GetConnectedProperty<TTag>(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return Connector.Get<TTag>(carrier, name, bypassValidation);
        }

        /// <summary>
        /// Gets a named connectible property from a given namespace for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">The namespace tag.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="name">The name of the connectible property to retrieve.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Defaults to <c>false</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet<TTag>(carrier, name, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty<TTag>(this object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            return Connector.TryGet<TTag>(carrier, name, bypassValidation);
        }
    }
}
