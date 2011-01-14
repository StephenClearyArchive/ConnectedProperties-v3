// <copyright file="AttachedPropertyDefinition.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Nito.AttachedProperties
{
    /// <summary>
    /// A single attached property definition.
    /// </summary>
    /// <typeparam name="TCarrier">The type of carrier objects to which the property may be attached. This must be a reference type. This may be <see cref="Object"/> to allow this attached property to attach to any type of object.</typeparam>
    /// <typeparam name="TValue">The attached property type.</typeparam>
    public sealed class AttachedPropertyDefinition<TCarrier, TValue>
        where TCarrier : class
    {
        /// <summary>
        /// The property store.
        /// </summary>
        private readonly IPropertyStore<TCarrier, TValue> store;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachedPropertyDefinition&lt;TCarrier, TValue&gt;"/> class.
        /// </summary>
        public AttachedPropertyDefinition()
        {
            this.store = PropertyStoreUtil.Create<TCarrier, TValue>();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.store != null);
        }

        /// <summary>
        /// Gets an accessor for this attached property on the specified carrier object, returning <c>null</c> if the specified object cannot have attached properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the attached property accessor. May not be <c>null</c>.</param>
        /// <returns>The attached property accessor, or <c>null</c> if <paramref name="carrier"/> may not have attached properties.</returns>
        public IAttachedPropertyAccessor<TValue> TryGetAttachedProperty(TCarrier carrier)
        {
            Contract.Requires(carrier != null);
            if (!PropertyStoreUtil.TryVerify(carrier))
                return null;
            return new AttachedPropertyAccessor<TCarrier, TValue>(this.store, carrier);
        }

        /// <summary>
        /// Gets an accessor for this attached property on the specified carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have attached properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the attached property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The attached property accessor.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have attached properties.</exception>
        public IAttachedPropertyAccessor<TValue> GetAttachedProperty(TCarrier carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IAttachedPropertyAccessor<TValue>>() != null);
            PropertyStoreUtil.Verify(carrier);
            return new AttachedPropertyAccessor<TCarrier, TValue>(this.store, carrier);
        }
    }
}
