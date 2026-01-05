using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlPlacemarkTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "1";
        var name = "Test name";
        var description = "This is a test description.";
        var geometry = new KmlPoint(1, 2, 3);
        var kmlPlacemarkItem = new KmlPlacemarkItem(id, name, description, geometry);
        // Act
        var kmlPlacemark = new KmlPlacemark(kmlPlacemarkItem);
        // Assert
        Assert.AreEqual(id, kmlPlacemark.Id);
        Assert.AreEqual(name, kmlPlacemark.Name);
        Assert.AreEqual(description, kmlPlacemark.Description);
        Assert.AreEqual(geometry, kmlPlacemark.Geometry);
    }
}