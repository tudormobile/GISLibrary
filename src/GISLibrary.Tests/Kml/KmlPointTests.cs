using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlPointTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var latitude = 10.0;
        var longitude = 20.0;
        var altitude = 30.0;
        // Act
        var kmlPoint = new KmlPoint(latitude, longitude, altitude);
        // Assert
        Assert.AreEqual(latitude, kmlPoint.Latitude);
        Assert.AreEqual(longitude, kmlPoint.Longitude);
        Assert.AreEqual(altitude, kmlPoint.Altitude);
        Assert.AreEqual(KmlGeometryType.Point, kmlPoint.GeometryType);
    }

    [TestMethod]
    public void ConstructWith_SetsPropertiesCorrectly()
    {
        // Arrange
        var latitude = 10.0;
        var longitude = 20.0;
        var altitude = 30.0;
        // Act
        var kmlPoint = new KmlPoint(0, 0, 0) with
        {
            Latitude = latitude,
            Longitude = longitude,
            Altitude = altitude
        };
        // Assert
        Assert.AreEqual(latitude, kmlPoint.Latitude);
        Assert.AreEqual(longitude, kmlPoint.Longitude);
        Assert.AreEqual(altitude, kmlPoint.Altitude);
        Assert.AreEqual(KmlGeometryType.Point, kmlPoint.GeometryType);
    }

}