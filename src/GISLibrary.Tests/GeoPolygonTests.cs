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
    [TestClass]
    public class GeoPolygonTests
    {
        [TestMethod]
        public void GeoPolygonTest()
        {
            var target = new GeoPolygon([new(1, 2), new(3, 4)]);
            Assert.AreEqual(GeometryType.Polygon, target.GeometryType);
            Assert.AreEqual(2, target.Points.Length);
        }

        [TestMethod]
        public void GeoPolygonDefaultConstructorTest()
        {
            var target = new GeoPolygon();
            Assert.AreEqual(GeometryType.Polygon, target.GeometryType);
            Assert.AreEqual(0, target.Points.Length);
        }

    }
}