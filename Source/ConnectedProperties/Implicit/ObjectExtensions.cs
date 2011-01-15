// <copyright file="ObjectExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Nito.ConnectedProperties.Implicit
{
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
            return new ImplicitPropertyConnector(carrier).Property<TValue, TTag>();
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
            if (!PropertyStoreUtil.TryVerify(carrier))
                return null;
            return new ImplicitPropertyConnector(carrier, skipCarrierVerification:true).Property<TValue, TTag>();
        }
    }
}
