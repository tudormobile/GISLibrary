using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONPositionTests
{
    [TestMethod]
    public void GeoJSONPosition_ConstructWithValues()
    {
        var position = new GeoJSONPosition(10.0, 20.0, 5.0);
        Assert.AreEqual(10.0, position.Latitude);
        Assert.AreEqual(20.0, position.Longitude);
        Assert.AreEqual(5.0, position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPosition_ConstructWithLatLonValues()
    {
        var position = new GeoJSONPosition(10.0, 20.0);
        Assert.AreEqual(10.0, position.Latitude);
        Assert.AreEqual(20.0, position.Longitude);
        Assert.IsNull(position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPosition_ConstructWithValuesCollection()
    {
        var position = new GeoJSONPosition([20.0, 10.0, 5.0]);
        Assert.AreEqual(10.0, position.Latitude);
        Assert.AreEqual(20.0, position.Longitude);
        Assert.AreEqual(5.0, position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPosition_ConstructWithValuesCollection_NoAltitude()
    {
        var position = new GeoJSONPosition([20.0, 10.0]);
        Assert.AreEqual(10.0, position.Latitude);
        Assert.AreEqual(20.0, position.Longitude);
        Assert.IsNull(position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPosition_ConstructWithValuesCollection_ExtraValues_Throws()
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONPosition([20.0, 10.0, 5.0, 100.0, 200.0]));
        Assert.AreEqual("A GeoJSON position can have at most three values: longitude, latitude, and altitude.", exception.Message);
    }

    [TestMethod]
    public void GeoJSONPosition_ConstructWithValuesCollection_InsufficientValues_Throws()
    {
        var exception = Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONPosition([20.0]));
        Assert.AreEqual("A GeoJSON position must have at least two values: longitude and latitude.", exception.Message);
    }
}
