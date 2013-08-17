// <copyright file="PropertyConnector.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties.Explicit
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A property connector. Allows access to a connectible property on various carrier objects.
    /// </summary>
    /// <typeparam name="TCarrier">The type of carrier objects to which the property may be connected. This must be a reference type. This may be <see cref="Object"/> to allow this property to connect to any type of object.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    [ContractClass(typeof(PropertyConnectorContracts<,>))]
    public interface IPropertyConnector<in TCarrier, TValue>
        where TCarrier : class
    {
        /// <summary>
        /// Gets a connectible property on the specified carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
        IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier);

        /// <summary>
        /// Gets a connectible property on the specified carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier, bool bypassValidation);

        /// <summary>
        /// Gets a connectible property on the specified carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
        IConnectibleProperty<TValue> GetProperty(TCarrier carrier);

        /// <summary>
        /// Gets a connectible property on the specified carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        IConnectibleProperty<TValue> GetProperty(TCarrier carrier, bool bypassValidation);
    }

    [ContractClassFor(typeof(IPropertyConnector<,>))]
    internal abstract class PropertyConnectorContracts<TCarrier, TValue> : IPropertyConnector<TCarrier, TValue>
        where TCarrier : class
    {
        public IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier)
        {
            Contract.Requires(carrier != null);
            return null;
        }

        public IConnectibleProperty<TValue> GetProperty(TCarrier carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<TValue>>() != null);
            return null;
        }

        public IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            return null;
        }

        public IConnectibleProperty<TValue> GetProperty(TCarrier carrier, bool bypassValidation)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<IConnectibleProperty<TValue>>() != null);
            return null;
        }
    }

    /// <summary>
    /// A property connector. Allows access to a connectible property on various carrier objects.
    /// </summary>
    /// <typeparam name="TCarrier">The type of carrier objects to which the property may be connected. This must be a reference type. This may be <see cref="Object"/> to allow this property to connect to any type of object.</typeparam>
    /// <typeparam name="TValue">The property type.</typeparam>
    public sealed class PropertyConnector<TCarrier, TValue> : IPropertyConnector<TCarrier, TValue>
        where TCarrier : class
    {
        /// <summary>
        /// The underlying property store.
        /// </summary>
        private readonly IPropertyStore<TCarrier, TValue> store;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConnector&lt;TCarrier, TValue&gt;"/> class.
        /// </summary>
        public PropertyConnector()
        {
            this.store = PropertyStoreUtil.Create<TCarrier, TValue>();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.store != null);
        }

        /// <summary>
        /// Gets a connectible property on the specified carrier object, returning <c>null</c> if the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="carrier"/> may not have connected properties.</returns>
        public IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier)
        {
            return this.TryGetProperty(carrier, false);
        }

        /// <summary>
        /// Gets a connectible property on the specified carrier object, optionally bypassing carrier object validation. Returns <c>null</c> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property, or <c>null</c> if <paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</returns>
        public IConnectibleProperty<TValue> TryGetProperty(TCarrier carrier, bool bypassValidation)
        {
            if (!bypassValidation && !PropertyStoreUtil.TryVerify(carrier))
                return null;
            return new ConnectibleProperty<TCarrier, TValue>(this.store, carrier);
        }

        /// <summary>
        /// Gets a connectible property on the specified carrier object, throwing <see cref="InvalidOperationException"/> if the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable. May not be <c>null</c>.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="carrier"/> may not have connected properties.</exception>
        public IConnectibleProperty<TValue> GetProperty(TCarrier carrier)
        {
            return this.GetProperty(carrier, false);
        }

        /// <summary>
        /// Gets a connectible property on the specified carrier object, optionally bypassing carrier object validation. Throws <see cref="InvalidOperationException"/> if validation is not bypassed and the specified object cannot have connected properties.
        /// </summary>
        /// <param name="carrier">The carrier object for which to retrieve the connectible property. This object must be reference-equatable unless validation is bypassed. May not be <c>null</c>.</param>
        /// <param name="bypassValidation">Whether to bypass carrier object validation. Normally, callers pass <c>true</c> for this parameter.</param>
        /// <returns>The connectible property.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="bypassValidation"/> is <c>false</c> and <paramref name="carrier"/> may not have connected properties.</exception>
        public IConnectibleProperty<TValue> GetProperty(TCarrier carrier, bool bypassValidation)
        {
            if (!bypassValidation)
                PropertyStoreUtil.Verify(carrier);
            return new ConnectibleProperty<TCarrier, TValue>(this.store, carrier);
        }
    }
}
