using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONPointTests
{
    [TestMethod]
    public void GeoJSONPoint_ConstructWithValues()
    {
        var point = new GeoJSONPoint(10.0, 20.0, 5.0);
        Assert.AreEqual(10.0, point.Position.Latitude);
        Assert.AreEqual(20.0, point.Position.Longitude);
        Assert.AreEqual(5.0, point.Position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithPosition()
    {
        var position = new GeoJSONPosition(10.0, 20.0);
        var point = new GeoJSONPoint(position);
        Assert.AreEqual(10.0, point.Position.Latitude);
        Assert.AreEqual(20.0, point.Position.Longitude);
        Assert.IsNull(position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ImplicityCastToPosition()
    {
        var point = new GeoJSONPoint(10.0, 20.0, 5.0);
        GeoJSONPosition position = point;
        Assert.AreEqual(10.0, position.Latitude);
        Assert.AreEqual(20.0, position.Longitude);
        Assert.AreEqual(5.0, position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ImplicityCastToPositionFromNull_ReturnsDefault()
    {
        GeoJSONPoint point = null!;
        GeoJSONPosition position = point;
        Assert.AreEqual(0.0, position.Latitude);
        Assert.AreEqual(0.0, position.Longitude);
        Assert.IsNull(position.Altitude);
    }
}