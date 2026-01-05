using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlUnsupportedGeometryTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var geometryType = KmlGeometryType.Unsupported;
        // Act
        var kmlUnsupportedGeometry = new KmlUnsupportedGeometry();
        // Assert
        Assert.AreEqual(geometryType, kmlUnsupportedGeometry.GeometryType);
    }

    [TestMethod]
    public void Constructor_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var geometryType = KmlGeometryType.Unsupported;
        // Act
        var kmlUnsupportedGeometry = new KmlUnsupportedGeometry() with
        {
            // No properties to set in this case
        };
        // Assert
        Assert.AreEqual(geometryType, kmlUnsupportedGeometry.GeometryType);
    }

}
