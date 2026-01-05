using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlLineStringTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var coordinates = new List<(double Latitude, double Longitude, double Altitude)>
        {
            (34.0, -118.0, 0.0),
            (35.0, -119.0, 0.0)
        };
        // Act
        var kmlLineString = new KmlLineString(coordinates);
        // Assert
        Assert.AreEqual(coordinates, kmlLineString.Coordinates);
        Assert.AreEqual(KmlGeometryType.LineString, kmlLineString.GeometryType);
    }

    [TestMethod]
    public void Construct_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var coordinates = new List<(double Latitude, double Longitude, double Altitude)>
        {
            (34.0, -118.0, 0.0),
            (35.0, -119.0, 0.0)
        };
        // Act
        var kmlLineString = new KmlLineString(coordinates) with
        {
            Coordinates = coordinates
        };
        // Assert
        Assert.AreEqual(coordinates, kmlLineString.Coordinates);
        Assert.AreEqual(KmlGeometryType.LineString, kmlLineString.GeometryType);
    }

}
