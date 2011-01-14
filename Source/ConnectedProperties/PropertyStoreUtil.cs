// <copyright file="PropertyStoreUtil.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// Helper methods for working with the property store.
    /// </summary>
    internal static class PropertyStoreUtil
    {
        /// <summary>
        /// Verifies the carrier object: it must be a reference type that uses reference equality. Returns <c>true</c> if the carrier object may have attached properties; <c>false</c> if the carrier object may not have attached properties.
        /// </summary>
        /// <param name="carrier">The carrier object to verify.</param>
        /// <returns><c>true</c> if the carrier object may have attached properties; <c>false</c> if the carrier object may not have attached properties.</returns>
        public static bool TryVerify(object carrier)
        {
            Contract.Requires(carrier != null);
            return carrier.GetType().IsReferenceEquatable();
        }

        /// <summary>
        /// Verifies the carrier object: it must be a reference type that uses reference equality. Throws <see cref="InvalidOperationException"/> if the carrier object may not have attached properties.
        /// </summary>
        /// <param name="carrier">The carrier object to verify.</param>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have attached properties.</exception>
        public static void Verify(object carrier)
        {
            Contract.Requires(carrier != null);

            if (!TryVerify(carrier))
                throw new InvalidOperationException("Object of type \"" + carrier.GetType() + "\" may not have attached properties. Only reference types that use reference equality may have attached properties.");
        }

        /// <summary>
        /// Creates a new property store for the specified key (carrier) and value (attached property) types.
        /// </summary>
        /// <typeparam name="TKey">The type of objects to which the property may be attached. This must be a reference type.</typeparam>
        /// <typeparam name="TValue">The attached property type.</typeparam>
        /// <returns>The property store.</returns>
        public static IPropertyStore<TKey, TValue> Create<TKey, TValue>()
            where TKey : class
        {
            Contract.Ensures(Contract.Result<IPropertyStore<TKey, TValue>>() != null);

            // Use ValueTypePropertyStore (with a wrapper) for structs, nullable value types, and enumerations.
            // Use ReferenceTypePropertyStore (without a wrapper) for classes, interfaces, delegates, and arrays.
            var valueType = typeof(TValue);
            if (valueType.IsClass || valueType.IsInterface)
            {
                var storeType = typeof(PropertyStoreForReferenceTypes<,>);
                Contract.Assume(storeType.IsGenericTypeDefinition);
                Contract.Assume(storeType.GetGenericArguments().Length == 2);
                return (IPropertyStore<TKey, TValue>)Activator.CreateInstance(storeType.MakeGenericType(typeof(TKey), valueType));
            }

            return new PropertyStoreForValueTypes<TKey, TValue>();
        }
    }
}
