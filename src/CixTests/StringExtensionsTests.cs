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
            string a = "Hello";
            string[] b = new string[] { "Hello", "World" };

            Assert.IsTrue(a.IsOneOfString(b));
        }
    }
}