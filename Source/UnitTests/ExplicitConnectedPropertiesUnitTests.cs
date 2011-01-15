// <copyright file="ExplicitConnectedPropertiesUnitTests.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.ConnectedProperties;

namespace UnitTests
{
    [TestClass]
    public class ExplicitConnectedPropertiesUnitTests
    {
        [TestMethod]
        public void ConnectedPropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Set(13);
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect(17));
            Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void ConnectedPropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Set(13);
            propertyDefinition.GetProperty(carrier).Disconnect();
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryDisconnect());
        }

        [TestMethod]
        public void ConnectedPropertyMayBeRead()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Set(13);
            Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrConnectConnectsIfNecessary()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).GetOrConnect(13));
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect(17));
            Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteIfConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Set(17);
            Assert.AreEqual(17, propertyDefinition.GetProperty(carrier).GetOrConnect(13));
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect(19));
        }

        [TestMethod]
        public void SetOverwritesIfConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Set(17);
            propertyDefinition.GetProperty(carrier).Set(13);
            Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void DifferentConnectedPropertiesAreIndependent()
        {
            object carrier = new object();
            var propertyDefinition1 = new PropertyConnector<object, int>();
            var propertyDefinition2 = new PropertyConnector<object, int>();
            propertyDefinition1.GetProperty(carrier).Set(17);
            propertyDefinition2.GetProperty(carrier).Set(13);
            Assert.AreEqual(17, propertyDefinition1.GetProperty(carrier).Get());
            Assert.AreEqual(13, propertyDefinition2.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAreInitiallyDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterSetAreConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            propertyDefinition.GetProperty(carrier).Set("A");
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect("B"));
            Assert.AreEqual("A", propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void ConnectedReferencePropertiesAfterDisconnectAreDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            propertyDefinition.GetProperty(carrier).Set("A");
            propertyDefinition.GetProperty(carrier).Disconnect();
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryDisconnect());
        }

        [TestMethod]
        public void ConnectedReferencePropertyMayBeRead()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            propertyDefinition.GetProperty(carrier).Set("A");
            Assert.AreEqual("A", propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrConnectConnectsReferenceIfNecessary()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            Assert.AreEqual("A", propertyDefinition.GetProperty(carrier).GetOrConnect("A"));
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect("B"));
            Assert.AreEqual("A", propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrConnectDoesNotOverwriteReferenceIfConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            propertyDefinition.GetProperty(carrier).Set("B");
            Assert.AreEqual("B", propertyDefinition.GetProperty(carrier).GetOrConnect("A"));
            Assert.IsFalse(propertyDefinition.GetProperty(carrier).TryConnect("C"));
        }

        [TestMethod]
        public void SetOverwritesReferenceIfConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, string>();
            propertyDefinition.GetProperty(carrier).Set("B");
            propertyDefinition.GetProperty(carrier).Set("A");
            Assert.AreEqual("A", propertyDefinition.GetProperty(carrier).Get());
        }

        [TestMethod]
        public void DifferentConnectedReferencePropertiesAreIndependent()
        {
            object carrier = new object();
            var propertyDefinition1 = new PropertyConnector<object, string>();
            var propertyDefinition2 = new PropertyConnector<object, string>();
            propertyDefinition1.GetProperty(carrier).Set("A");
            propertyDefinition2.GetProperty(carrier).Set("B");
            Assert.AreEqual("A", propertyDefinition1.GetProperty(carrier).Get());
            Assert.AreEqual("B", propertyDefinition2.GetProperty(carrier).Get());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConnectWillThrowIfConnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Connect(13);
            propertyDefinition.GetProperty(carrier).Connect(17);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisconnectWillThrowIfDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Disconnect();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWillThrowIfDisconnected()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier).Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueTypesCannotBeCarrierObjects()
        {
            int carrier = 13;
            var propertyDefinition = new PropertyConnector<object, int>();
            propertyDefinition.GetProperty(carrier);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StringsCannotBeCarrierObjects()
        {
            string carrier = "Hi";
            var propertyDefinition = new PropertyConnector<string, int>();
            propertyDefinition.GetProperty(carrier);
        }

        [TestMethod]
        public void ValueTypesCannotBeCarrierObjects_Try()
        {
            int carrier = 13;
            var propertyDefinition = new PropertyConnector<object, int>();
            Assert.IsNull(propertyDefinition.TryGetProperty(carrier));
        }

        [TestMethod]
        public void StringsCannotBeCarrierObjects_Try()
        {
            string carrier = "Hi";
            var propertyDefinition = new PropertyConnector<string, int>();
            Assert.IsNull(propertyDefinition.TryGetProperty(carrier));
        }

        [TestMethod]
        public void ObjectCanBeCarrierObjects_Try()
        {
            object carrier = new object();
            var propertyDefinition = new PropertyConnector<object, int>();
            Assert.IsNotNull(propertyDefinition.TryGetProperty(carrier));
        }
    }
}
