using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONLineStringTests
{
    [TestMethod]
    public void CreateWithPositionsTest()
    {
        var geoJSONLineString = new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0), new GeoJSONPosition(30.0, 40.0)]);
        Assert.HasCount(2, geoJSONLineString.Positions);
    }

    [TestMethod]
    public void CreateWithPointsTest()
    {
        var geoJSONLineString = new GeoJSONLineString([new GeoJSONPoint(10.0, 20.0), new GeoJSONPoint(30.0, 40.0)]);
        Assert.HasCount(2, geoJSONLineString.Positions);
    }

    [TestMethod]
    public void CreateWithInsufficientPositions_ThrowsExceptionTest()
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0)]));
        Assert.AreEqual("A GeoJSON LineString must have at least two positions.", exception.Message);
    }

}
