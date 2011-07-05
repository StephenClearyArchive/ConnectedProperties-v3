using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace IntegrationTests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Net4Client_WithCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\Net 4 Client CC\bin\Debug\Net 4 Client CC.exe");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }

        [TestMethod]
        public void Net4Client_WithoutCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\Net 4 Client NoCC\bin\Debug\Net 4 Client NoCC.exe");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }

        [TestMethod]
        public void Net4Full_WithCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\Net 4 Full CC\bin\Debug\Net 4 Full CC.exe");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }

        [TestMethod]
        public void Net4Full_WithoutCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\Net 4 Full NoCC\bin\Debug\Net 4 Full NoCC.exe");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }

        [TestMethod]
        public void SL4_WithCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\SL 4 CC\bin\Debug\SL 4 CC.dll");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }

        [TestMethod]
        public void SL4_WithoutCC_HasCCExceptionType()
        {
            var assembly = Assembly.LoadFrom(@"..\..\..\SL 4 NoCC\bin\Debug\SL 4 NoCC.dll");
            var method = assembly.GetType("IntegrationTest.Program").GetMethod("PreconditionViolationExceptionType", BindingFlags.Static | BindingFlags.Public);
            string result = (string)method.Invoke(null, null);
            Assert.IsTrue(result.Contains("System.Diagnostics.Contracts"));
        }
    }
}
