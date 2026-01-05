using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlPolygonTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var innerBoundaries = new List<List<(double Latitude, double Longitude, double Altitude)>>
        {
            new List<(double Latitude, double Longitude, double Altitude)>
            {
                (34.0, -118.0, 0.0),
                (34.1, -118.1, 0.0),
                (34.2, -118.2, 0.0),
                (34.0, -118.0, 0.0)
            }
        };
        var outerBoundary = new List<(double Latitude, double Longitude, double Altitude)>
        {
            (33.0, -117.0, 0.0),
            (33.1, -117.1, 0.0),
            (33.2, -117.2, 0.0),
            (33.0, -117.0, 0.0)
        };
        // Act
        var kmlPolygon = new KmlPolygon(outerBoundary, innerBoundaries);
        // Assert
        Assert.AreEqual(outerBoundary, kmlPolygon.OuterBoundary);
        Assert.AreEqual(innerBoundaries, kmlPolygon.InnerBoundaries);
        Assert.AreEqual(KmlGeometryType.Polygon, kmlPolygon.GeometryType);
    }

    [TestMethod]
    public void Construct_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var innerBoundaries = new List<List<(double Latitude, double Longitude, double Altitude)>>
        {
            new List<(double Latitude, double Longitude, double Altitude)>
            {
                (34.0, -118.0, 0.0),
                (34.1, -118.1, 0.0),
                (34.2, -118.2, 0.0),
                (34.0, -118.0, 0.0)
            }
        };
        var outerBoundary = new List<(double Latitude, double Longitude, double Altitude)>
        {
            (33.0, -117.0, 0.0),
            (33.1, -117.1, 0.0),
            (33.2, -117.2, 0.0),
            (33.0, -117.0, 0.0)
        };
        // Act
        var kmlPolygon = new KmlPolygon(outerBoundary, innerBoundaries) with
        {
            OuterBoundary = outerBoundary,
            InnerBoundaries = innerBoundaries
        }
        ;

        // Assert
        Assert.AreEqual(outerBoundary, kmlPolygon.OuterBoundary);
        Assert.AreEqual(innerBoundaries, kmlPolygon.InnerBoundaries);
        Assert.AreEqual(KmlGeometryType.Polygon, kmlPolygon.GeometryType);
    }
}