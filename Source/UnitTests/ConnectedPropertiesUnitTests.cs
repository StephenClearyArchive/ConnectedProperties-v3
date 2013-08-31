﻿// Copyright (c) 2011-2013 Nito Programs.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.ConnectedProperties;

namespace UnitTests
{
    [TestClass]
    public class ConnectedPropertiesUnitTests
    {
        [TestMethod]
        public void TryConnect_WhenDisconnected_ReturnsTrueAndSetsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            Assert.IsTrue(PropertyConnector.Default.Get(carrier, name).TryConnect(17));
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void TryConnect_WhenConnected_ReturnsFalseAndDoesNotSetValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryConnect(17));
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Connect_WhenDisconnected_SetsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Connect(13);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Connect_WhenConnected_ThrowsAndDoesNotModifyValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Connect(13);
            var property = PropertyConnector.Default.Get(carrier, name);
            AssertEx.Throws<InvalidOperationException>(() => property.Connect(17));
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void TryDisconnect_WhenConnected_ReturnsTrueAndDisconnects()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.IsTrue(PropertyConnector.Default.Get(carrier, name).TryDisconnect());
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryDisconnect());
        }

        [TestMethod]
        public void TryDisconnect_WhenDisconnected_ReturnsFalse()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryDisconnect());
        }

        [TestMethod]
        public void Disconnect_WhenConnected_Disconnects()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            PropertyConnector.Default.Get(carrier, name).Disconnect();
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryDisconnect());
        }

        [TestMethod]
        public void Disconnect_WhenDisconnected_Throws()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var property = PropertyConnector.Default.Get(carrier, name);
            AssertEx.Throws<InvalidOperationException>(property.Disconnect);
        }

        [TestMethod]
        public void TryGet_WhenConnected_ReturnsTrueAndValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            dynamic value;
            Assert.IsTrue(PropertyConnector.Default.Get(carrier, name).TryGet(out value));
            Assert.AreEqual(13, value);
        }

        [TestMethod]
        public void Get_WhenConnected_ReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void TryGet_WhenDisconnected_ReturnsFalse()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            dynamic value;
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryGet(out value));
        }

        [TestMethod]
        public void Get_WhenDisconnected_Throws()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var property = PropertyConnector.Default.Get(carrier, name);
            AssertEx.Throws<InvalidOperationException>(() => property.Get());
        }

        [TestMethod]
        public void GetOrConnect_WhenDisconnected_SetsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyConnector.Default.Get(carrier, name).GetOrConnect(13);
            Assert.AreEqual(13, result);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void GetOrConnect_WhenConnected_ReturnsExistingValueAndDoesNotModifyValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(17);
            var result = PropertyConnector.Default.Get(carrier, name).GetOrConnect(13);
            Assert.AreEqual(17, result);
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Set_WhenDisconnected_SetsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(17);
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Set_WhenConnected_OverwritesExistingValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(17);
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void ConnectOrUpdate_WhenDisconnected_SetsAndReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyConnector.Default.Get(carrier, name).Cast<int>().ConnectOrUpdate(13, x => x + 2);
            Assert.AreEqual(13, result);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void ConnectOrUpdate_WhenConnected_UpdatesAndReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var result = PropertyConnector.Default.Get(carrier, name).Cast<int>().ConnectOrUpdate(13, x => x + 2);
            Assert.AreEqual(15, result);
            Assert.AreEqual(15, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void TryUpdate_WhenDisconnected_ReturnsFalseAndDoesNotSetValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyConnector.Default.Get(carrier, name).TryUpdate(19, 17);
            Assert.IsFalse(result);
            Assert.IsFalse(PropertyConnector.Default.Get(carrier, name).TryDisconnect());
        }

        [TestMethod]
        public void TryUpdate_WhenConnectedWithNonmatchingValue_ReturnsFalseAndDoesNotSetValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var result = PropertyConnector.Default.Get(carrier, name).TryUpdate(19, 17);
            Assert.IsFalse(result);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void TryUpdate_WhenConnectedWithMatchingValue_ReturnsTrueAndSetsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var result = PropertyConnector.Default.Get(carrier, name).TryUpdate(19, 13);
            Assert.IsTrue(result);
            Assert.AreEqual(19, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void GetOrCreate_WhenConnected_GetsValueAndDoesNotSetValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var result = PropertyConnector.Default.Get(carrier, name).GetOrCreate(() => 17);
            Assert.AreEqual(13, result);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void GetOrCreate_WhenDisconnected_SetsAndReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyConnector.Default.Get(carrier, name).GetOrCreate(() => 17);
            Assert.AreEqual(17, result);
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void CreateOrUpdate_WhenDisconnected_SetsAndReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyConnector.Default.Get(carrier, name).CreateOrUpdate(() => 13, x => x + 2);
            Assert.AreEqual(13, result);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void CreateOrUpdate_WhenConnected_UpdatesAndReturnsValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(17);
            var result = PropertyConnector.Default.Get(carrier, name).CreateOrUpdate(() => 13, x => x + 2);
            Assert.AreEqual(19, result);
            Assert.AreEqual(19, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Cast_ReferenesSamePropertyValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Cast<int>().Get());
        }

        [TestMethod]
        public void Cast_ToInvalidType_ThrowsWhenUsed()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var property = PropertyConnector.Default.Get(carrier, name).Cast<string>();
            AssertEx.Throws<InvalidCastException>(() => property.Get());
        }

        [TestMethod]
        public void TryCastFrom_ReferencesSamePropertyValue()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.AreEqual(13, ConnectibleProperty<int>.TryCastFrom(PropertyConnector.Default.Get(carrier, name)).Get());
        }

        [TestMethod]
        public void TryCastFrom_ToInvalidType_ThrowsWhenUsed()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name).Set(13);
            var property = ConnectibleProperty<string>.TryCastFrom(PropertyConnector.Default.Get(carrier, name));
            AssertEx.Throws<InvalidCastException>(() => property.Get());
        }

        [TestMethod]
        public void TryCastFrom_WithNullSource_ReturnsNull()
        {
            ConnectibleProperty<dynamic> source = null;
            Assert.IsNull(ConnectibleProperty<string>.TryCastFrom(source));
        }

