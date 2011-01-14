// <copyright file="AttachedPropertyAccessorExtensions.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Nito.AttachedProperties
{
    /// <summary>
    /// Provides convenience methods for attached property accessors.
    /// </summary>
    public static class AttachedPropertyAccessorExtensions
    {
        /// <summary>
        /// Detaches the attached property, throwing an exception if the attached property was already detached.
        /// </summary>
        /// <param name="property">The attached property. Must not be <c>null</c>.</param>
        public static void Detach<TValue>(this IAttachedPropertyAccessor<TValue> property)
        {
            Contract.Requires(property != null);
            if (!property.TryDetach())
                throw new InvalidOperationException("Attached property was already detached.");
        }

        /// <summary>
        /// Gets the value of the attached property, throwing an exception if the attached property was detached.
        /// </summary>
        /// <param name="property">The attached property. Must not be <c>null</c>.</param>
        /// <returns>The value of the attached property.</returns>
        public static TValue Get<TValue>(this IAttachedPropertyAccessor<TValue> property)
        {
            Contract.Requires(property != null);
            TValue ret;
            if (!property.TryGet(out ret))
                throw new InvalidOperationException("Attached property is detached.");
            return ret;
        }

        /// <summary>
        /// Sets the value of the attached property, throwing an exception if the attached property was already attached.
        /// </summary>
        /// <param name="property">The attached property. Must not be <c>null</c>.</param>
        /// <param name="value">The value to set.</param>
        public static void Attach<TValue>(this IAttachedPropertyAccessor<TValue> property, TValue value)
        {
            Contract.Requires(property != null);
            if (!property.TryAttach(value))
                throw new InvalidOperationException("Attached property was already attached.");
        }

        /// <summary>
        /// Gets the value of the attached property if it is attached; otherwise, sets the value of the attached property and returns the new value.
        /// </summary>
        /// <param name="property">The attached property. Must not be <c>null</c>.</param>
        /// <param name="attachValue">The new value of the attached property, if it is detached.</param>
        /// <returns>The value of the attached property.</returns>
        public static TValue GetOrAttach<TValue>(this IAttachedPropertyAccessor<TValue> property, TValue attachValue)
        {
            Contract.Requires(property != null);
            return property.GetOrAttach(() => attachValue);
        }
    }
}
