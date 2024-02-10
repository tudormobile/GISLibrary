using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.GISLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void addTest()
        {
            var target = new Class1();
            var expected = 3;
            var actual = target.add(1, 2);
            Assert.AreEqual(expected, actual);
        }
    }
}