using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void IsOneOfStringTest()
        {
            const string a = "Hello";
            var b = new[] { "Hello", "World" };

            Assert.IsTrue(a.IsOneOfString(b));
        }
    }
}