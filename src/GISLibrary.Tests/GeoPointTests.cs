using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.GISLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass]
    public class GeoPointTests
    {
        [TestMethod]
        public void GeoPointTest()
        {
            var x = 1.234;
            var y = 5.678;
            var target = new GeoPoint(x, y);
            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
            Assert.AreEqual(GeometryType.Point, target.GeometryType);
        }

        [TestMethod]
        public void GeoPointDefaultConstructorTest()
        {
            var target = new GeoPoint();
            Assert.AreEqual(0, target.X);
            Assert.AreEqual(0, target.Y);
            Assert.AreEqual(GeometryType.Point, target.GeometryType);
        }
    }
}