// <copyright file="ExplicitAttachedPropertiesUnitTests.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AttachedProperties;

namespace UnitTests
{
    [TestClass]
    public class ExplicitAttachedPropertiesUnitTests
    {
        [TestMethod]
        public void AttachedPropertiesAreInitiallyDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryDetach());
        }

        [TestMethod]
        public void AttachedPropertiesAfterSetAreAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Set(13);
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach(17));
            Assert.AreEqual(13, propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void AttachedPropertiesAfterDetachAreDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Set(13);
            propertyDefinition.GetAttachedProperty(carrier).Detach();
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryDetach());
        }

        [TestMethod]
        public void AttachedPropertyMayBeRead()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Set(13);
            Assert.AreEqual(13, propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrAttachAttachesIfNecessary()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            Assert.AreEqual(13, propertyDefinition.GetAttachedProperty(carrier).GetOrAttach(13));
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach(17));
            Assert.AreEqual(13, propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrAttachDoesNotOverwriteIfAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Set(17);
            Assert.AreEqual(17, propertyDefinition.GetAttachedProperty(carrier).GetOrAttach(13));
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach(19));
        }

        [TestMethod]
        public void SetOverwritesIfAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Set(17);
            propertyDefinition.GetAttachedProperty(carrier).Set(13);
            Assert.AreEqual(13, propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void DifferentAttachedPropertiesAreIndependent()
        {
            object carrier = new object();
            var propertyDefinition1 = new AttachedPropertyDefinition<object, int>();
            var propertyDefinition2 = new AttachedPropertyDefinition<object, int>();
            propertyDefinition1.GetAttachedProperty(carrier).Set(17);
            propertyDefinition2.GetAttachedProperty(carrier).Set(13);
            Assert.AreEqual(17, propertyDefinition1.GetAttachedProperty(carrier).Get());
            Assert.AreEqual(13, propertyDefinition2.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAreInitiallyDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryDetach());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAfterSetAreAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            propertyDefinition.GetAttachedProperty(carrier).Set("A");
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach("B"));
            Assert.AreEqual("A", propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void AttachedReferencePropertiesAfterDetachAreDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            propertyDefinition.GetAttachedProperty(carrier).Set("A");
            propertyDefinition.GetAttachedProperty(carrier).Detach();
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryDetach());
        }

        [TestMethod]
        public void AttachedReferencePropertyMayBeRead()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            propertyDefinition.GetAttachedProperty(carrier).Set("A");
            Assert.AreEqual("A", propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrAttachAttachesReferenceIfNecessary()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            Assert.AreEqual("A", propertyDefinition.GetAttachedProperty(carrier).GetOrAttach("A"));
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach("B"));
            Assert.AreEqual("A", propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void GetOrAttachDoesNotOverwriteReferenceIfAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            propertyDefinition.GetAttachedProperty(carrier).Set("B");
            Assert.AreEqual("B", propertyDefinition.GetAttachedProperty(carrier).GetOrAttach("A"));
            Assert.IsFalse(propertyDefinition.GetAttachedProperty(carrier).TryAttach("C"));
        }

        [TestMethod]
        public void SetOverwritesReferenceIfAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, string>();
            propertyDefinition.GetAttachedProperty(carrier).Set("B");
            propertyDefinition.GetAttachedProperty(carrier).Set("A");
            Assert.AreEqual("A", propertyDefinition.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        public void DifferentAttachedReferencePropertiesAreIndependent()
        {
            object carrier = new object();
            var propertyDefinition1 = new AttachedPropertyDefinition<object, string>();
            var propertyDefinition2 = new AttachedPropertyDefinition<object, string>();
            propertyDefinition1.GetAttachedProperty(carrier).Set("A");
            propertyDefinition2.GetAttachedProperty(carrier).Set("B");
            Assert.AreEqual("A", propertyDefinition1.GetAttachedProperty(carrier).Get());
            Assert.AreEqual("B", propertyDefinition2.GetAttachedProperty(carrier).Get());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttachWillThrowIfAttached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Attach(13);
            propertyDefinition.GetAttachedProperty(carrier).Attach(17);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DetachWillThrowIfDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Detach();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWillThrowIfDetached()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier).Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueTypesCannotBeCarrierObjects()
        {
            int carrier = 13;
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            propertyDefinition.GetAttachedProperty(carrier);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StringsCannotBeCarrierObjects()
        {
            string carrier = "Hi";
            var propertyDefinition = new AttachedPropertyDefinition<string, int>();
            propertyDefinition.GetAttachedProperty(carrier);
        }

        [TestMethod]
        public void ValueTypesCannotBeCarrierObjects_Try()
        {
            int carrier = 13;
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            Assert.IsNull(propertyDefinition.TryGetAttachedProperty(carrier));
        }

        [TestMethod]
        public void StringsCannotBeCarrierObjects_Try()
        {
            string carrier = "Hi";
            var propertyDefinition = new AttachedPropertyDefinition<string, int>();
            Assert.IsNull(propertyDefinition.TryGetAttachedProperty(carrier));
        }

        [TestMethod]
        public void ObjectCanBeCarrierObjects_Try()
        {
            object carrier = new object();
            var propertyDefinition = new AttachedPropertyDefinition<object, int>();
            Assert.IsNotNull(propertyDefinition.TryGetAttachedProperty(carrier));
        }
    }
}
