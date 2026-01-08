using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlFolderItemTests
{
    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "123";
        var name = "Test Folder";
        var description = "This is a test KML folder.";
        // Act
        var kmlFolderItem = new KmlFolderItem(id, name, description);
        // Assert
        Assert.AreEqual(KmlItemType.Folder, kmlFolderItem.ItemType);
        Assert.AreEqual(id, kmlFolderItem.Id);
        Assert.AreEqual(name, kmlFolderItem.Name);
        Assert.AreEqual(description, kmlFolderItem.Description);
    }

    [TestMethod]
    public void Construct_With_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "123";
        var name = "Test Folder";
        var description = "This is a test KML folder.";
        // Act
        var kmlFolderItem = new KmlFolderItem(id, name, description) with
        {
            Id = id,
            Name = name,
            Description = description
        };
        // Assert
        Assert.AreEqual(KmlItemType.Folder, kmlFolderItem.ItemType);
        Assert.AreEqual(id, kmlFolderItem.Id);
        Assert.AreEqual(name, kmlFolderItem.Name);
        Assert.AreEqual(description, kmlFolderItem.Description);
    }

}
