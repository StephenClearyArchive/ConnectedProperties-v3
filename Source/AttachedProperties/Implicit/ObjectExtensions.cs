// <copyright file="ObjectExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Nito.AttachedProperties.Implicit
{
    /// <summary>
    /// Extensions to allow accessing implicit attached properties from any reference object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets the implicit attached property definition for a specific carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have attached properties.
        /// </summary>
        /// <typeparam name="TValue">The attached property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit attached properties.</typeparam>
        /// <param name="carrier">The carrier object for the attached properties. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The attached property accessor.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have attached properties.</exception>
        public static IAttachedPropertyAccessor<TValue> GetAttachedProperty<TValue, TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IAttachedPropertyAccessor<TValue>>() != null);
            return new ImplicitAttachedPropertyDefinitions(carrier).Property<TValue, TTag>();
        }

        /// <summary>
        /// Gets the implicit attached property definition for a specific carrier object, returning <c>null</c> if the specified object cannot have attached properties.
        /// </summary>
        /// <typeparam name="TValue">The attached property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit attached properties.</typeparam>
        /// <param name="carrier">The carrier object for the attached properties. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The attached property accessor, or <c>null</c> if <paramref name="carrier"/> may not have attached properties.</returns>
        public static IAttachedPropertyAccessor<TValue> TryGetAttachedProperty<TValue, TTag>(this object carrier)
        {
            Contract.Requires(carrier != null);
            if (!PropertyStoreUtil.TryVerify(carrier))
                return null;
            return new ImplicitAttachedPropertyDefinitions(carrier, skipCarrierVerification:true).Property<TValue, TTag>();
        }
    }
}
