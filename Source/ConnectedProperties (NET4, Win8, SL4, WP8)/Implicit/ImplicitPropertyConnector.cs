// <copyright file="ImplicitPropertyConnector.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties.Implicit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A static class which creates a property store for each implicit connectible property as necessary.
    /// </summary>
    /// <typeparam name="TValue">The property type.</typeparam>
    /// <typeparam name="TTag">A "tag" type used to distinguish different implicit connected properties.</typeparam>
    internal static class ImplicitPropertyConnector<TValue, TTag>
    {
        /// <summary>
        /// The underlying property store.
        /// </summary>
        private static readonly IPropertyStore<object, TValue> store;

        /// <summary>
        /// Initializes the <see cref="ImplicitPropertyConnector&lt;TValue, TTag&gt;"/> class, creating the underlying property store.
        /// </summary>
        static ImplicitPropertyConnector()
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
