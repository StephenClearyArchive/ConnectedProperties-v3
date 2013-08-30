// Copyright (c) 2011-2013 Nito Programs.

using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// A collection of connectible properties that can be connected to carrier objects.
    /// </summary>
    public sealed partial class PropertyConnector
    {
        /// <summary>
        /// The underlying property store.
        /// </summary>
        private readonly PropertyStore _propertyStore;

        /// <summary>
        /// Creates a new collection of connectible properties.
        /// </summary>
        public PropertyConnector()
        {
            _propertyStore = new PropertyStore();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_propertyStore != null);
        }

        /// <summary>
        /// Gets the default collection of connectible properties.
        /// </summary>
        public static PropertyConnector Default
        {
            get
            {
                Contract.Ensures(Contract.Result<PropertyConnector>() != null);
                return _default;
            }
        }

        private static readonly PropertyConnector _default = new PropertyConnector();

        /// <summary>
        /// Gets a connectible property with the specified name. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> Get(object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<ConnectibleProperty<dynamic>>() != null);
            return Get<Untagged>(carrier, name, bypassValidation);
        }

        /// <summary>
        /// Gets a connectible property with the specified name. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> TryGet(object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Requires(name != null);
            return TryGet<Untagged>(carrier, name, bypassValidation);
        }

        /// <summary>
        /// Gets a connectible property for the specified tag type. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <typeparam name="TTag">The tag type of the property.</typeparam>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> Get<TTag>(object carrier, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<ConnectibleProperty<dynamic>>() != null);
            return Get<TTag>(carrier, Unnamed, bypassValidation);
        }

        /// <summary>
        /// Gets a connectible property for the specified tag type. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <typeparam name="TTag">The tag type of the property.</typeparam>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            return TryGet<TTag>(carrier, Unnamed, bypassValidation);
        }

        /// <summary>
        /// Gets a connectible property for the specified tag type with the specified name. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <typeparam name="TTag">The tag type of the property.</typeparam>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> Get<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<ConnectibleProperty<dynamic>>() != null);
            if (!bypassValidation && !TryVerify(carrier))
                throw ValidationException(carrier);
            return TryGet<TTag>(carrier, name, bypassValidation: true);
        }
        
        /// <summary>
        /// Gets a connectible property for the specified tag type with the specified name. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <typeparam name="TTag">The tag type of the property.</typeparam>
        /// <param name="carrier">The carrier object for this property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, string name, bool bypassValidation = false)
        {
            Contract.Requires(carrier != null);
            Contract.Requires(name != null);
            if (!bypassValidation && !TryVerify(carrier))
                return null;
            var dictionary = _propertyStore.Get(carrier);
            return new ConnectibleProperty<dynamic>(dictionary, TagName<TTag>.Name + name);
        }

        /// <summary>
        /// Copies all connectible properties in this collection from one carrier object to another. Throws <see cref="InvalidOperationException"/> if either <paramref name="carrierFrom"/> or <paramref name="carrierTo"/> is not a valid carrier object.
        /// </summary>
        /// <param name="carrierFrom">The carrier object acting as the source of connectible properties.</param>
        /// <param name="carrierTo">The carrier object acting as the destination of connectible properties.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public void CopyAll(object carrierFrom, object carrierTo, bool bypassValidation = false)
        {
            Contract.Requires(carrierFrom != null);
            Contract.Requires(carrierTo != null);
            if (!bypassValidation)
            {
                if (!TryVerify(carrierFrom))
                    throw ValidationException(carrierFrom);
                if (!TryVerify(carrierTo))
                    throw ValidationException(carrierTo);
            }
            TryCopyAll(carrierFrom, carrierTo, bypassValidation: true);
        }

        /// <summary>
        /// Copies all connectible properties in this collection from one carrier object to another. Returns <c>false</c> if either <paramref name="carrierFrom"/> or <paramref name="carrierTo"/> is not a valid carrier object.
        /// </summary>
        /// <param name="carrierFrom">The carrier object acting as the source of connectible properties.</param>
        /// <param name="carrierTo">The carrier object acting as the destination of connectible properties.</param>
        /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
        public bool TryCopyAll(object carrierFrom, object carrierTo, bool bypassValidation = false)
        {
            Contract.Requires(carrierFrom != null);
            Contract.Requires(carrierTo != null);
            if (!bypassValidation && (!TryVerify(carrierFrom) || !TryVerify(carrierTo)))
                return false;
            var properties = _propertyStore.Get(carrierFrom).Snapshot();
            var destination = _propertyStore.Get(carrierTo);
            foreach (var property in properties)
            {
                destination.AddOrUpdate(property.Key, () => property.Value, _ => property.Value);
            }
            return true;
        }

        /// <summary>
        /// Returns an <see cref="InvalidOperationException"/> indicating that <paramref name="carrier"/> is not a valid carrier object.
        /// </summary>
        /// <param name="carrier">The carrier object.</param>
        private static Exception ValidationException(object carrier)
        {
            Contract.Requires(carrier != null);
            Contract.Ensures(Contract.Result<Exception>() != null);
            return new InvalidOperationException("Object of type \"" + carrier.GetType() + "\" may not have connected properties. Only reference types that use reference equality may have connected properties.");
        }
    }
}
