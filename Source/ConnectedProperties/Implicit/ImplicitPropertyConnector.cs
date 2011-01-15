// <copyright file="ImplicitPropertyConnector.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties.Implicit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Allows access to various connectible properties on a specific carrier object.
    /// </summary>
    public sealed class ImplicitPropertyConnector
    {
        /// <summary>
        /// The carrier object.
        /// </summary>
        private readonly object carrier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitPropertyConnector"/> class for a specific object.
        /// </summary>
        /// <param name="carrier">The carrier object. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connectible properties.</exception>
        public ImplicitPropertyConnector(object carrier)
            : this(carrier, skipCarrierVerification:false)
        {
            Contract.Requires(carrier != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitPropertyConnector"/> class for a specific object, optionally verifying that the object is a valid carrier.
        /// </summary>
        /// <param name="carrier">The carrier object. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <param name="skipCarrierVerification">Whether to skip verification of the carrier object.</param>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connectible properties.</exception>
        internal ImplicitPropertyConnector(object carrier, bool skipCarrierVerification)
        {
            Contract.Requires(carrier != null);
            if (!skipCarrierVerification)
            {
                PropertyStoreUtil.Verify(carrier);
            }
            else
            {
                Contract.Assume(PropertyStoreUtil.TryVerify(carrier));
            }

            this.carrier = carrier;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.carrier != null);
        }

        /// <summary>
        /// Gets a connectible property on this carrier object.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <returns>The connectible property.</returns>
        public IConnectibleProperty<TValue> Property<TValue, TTag>()
        {
            return Definition<TValue, TTag>.Property(this.carrier);
        }

        /// <summary>
        /// Gets a dynamic connectible property on this carrier object.
        /// </summary>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
        /// <returns>The connectible property.</returns>
        public IConnectibleProperty<dynamic> Property<TTag>()
        {
            return Definition<dynamic, TTag>.Property(this.carrier);
        }

        /// <summary>
        /// A nested static class which creates a property store for each implicit connectible property as necessary.
        /// </summary>
        /// <typeparam name="TValue">The property type.</typeparam>
        /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
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
            /// Gets a connectible property on the specified carrier object.
            /// </summary>
            /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
            /// <returns>The connectible property.</returns>
            public static IConnectibleProperty<TValue> Property(object carrier)
            {
                Contract.Requires(carrier != null);
                return new ConnectibleProperty<object, TValue>(store, carrier);
            }
        }
    }
}
