// <copyright file="NamedConnectedPropertiesUnitTests.cs" company="Nito Programs">
//     Copyright (c) 2012 Nito Programs.
// </copyright>

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Implicit;
using Nito.ConnectedProperties.Named;

namespace UnitTests
{
    [TestClass]
    public class NamedConnectedPropertiesUnitTests
    {
        private struct tag1 { };
        private struct tag2 { };

        [TestMethod]
        public void ConnectedPropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(13);
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect(17));
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(13);
            carrier.GetConnectedProperty("Name").Disconnect();
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(13);
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void GetOrConnectConnectsIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").GetOrConnect(13));
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect(17));
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(17);
            Assert.AreEqual(17, carrier.GetConnectedProperty("Name").GetOrConnect(13));
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect(19));
        }

        [TestMethod]
        public void SetOverwritesIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(17);
            carrier.GetConnectedProperty("Name").Set(13);
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void DifferentConnectedPropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set(17);
            carrier.GetConnectedProperty("name").Set(13);
            Assert.AreEqual(17, carrier.GetConnectedProperty("Name").Get());
            Assert.AreEqual(13, carrier.GetConnectedProperty("name").Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("A");
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect("B"));
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("A");
            carrier.GetConnectedProperty("Name").Disconnect();
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("A");
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void GetOrConnectConnectesReferenceIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").GetOrConnect("A"));
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect("B"));
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteReferenceIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("B");
            Assert.AreEqual("B", carrier.GetConnectedProperty("Name").GetOrConnect("A"));
            Assert.IsFalse(carrier.GetConnectedProperty("Name").TryConnect("C"));
        }

        [TestMethod]
        public void SetOverwritesReferenceIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("B");
            carrier.GetConnectedProperty("Name").Set("A");
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").Get());
        }

        [TestMethod]
        public void DifferentConnectedReferencePropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Set("A");
            carrier.GetConnectedProperty("name").Set("B");
            Assert.AreEqual("A", carrier.GetConnectedProperty("Name").Get());
            Assert.AreEqual("B", carrier.GetConnectedProperty("name").Get());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConnectWillThrowIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Connect(13);
            carrier.GetConnectedProperty("Name").Connect(17);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisconnectWillThrowIfDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Disconnect();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWillThrowIfDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty("Name").Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueTypesCannotBeCarrierObjects()
        {
            int carrier = 13;
            carrier.GetConnectedProperty("Name");
        }

        [TestMethod]
        public void ValueTypesWithoutValidationCanBeCarrierObjects()
        {
            // Please note: this is an extremely dangerous example! Do not use in real-world code!
            int carrier = 13;
            carrier.GetConnectedProperty("Name", true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StringsCannotBeCarrierObjects()
        {
            string carrier = "Hi";
            carrier.GetConnectedProperty("Name");
        }

        [TestMethod]
        public void StringsWithoutValidationCanBeCarrierObjects()
        {
            // Please note: this is a highly dangerous example! Do not use in real-world code unless you know for-sure what you're doing!
            string carrier = "Hi";
            carrier.GetConnectedProperty("Name", true);
        }

        [TestMethod]
        public void ValueTypesCannotBeCarrierObjects_Try()
        {
            int carrier = 13;
            Assert.IsNull(carrier.TryGetConnectedProperty("Name"));
        }

        [TestMethod]
        public void ValueTypesWithoutValidationCanBeCarrierObjects_Try()
        {
            // Please note: this is an extremely dangerous example! Do not use in real-world code!
            int carrier = 13;
            Assert.IsNotNull(carrier.TryGetConnectedProperty("Name", true));
        }

        [TestMethod]
        public void StringsWithoutValidationCanBeCarrierObjects_Try()
        {
            // Please note: this is a highly dangerous example! Do not use in real-world code unless you know for-sure what you're doing!
            string carrier = "Hi";
            Assert.IsNotNull(carrier.TryGetConnectedProperty("Name", true));
        }

        [TestMethod]
        public void ObjectCanBeCarrierObjects_Try()
        {
            object carrier = new object();
            Assert.IsNotNull(carrier.TryGetConnectedProperty("Name"));
        }

        [TestMethod]
        public void PropertiesInDifferentNamespacesAreIndependent()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<tag1>("Name").Set(17);
            carrier.GetConnectedProperty<tag2>("Name").Set(13);
            Assert.AreEqual(17, carrier.GetConnectedProperty<tag1>("Name").Get());
            Assert.AreEqual(13, carrier.GetConnectedProperty<tag2>("Name").Get());
        }

        [TestMethod]
        public void PropertiesInNamespacesAreIndependentFromPropertiesWithoutNamespaces()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<tag1>("Name").Set(17);
            carrier.GetConnectedProperty("Name").Set(13);
            Assert.AreEqual(17, carrier.GetConnectedProperty<tag1>("Name").Get());
            Assert.AreEqual(13, carrier.GetConnectedProperty("Name").Get());
        }
    }
}
