using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLatLonTests
{
    [TestMethod]
    public void Constructor_WithLatitudeLongitude_SetsProperties()
    {
        // arrange
        double latitude = 1.0;
        double longitude = 2.0;
        // act
        var pos = new GeoLatLon(latitude, longitude);
        // assert
        Assert.AreEqual(longitude, pos.Longitude);
        Assert.AreEqual(latitude, pos.Latitude);
    }
}