#if DEBUG
#warning Skipping some unit tests due to DEBUG; build in Release to run all unit tests.
#else
        [TestMethod]
        public void Property_WhenCarrierIsAlive_IsNotCollected()
        {
            // A "dictionary mapping" with weak value references will not pass this test.
            var name = Guid.NewGuid().ToString("N");
            var carrier = new object();
            var value = new object();
            var valueRef = new WeakReference(value);
            PropertyConnector.Default.Get(carrier, name).Set(value);
            GC.Collect();
            Assert.IsTrue(valueRef.IsAlive);
            GC.KeepAlive(carrier);
        }

        [TestMethod]
        public void Property_WhenCarrierIsCollected_IsCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var carrier = new object();
            var carrierRef = new WeakReference(carrier);
            var value = new object();
            var valueRef = new WeakReference(value);
            PropertyConnector.Default.Get(carrier, name).Set(value);
            GC.Collect();
            Assert.IsFalse(carrierRef.IsAlive);
            Assert.IsFalse(valueRef.IsAlive);
        }

        [TestMethod]
        public void Property_ReferencingCarrier_WhenCarrierIsCollected_IsCollected()
        {
            // A "dictionary mapping" with strong value references will not pass this test.
            var name = Guid.NewGuid().ToString("N");
            var carrier = new object();
            var carrierRef = new WeakReference(carrier);
            Func<int> value = carrier.GetHashCode;
            var valueRef = new WeakReference(value);
            PropertyConnector.Default.Get(carrier, name).Set(value);
            GC.Collect();
            Assert.IsFalse(carrierRef.IsAlive);
            Assert.IsFalse(valueRef.IsAlive);
        }
        
        [TestMethod]
        public void Properties_CrossReferencingCarriers_WhenCarriersAreCollected_AreCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var carrier1 = new object();
            var carrierRef1 = new WeakReference(carrier1);
            var carrier2 = new object();
            var carrierRef2 = new WeakReference(carrier1);
            Func<int> value1 = carrier2.GetHashCode;
            var valueRef1 = new WeakReference(value1);
            Func<int> value2 = carrier1.GetHashCode;
            var valueRef2 = new WeakReference(value2);
            PropertyConnector.Default.Get(carrier1, name).Set(value1);
            PropertyConnector.Default.Get(carrier2, name).Set(value2);
            GC.Collect();
            Assert.IsFalse(carrierRef1.IsAlive);
            Assert.IsFalse(valueRef1.IsAlive);
            Assert.IsFalse(carrierRef2.IsAlive);
            Assert.IsFalse(valueRef2.IsAlive);
        }

        [TestMethod]
        public void Properties_CrossReferencingCarriers_WhenOnePropertyRemainsAlive_AreNotCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var carrier1 = new object();
            var carrierRef1 = new WeakReference(carrier1);
            var carrier2 = new object();
            var carrierRef2 = new WeakReference(carrier1);
            Func<int> value1 = carrier2.GetHashCode;
            var valueRef1 = new WeakReference(value1);
            Func<int> value2 = carrier1.GetHashCode;
            var valueRef2 = new WeakReference(value2);
            PropertyConnector.Default.Get(carrier1, name).Set(value1);
            PropertyConnector.Default.Get(carrier2, name).Set(value2);
            GC.Collect();
            Assert.IsTrue(carrierRef1.IsAlive);
            Assert.IsTrue(valueRef1.IsAlive);
            Assert.IsTrue(carrierRef2.IsAlive);
            Assert.IsTrue(valueRef2.IsAlive);
            GC.KeepAlive(value1);
        }

        [TestMethod]
        public void Properties_CrossReferencingCarriersAcrossConnectors_WhenCarriersAreCollected_AreCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var other = new PropertyConnector();
            var carrier1 = new object();
            var carrierRef1 = new WeakReference(carrier1);
            var carrier2 = new object();
            var carrierRef2 = new WeakReference(carrier1);
            Func<int> value1 = carrier2.GetHashCode;
            var valueRef1 = new WeakReference(value1);
            Func<int> value2 = carrier1.GetHashCode;
            var valueRef2 = new WeakReference(value2);
            PropertyConnector.Default.Get(carrier1, name).Set(value1);
            other.Get(carrier2, name).Set(value2);
            GC.Collect();
            Assert.IsFalse(carrierRef1.IsAlive);
            Assert.IsFalse(valueRef1.IsAlive);
            Assert.IsFalse(carrierRef2.IsAlive);
            Assert.IsFalse(valueRef2.IsAlive);
        }

        [TestMethod]
        public void Properties_CrossReferencingCarriersAcrossConnectors_WhenOnePropertyRemainsAlive_AreNotCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var other = new PropertyConnector();
            var carrier1 = new object();
            var carrierRef1 = new WeakReference(carrier1);
            var carrier2 = new object();
            var carrierRef2 = new WeakReference(carrier1);
            Func<int> value1 = carrier2.GetHashCode;
            var valueRef1 = new WeakReference(value1);
            Func<int> value2 = carrier1.GetHashCode;
            var valueRef2 = new WeakReference(value2);
            PropertyConnector.Default.Get(carrier1, name).Set(value1);
            other.Get(carrier2, name).Set(value2);
            GC.Collect();
            Assert.IsTrue(carrierRef1.IsAlive);
            Assert.IsTrue(valueRef1.IsAlive);
            Assert.IsTrue(carrierRef2.IsAlive);
            Assert.IsTrue(valueRef2.IsAlive);
            GC.KeepAlive(value1);
        }

        [TestMethod]
        public void Property_WhenConnectorIsCollected_IsCollected()
        {
            var name = Guid.NewGuid().ToString("N");
            var connector = new PropertyConnector();
            var carrier = new object();
            var value = new object();
            var valueRef = new WeakReference(value);
            connector.Get(carrier, name).Set(value);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.IsFalse(valueRef.IsAlive);
            GC.KeepAlive(carrier);
        }

        [TestMethod]
        public void Property_ReferencingConnector_ActsAsStrongReference()
        {
            var name = Guid.NewGuid().ToString("N");
            var connector = new PropertyConnector();
            var connectorRef = new WeakReference(connector);
            var carrier = new object();
            Func<int> value = connector.GetHashCode;
            var valueRef = new WeakReference(value);
            connector.Get(carrier, name).Set(value);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.IsTrue(valueRef.IsAlive);
            Assert.IsTrue(connectorRef.IsAlive);
            GC.KeepAlive(carrier);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.IsFalse(valueRef.IsAlive);
            Assert.IsFalse(connectorRef.IsAlive);
        }
#endif
    }
}
