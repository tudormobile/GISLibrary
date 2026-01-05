using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlPlacemarkItemTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "1";
        var name = "Test name";
        var description = "This is a test description.";
        var geometry = new KmlPoint(1, 2, 3);
        // Act
        var kmlPlacemarkItem = new KmlPlacemarkItem(id, name, description, geometry);
        // Assert
        Assert.AreEqual(id, kmlPlacemarkItem.Id);
        Assert.AreEqual(name, kmlPlacemarkItem.Name);
        Assert.AreEqual(description, kmlPlacemarkItem.Description);
        Assert.AreEqual(geometry, kmlPlacemarkItem.Geometry);
        Assert.AreEqual(KmlItemType.Placemark, kmlPlacemarkItem.ItemType);
    }

    [TestMethod]
    public void Construct_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "1";
        var name = "Test name";
        var description = "This is a test description.";
        var geometry = new KmlPoint(1, 2, 3);
        // Act
        var kmlPlacemarkItem = new KmlPlacemarkItem(id, name, description, geometry) with
        {
            Id = id,
            Name = name,
            Description = description,
            Geometry = geometry
        };
        // Assert
        Assert.AreEqual(id, kmlPlacemarkItem.Id);
        Assert.AreEqual(name, kmlPlacemarkItem.Name);
        Assert.AreEqual(description, kmlPlacemarkItem.Description);
        Assert.AreEqual(geometry, kmlPlacemarkItem.Geometry);
        Assert.AreEqual(KmlItemType.Placemark, kmlPlacemarkItem.ItemType);
    }

}
