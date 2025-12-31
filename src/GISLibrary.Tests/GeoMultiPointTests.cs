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
            Assert.HasCount(2, target.Points);
        }

        [TestMethod]
        public void GeoMultiPointDefaultConstructorTest()
        {
            var target = new GeoMultiPoint();
            Assert.AreEqual(GeometryType.MultiPoint, target.GeometryType);
            Assert.IsEmpty(target.Points);
        }
    }
}