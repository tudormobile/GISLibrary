using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.GISLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass()]
    public class GeoMultiPointTests
    {
        [TestMethod]
        public void GeoMultiPointTest()
        {
            var target = new GeoMultiPoint([new(1, 2), new(3, 4)]);
            Assert.AreEqual(GeometryType.MultiPoint, target.GeometryType);
            Assert.AreEqual(2, target.Points.Length);
        }

        [TestMethod]
        public void GeoMultiPointDefaultConstructorTest()
        {
            var target = new GeoMultiPoint();
            Assert.AreEqual(GeometryType.MultiPoint, target.GeometryType);
            Assert.AreEqual(0, target.Points.Length);
        }
    }
}