// <copyright file="ImplicitAttachedPropertiesUnitTests.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AttachedProperties;
using Nito.AttachedProperties.Implicit;

namespace UnitTests
{
    [TestClass]
    public class ImplicitConnectedPropertiesUnitTests
    {
        private struct tag1 { }
        private struct tag2 { }

        [TestMethod]
        public void AttachedPropertiesAreInitiallyDetached()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetAttachedProperty<int, tag1>().TryDetach());
        }

        [TestMethod]
        public void AttachedPropertiesAfterSetAreAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(13);
            Assert.IsFalse(carrier.GetAttachedProperty<int, tag1>().TryAttach(17));
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void AttachedPropertiesAfterDetachAreDetached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(13);
            carrier.GetAttachedProperty<int, tag1>().Detach();
            Assert.IsFalse(carrier.GetAttachedProperty<int, tag1>().TryDetach());
        }

        [TestMethod]
        public void AttachedPropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(13);
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void GetOrAttachAttachesIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag1>().GetOrAttach(13));
            Assert.IsFalse(carrier.GetAttachedProperty<int, tag1>().TryAttach(17));
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void GetOrAttachDoesNotOverwriteIfAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(17);
            Assert.AreEqual(17, carrier.GetAttachedProperty<int, tag1>().GetOrAttach(13));
            Assert.IsFalse(carrier.GetAttachedProperty<int, tag1>().TryAttach(19));
        }

        [TestMethod]
        public void SetOverwritesIfAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(17);
            carrier.GetAttachedProperty<int, tag1>().Set(13);
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag1>().Get());
        }

        [TestMethod]
        public void DifferentAttachedPropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Set(17);
            carrier.GetAttachedProperty<int, tag2>().Set(13);
            Assert.AreEqual(17, carrier.GetAttachedProperty<int, tag1>().Get());
            Assert.AreEqual(13, carrier.GetAttachedProperty<int, tag2>().Get());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAreInitiallyDetached()
        {
            object carrier = new object();
            Assert.IsFalse(carrier.GetAttachedProperty<string, tag1>().TryDetach());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAfterSetAreAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("A");
            Assert.IsFalse(carrier.GetAttachedProperty<string, tag1>().TryAttach("B"));
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAfterDetachAreDetached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("A");
            carrier.GetAttachedProperty<string, tag1>().Detach();
            Assert.IsFalse(carrier.GetAttachedProperty<string, tag1>().TryDetach());
        }

        [TestMethod]
        public void AttachedReferencePropertyMayBeRead()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("A");
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void GetOrAttachAttachesReferenceIfNecessary()
        {
            object carrier = new object();
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().GetOrAttach("A"));
            Assert.IsFalse(carrier.GetAttachedProperty<string, tag1>().TryAttach("B"));
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void GetOrAttachDoesNotOverwriteReferenceIfAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("B");
            Assert.AreEqual("B", carrier.GetAttachedProperty<string, tag1>().GetOrAttach("A"));
            Assert.IsFalse(carrier.GetAttachedProperty<string, tag1>().TryAttach("C"));
        }

        [TestMethod]
        public void SetOverwritesReferenceIfAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("B");
            carrier.GetAttachedProperty<string, tag1>().Set("A");
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().Get());
        }

        [TestMethod]
        public void DifferentAttachedReferencePropertiesAreIndependent()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<string, tag1>().Set("A");
            carrier.GetAttachedProperty<string, tag2>().Set("B");
            Assert.AreEqual("A", carrier.GetAttachedProperty<string, tag1>().Get());
            Assert.AreEqual("B", carrier.GetAttachedProperty<string, tag2>().Get());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttachWillThrowIfAttached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Attach(13);
            carrier.GetAttachedProperty<int, tag1>().Attach(17);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DetachWillThrowIfDetached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Detach();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWillThrowIfDetached()
        {
            object carrier = new object();
            carrier.GetAttachedProperty<int, tag1>().Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueTypesCannotBeCarrierObjects()
        {
            int carrier = 13;
            carrier.GetAttachedProperty<int, tag1>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StringsCannotBeCarrierObjects()
        {
            string carrier = "Hi";
            carrier.GetAttachedProperty<int, tag1>();
        }

        [TestMethod]
        public void ValueTypesCannotBeCarrierObjects_Try()
        {
            int carrier = 13;
            Assert.IsNull(carrier.TryGetAttachedProperty<int, tag1>());
        }

        [TestMethod]
        public void StringsCannotBeCarrierObjects_Try()
        {
            string carrier = "Hi";
            Assert.IsNull(carrier.TryGetAttachedProperty<int, tag1>());
        }

        [TestMethod]
        public void ObjectCanBeCarrierObjects_Try()
        {
            object carrier = new object();
            Assert.IsNotNull(carrier.TryGetAttachedProperty<int, tag1>());
        }
    }
}
