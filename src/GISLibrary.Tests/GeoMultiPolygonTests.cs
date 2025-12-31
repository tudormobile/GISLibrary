using System.Numerics;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass()]
    public class GeoMultiPolygonTests
    {
        [TestMethod]
        public void GeoMultiPolygonTest()
        {
            var v1 = new Vector2[] { new(1, 2), new(3, 4) };
            var v2 = new Vector2[1][] { v1 };

            var target = new GeoMultiPolygon(v2);
            Assert.AreEqual(GeometryType.MultiPolygon, target.GeometryType);
            Assert.HasCount(1, target.Polygons);
            Assert.HasCount(2, target.Polygons[0]);
        }

        [TestMethod]
        public void GeoMultiPolygonDefaultConstructorTest()
        {
            var target = new GeoMultiPolygon();
            Assert.AreEqual(GeometryType.MultiPolygon, target.GeometryType);
            Assert.IsEmpty(target.Polygons);
        }
    }
}