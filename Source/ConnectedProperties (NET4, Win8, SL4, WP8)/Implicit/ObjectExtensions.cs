// Copyright (c) 2011-2013 Nito Programs.

namespace Nito.ConnectedProperties.Implicit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Extensions to allow accessing implicit connected properties from any reference object.
    /// </summary>
    [Obsolete("Use ConnectibleProperty static methods instead.")]
    public static class ObjectExtensions
    {
        private static readonly PropertyConnector Connector = new PropertyConnector();
        
        /// <summary>
        /// Gets a connectible property for a specific carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
        [Obsolete("Use ConnectibleProperty.Get<TTag>(carrier).Cast<TValue>() instead.")]
        public static IConnectibleProperty<TValue> GetConnectedProperty<TValue, TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<TValue>>() != null);
            return carrier.GetConnectedProperty<TValue, TTag>(false);
        }

        /// <summary>
        /// Gets a connectible property for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        [Obsolete("Use ConnectibleProperty.Get<TTag>(carrier, bypassValidation).Cast<TValue>() instead.")]
        public static IConnectibleProperty<TValue> GetConnectedProperty<TValue, TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<TValue>>() != null);
            return Connector.Get<TTag>(carrier, bypassValidation).Cast<TValue>();
        }

        /// <summary>
        /// Gets a connectible property for a specific carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet<TTag>(carrier).Cast<TValue>() instead.")]
        public static IConnectibleProperty<TValue> TryGetConnectedProperty<TValue, TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            return carrier.TryGetConnectedProperty<TValue, TTag>(false);
        }

        /// <summary>
        /// Gets a connectible property for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet<TTag>(carrier, bypassValidation).Cast<TValue>() instead.")]
        public static IConnectibleProperty<TValue> TryGetConnectedProperty<TValue, TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            return ConnectibleProperty<TValue>.TryCastFrom(Connector.TryGet<TTag>(carrier, bypassValidation));
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
        [Obsolete("Use ConnectibleProperty.Get<TTag>(carrier) instead.")]
        public static IConnectibleProperty<dynamic> GetConnectedProperty<TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return carrier.GetConnectedProperty<dynamic, TTag>(false);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        [Obsolete("Use ConnectibleProperty.Get<TTag>(carrier, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> GetConnectedProperty<TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return carrier.GetConnectedProperty<dynamic, TTag>(bypassValidation);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet<TTag>(carrier) instead.")]
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty<TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            return carrier.TryGetConnectedProperty<dynamic, TTag>(false);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        [Obsolete("Use ConnectibleProperty.TryGet<TTag>(carrier, bypassValidation) instead.")]
        public static IConnectibleProperty<dynamic> TryGetConnectedProperty<TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            return carrier.TryGetConnectedProperty<dynamic, TTag>(bypassValidation);
        }
    }
}
