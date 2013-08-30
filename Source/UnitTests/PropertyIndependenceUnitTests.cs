using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.ConnectedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class PropertyIndependenceUnitTests
    {
        private struct tag1 { };
        private struct tag2 { };

        [TestMethod]
        public void Properties_WithDifferentNames_HaveIndependentValues()
        {
            var carrier = new object();
            var name1 = Guid.NewGuid().ToString("N");
            var name2 = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get(carrier, name1).Set(17);
            PropertyConnector.Default.Get(carrier, name2).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name1).Get());
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name2).Get());
        }

        [TestMethod]
        public void Properties_WithDifferentTags_HaveIndependentValues()
        {
            var carrier = new object();
            PropertyConnector.Default.Get<tag1>(carrier).Set(17);
            PropertyConnector.Default.Get<tag2>(carrier).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier).Get());
            Assert.AreEqual(13, PropertyConnector.Default.Get<tag2>(carrier).Get());
        }

        [TestMethod]
        public void Properties_WithDifferentTagsAndSameName_HaveIndependentValues()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get<tag1>(carrier, name).Set(17);
            PropertyConnector.Default.Get<tag2>(carrier, name).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier, name).Get());
            Assert.AreEqual(13, PropertyConnector.Default.Get<tag2>(carrier, name).Get());
        }

        [TestMethod]
        public void Properties_WithSameNameButDifferentTagPresence_HaveIndependentValues()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get<tag1>(carrier, name).Set(17);
            PropertyConnector.Default.Get(carrier, name).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier, name).Get());
            Assert.AreEqual(13, PropertyConnector.Default.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Properties_WithSameTagsButDifferentNamePresence_HaveIndependentValues()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            PropertyConnector.Default.Get<tag1>(carrier, name).Set(17);
            PropertyConnector.Default.Get<tag1>(carrier).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier, name).Get());
            Assert.AreEqual(13, PropertyConnector.Default.Get<tag1>(carrier).Get());
        }

        [TestMethod]
        public void Properties_WithSameNameButDifferentConnectors_HaveIndependentValues()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var other = new PropertyConnector();
            PropertyConnector.Default.Get(carrier, name).Set(17);
            other.Get(carrier, name).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get(carrier, name).Get());
            Assert.AreEqual(13, other.Get(carrier, name).Get());
        }

        [TestMethod]
        public void Properties_WithSameTagsButDifferentConnectors_HaveIndependentValues()
        {
            var carrier = new object();
            var other = new PropertyConnector();
            PropertyConnector.Default.Get<tag1>(carrier).Set(17);
            other.Get<tag1>(carrier).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier).Get());
            Assert.AreEqual(13, other.Get<tag1>(carrier).Get());
        }

        [TestMethod]
        public void Properties_WithSameNamesAndTagsButDifferentConnectors_HaveIndependentValues()
        {
            var carrier = new object();
            var name = Guid.NewGuid().ToString("N");
            var other = new PropertyConnector();
            PropertyConnector.Default.Get<tag1>(carrier, name).Set(17);
            other.Get<tag1>(carrier, name).Set(13);
            Assert.AreEqual(17, PropertyConnector.Default.Get<tag1>(carrier, name).Get());
            Assert.AreEqual(13, other.Get<tag1>(carrier, name).Get());
        }
    }
}
