using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.GISLibrary;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass]
    public class GeometryObjectTests
    {
        [TestMethod]
        public void GeometryObjectUnknownTest()
        {
            var target = GeometryObject.Unknown;
            Assert.AreEqual(GeometryType.Unknown, target.GeometryType);
        }
    }
}
