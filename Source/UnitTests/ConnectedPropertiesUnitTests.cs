// Copyright (c) 2011-2013 Nito Programs.

using System;
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
    }
}
