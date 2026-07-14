using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitPositionTests
{
    [TestMethod]
    public void Constructor_SetsProperties()
    {
        var position = new FitPosition(42.84601, -73.82947, 76.6);

        Assert.AreEqual(42.84601, position.Latitude);
        Assert.AreEqual(-73.82947, position.Longitude);
        Assert.AreEqual(76.6, position.Altitude);
    }

    [TestMethod]
    public void Constructor_WithNullAltitude_LeavesAltitudeNull()
    {
        var position = new FitPosition(42.84601, -73.82947, null);

        Assert.IsNull(position.Altitude);
    }

    [TestMethod]
    public void ToGeoPosition_WithAltitude_UsesAltitude()
    {
        var position = new FitPosition(42.84601, -73.82947, 76.6);

        var geoPosition = position.ToGeoPosition();

        Assert.AreEqual(42.84601, geoPosition.Latitude);
        Assert.AreEqual(-73.82947, geoPosition.Longitude);
        Assert.AreEqual(76.6, geoPosition.Altitude);
    }

    [TestMethod]
    public void ToGeoPosition_WithNullAltitude_UsesDefaultAltitude()
    {
        var position = new FitPosition(42.84601, -73.82947, null);

        var geoPosition = position.ToGeoPosition(defaultAltitude: 12.5);

        Assert.AreEqual(12.5, geoPosition.Altitude);
    }

    [TestMethod]
    public void ToGeoPosition_WithNullAltitude_DefaultsToZero()
    {
        var position = new FitPosition(42.84601, -73.82947, null);

        var geoPosition = position.ToGeoPosition();

        Assert.AreEqual(0, geoPosition.Altitude);
    }

    [TestMethod]
    public void ToGeoLocation_DiscardsAltitude()
    {
        var position = new FitPosition(42.84601, -73.82947, 76.6);

        var geoLocation = position.ToGeoLocation();

        Assert.AreEqual(42.84601, geoLocation.Latitude);
        Assert.AreEqual(-73.82947, geoLocation.Longitude);
    }
}
