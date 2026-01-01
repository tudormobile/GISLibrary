using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLatLonAltTests
{
    [TestMethod]
    public void Constructor_WithLatitudeLongitudeAltitude_SetsProperties()
    {
        // arrange
        double latitude = 1.0;
        double longitude = 2.0;
        double altitude = 3.0;

        // act
        var pos = new GeoLatLonAlt(latitude, longitude, altitude);

        // assert
        Assert.AreEqual(longitude, pos.Longitude);
        Assert.AreEqual(latitude, pos.Latitude);
        Assert.AreEqual(altitude, pos.Altitude);
    }
}
