// <copyright file="ObjectExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011-2012 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties.Implicit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Extensions to allow accessing implicit connected properties from any reference object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets a connectible property for a specific carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
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
        public static IConnectibleProperty<TValue> GetConnectedProperty<TValue, TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<TValue>>() != null);
            if (!bypassValidation)
                PropertyStoreUtil.Verify(carrier);
            return ImplicitPropertyConnector<TValue, TTag>.Property(carrier);
        }

        /// <summary>
        /// Gets a connectible property for a specific carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
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
        public static IConnectibleProperty<TValue> TryGetConnectedProperty<TValue, TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            if (!bypassValidation && !PropertyStoreUtil.TryVerify(carrier))
                return null;
            return ImplicitPropertyConnector<TValue, TTag>.Property(carrier);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
        public static IConnectibleProperty<object> GetConnectedPropertyAsObject<TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return carrier.GetConnectedProperty<object, TTag>(false);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        public static IConnectibleProperty<object> GetConnectedPropertyAsObject<TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<dynamic>>() != null);
            return carrier.GetConnectedProperty<object, TTag>(bypassValidation);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
        public static IConnectibleProperty<object> TryGetConnectedPropertyAsObject<TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            return carrier.TryGetConnectedProperty<object, TTag>(false);
        }

        /// <summary>
        /// Gets a dynamic connectible property for a specific carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        public static IConnectibleProperty<object> TryGetConnectedPropertyAsObject<TTag>(this object carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            return carrier.TryGetConnectedProperty<object, TTag>(bypassValidation);
        }
    }
}
