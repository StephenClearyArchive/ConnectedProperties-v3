// <copyright file="ImplicitConnectedPropertiesUnitTests.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Implicit;

namespace UnitTests
{
    [TestClass]
    public class ImplicitConnectedPropertiesUnitTests
    {
        private struct tag1 { }
        private struct tag2 { }

        [TestMethod]
        public void ConnectedPropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetConnectedProperty<int, tag1>().TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(13);
            Assert.IsFalse(carrier.GetConnectedProperty<int, tag1>().TryConnect(17));
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(13);
            carrier.GetConnectedProperty<int, tag1>().Disconnect();
            Assert.IsFalse(carrier.GetConnectedProperty<int, tag1>().TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(13);
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void GetOrConnectConnectesIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag1>().GetOrConnect(13));
            Assert.IsFalse(carrier.GetConnectedProperty<int, tag1>().TryConnect(17));
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(17);
            Assert.AreEqual(17, carrier.GetConnectedProperty<int, tag1>().GetOrConnect(13));
            Assert.IsFalse(carrier.GetConnectedProperty<int, tag1>().TryConnect(19));
        }

        [TestMethod]
        public void SetOverwritesIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(17);
            carrier.GetConnectedProperty<int, tag1>().Set(13);
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void DifferentConnectedPropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Set(17);
            carrier.GetConnectedProperty<int, tag2>().Set(13);
            Assert.AreEqual(17, carrier.GetConnectedProperty<int, tag1>().Get());
            Assert.AreEqual(13, carrier.GetConnectedProperty<int, tag2>().Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetConnectedProperty<string, tag1>().TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("A");
            Assert.IsFalse(carrier.GetConnectedProperty<string, tag1>().TryConnect("B"));
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("A");
            carrier.GetConnectedProperty<string, tag1>().Disconnect();
            Assert.IsFalse(carrier.GetConnectedProperty<string, tag1>().TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("A");
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void GetOrConnectConnectesReferenceIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().GetOrConnect("A"));
            Assert.IsFalse(carrier.GetConnectedProperty<string, tag1>().TryConnect("B"));
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteReferenceIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("B");
            Assert.AreEqual("B", carrier.GetConnectedProperty<string, tag1>().GetOrConnect("A"));
            Assert.IsFalse(carrier.GetConnectedProperty<string, tag1>().TryConnect("C"));
        }

        [TestMethod]
        public void SetOverwritesReferenceIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("B");
            carrier.GetConnectedProperty<string, tag1>().Set("A");
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void DifferentConnectedReferencePropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<string, tag1>().Set("A");
            carrier.GetConnectedProperty<string, tag2>().Set("B");
            Assert.AreEqual("A", carrier.GetConnectedProperty<string, tag1>().Get());
            Assert.AreEqual("B", carrier.GetConnectedProperty<string, tag2>().Get());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConnectWillThrowIfConnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Connect(13);
            carrier.GetConnectedProperty<int, tag1>().Connect(17);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisconnectWillThrowIfDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Disconnect();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWillThrowIfDisconnected()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<int, tag1>().Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueTypesCannotBeCarrierObjects()
        {
            int carrier = 13;
            carrier.GetConnectedProperty<int, tag1>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StringsCannotBeCarrierObjects()
        {
            string carrier = "Hi";
            carrier.GetConnectedProperty<int, tag1>();
        }

        [TestMethod]
        public void ValueTypesCannotBeCarrierObjects_Try()
        {
            int carrier = 13;
            Assert.IsNull(carrier.TryGetConnectedProperty<int, tag1>());
        }

        [TestMethod]
        public void StringsCannotBeCarrierObjects_Try()
        {
            string carrier = "Hi";
            Assert.IsNull(carrier.TryGetConnectedProperty<int, tag1>());
        }

        [TestMethod]
        public void ObjectCanBeCarrierObjects_Try()
        {
            object carrier = new object();
            Assert.IsNotNull(carrier.TryGetConnectedProperty<int, tag1>());
        }

        [TestMethod]
        public void DynamicIsSameAsObject()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<dynamic, tag1>().Set(13);
            carrier.GetConnectedProperty<object, tag1>().Set(17);
            Assert.AreEqual(17, carrier.GetConnectedProperty<dynamic, tag1>().Get());
            Assert.AreEqual(17, carrier.GetConnectedProperty<object, tag1>().Get());
        }

        [TestMethod]
        public void DynamicIsTheDefaultValueType()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<tag1>().Set(13);
            carrier.GetConnectedProperty<dynamic, tag1>().Set(17);
            Assert.AreEqual(17, carrier.GetConnectedProperty<tag1>().Get());
            Assert.AreEqual(17, carrier.GetConnectedProperty<dynamic, tag1>().Get());
        }

        [TestMethod]
        public void DynamicIsTheDefaultValueType_Try()
        {
            object carrier = new object();
            carrier.GetConnectedProperty<tag1>().Set(13);
            carrier.TryGetConnectedProperty<dynamic, tag1>().Set(17);
            Assert.AreEqual(17, carrier.TryGetConnectedProperty<tag1>().Get());
            Assert.AreEqual(17, carrier.GetConnectedProperty<dynamic, tag1>().Get());
        }
    }
}
