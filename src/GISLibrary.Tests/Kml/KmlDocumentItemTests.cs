using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlDocumentItemTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "Test Document";
        var description = "This is a test KML document.";
        var id = "1";
        // Act
        var kmlDocumentItem = new KmlDocumentItem(id, name, description);
        // Assert
        Assert.AreEqual(KmlItemType.Document, kmlDocumentItem.ItemType);
        Assert.AreEqual(id, kmlDocumentItem.Id);
        Assert.AreEqual(name, kmlDocumentItem.Name);
        Assert.AreEqual(description, kmlDocumentItem.Description);
    }

    [TestMethod]
    public void Construct_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "Test Document";
        var description = "This is a test KML document.";
        var id = "1";
        // Act
        var kmlDocumentItem = new KmlDocumentItem(id, name, description) with
        {
            Id = id,
            Name = name,
            Description = description
        };
        // Assert
        Assert.AreEqual(KmlItemType.Document, kmlDocumentItem.ItemType);
        Assert.AreEqual(id, kmlDocumentItem.Id);
        Assert.AreEqual(name, kmlDocumentItem.Name);
        Assert.AreEqual(description, kmlDocumentItem.Description);
        Assert.AreEqual(KmlItemType.Document, kmlDocumentItem.ItemType);
    }

}