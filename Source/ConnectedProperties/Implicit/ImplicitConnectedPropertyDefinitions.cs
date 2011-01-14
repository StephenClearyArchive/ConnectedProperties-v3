// <copyright file="ImplicitConnectedPropertyDefinitions.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Nito.ConnectedProperties.Implicit
{
    /// <summary>
    /// Provides access to implicit attached property definitions for a specific object.
    /// </summary>
    public sealed class ImplicitConnectedPropertyDefinitions
    {
        /// <summary>
        /// The carrier object for the attached properties.
        /// </summary>
        private readonly object carrier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitConnectedPropertyDefinitions"/> class for a specific object.
        /// </summary>
        /// <param name="carrier">The carrier object for the attached properties. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have attached properties.</exception>
        public ImplicitConnectedPropertyDefinitions(object carrier)
            : this(carrier, skipCarrierVerification:false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitConnectedPropertyDefinitions"/> class for a specific object, optionally verifying that the object is a valid carrier.
        /// </summary>
        /// <param name="carrier">The carrier object for the attached properties. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <param name="skipCarrierVerification">Whether to skip verification of the carrier object.</param>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have attached properties.</exception>
        internal ImplicitConnectedPropertyDefinitions(object carrier, bool skipCarrierVerification)
        {
            Contract.Requires(carrier != null);
            if (!skipCarrierVerification)
            {
                PropertyStoreUtil.Verify(carrier);
            }
            else
            {
                Contract.Assert(PropertyStoreUtil.TryVerify(carrier));
            }

            this.carrier = carrier;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.carrier != null);
        }

        /// <summary>
        /// Gets an accessor for the specified attached property on this carrier object.
        /// </summary>
        /// <typeparam name="TValue">The attached property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit attached properties.</typeparam>
        /// <returns>The attached property accessor.</returns>
        public IConnectedPropertyAccessor<TValue> Property<TValue, TTag>()
        {
            return Definition<TValue, TTag>.AttachedProperty(this.carrier);
        }

        /// <summary>
        /// A nested static class which creates a property store for each implicit attached property as necessary.
        /// </summary>
        /// <typeparam name="TValue">The attached property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit attached properties.</typeparam>
        private static class Definition<TValue, TTag>
        {
            /// <summary>
            /// The underlying property store.
            /// </summary>
            private static readonly IPropertyStore<object, TValue> store;

            /// <summary>
            /// Initializes the <see cref="Definition&lt;TValue, TTag&gt;"/> class, creating the underlying property store.
            /// </summary>
            static Definition()
            {
                store = PropertyStoreUtil.Create<object, TValue>();
            }

            /// <summary>
            /// Gets an accessor for this attached property on the specified carrier object.
            /// </summary>
            /// <param name="carrier">The carrier object for which to retrieve the attached property. This object must be reference-equatable. May not be <c>null</c>.</param>
            /// <returns>The attached property accessor.</returns>
            public static IConnectedPropertyAccessor<TValue> AttachedProperty(object carrier)
            {
                Contract.Requires(carrier != null);
                return new ConnectedPropertyAccessor<object, TValue>(store, carrier);
            }
        }
    }
}
