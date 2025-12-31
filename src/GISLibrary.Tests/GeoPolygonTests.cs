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
            Assert.HasCount(2, target.Points);
        }

        [TestMethod]
        public void GeoPolygonDefaultConstructorTest()
        {
            var target = new GeoPolygon();
            Assert.AreEqual(GeometryType.Polygon, target.GeometryType);
            Assert.IsEmpty(target.Points);
        }

    }
}