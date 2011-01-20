using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixXmlDocumentation;

namespace DocumentationIdUnitTests
{
    [TestClass]
    public class KnownSamplesUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new DocumentationId("T:Nito.ConnectedProperties.Extensions");
            Assert.AreEqual(DocumentationId.Type.Type, obj.ObjectType);
            Assert.AreEqual("Extensions", obj.Name);
            Assert.AreEqual(0, obj.GenericParameterCount);
            Assert.IsNull(obj.Parameters);
            Assert.AreEqual(2, obj.Context.Count);
            Assert.AreEqual("Nito", obj.Context[0].Name);
            Assert.AreEqual(0, obj.Context[0].GenericParameterCount);
            Assert.AreEqual("ConnectedProperties", obj.Context[1].Name);
            Assert.AreEqual(0, obj.Context[1].GenericParameterCount);
            Assert.AreEqual("Type: Nito.ConnectedProperties.Extensions", obj.ToString());
        }

        [TestMethod]
        public void TestMethod2()
        {
            var obj = new DocumentationId("M:Nito.ConnectedProperties.Extensions.IsReferenceEquatable(System.Type)");
            Assert.AreEqual("Method: Nito.ConnectedProperties.Extensions.IsReferenceEquatable(System.Type)", obj.ToString());
        }

        [TestMethod]
        public void TestMethod3()
        {
            var obj = new DocumentationId("T:Nito.ConnectedProperties.IPropertyStore`2");
            Assert.AreEqual("Type: Nito.ConnectedProperties.IPropertyStore`2", obj.ToString());
        }

        [TestMethod]
        public void TestMethod4()
        {
            var obj = new DocumentationId("T:System.Runtime.CompilerServices.ConditionalWeakTable`2");
            Assert.AreEqual("Type: System.Runtime.CompilerServices.ConditionalWeakTable`2", obj.ToString());
        }

        [TestMethod]
        public void TestMethod5()
        {
            var obj = new DocumentationId("M:Nito.ConnectedProperties.IPropertyStore`2.Add(`0,`1)");
            Assert.AreEqual("Method: Nito.ConnectedProperties.IPropertyStore`2.Add({ GenericTypeParameter Index=0 }, { GenericTypeParameter Index=1 })", obj.ToString());
        }

        [TestMethod]
        public void TestMethod6()
        {
            var obj = new DocumentationId("M:Nito.ConnectedProperties.IPropertyStore`2.TryGetValue(`0,`1@)");
            Assert.AreEqual("Method: Nito.ConnectedProperties.IPropertyStore`2.TryGetValue({ GenericTypeParameter Index=0 }, { GenericTypeParameter Index=1 } (byref))", obj.ToString());
        }

        [TestMethod]
        public void TestMethod7()
        {
            var obj = new DocumentationId("M:Nito.ConnectedProperties.IPropertyStore`2.GetValue(`0,System.Func{`1})");
            Assert.AreEqual("Method: Nito.ConnectedProperties.IPropertyStore`2.GetValue({ GenericTypeParameter Index=0 }, System.Func<{ GenericTypeParameter Index=1 }>)", obj.ToString());
        }
    }
}
