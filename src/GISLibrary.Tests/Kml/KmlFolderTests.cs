using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlFolderTests
{


    [TestMethod]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "1";
        var name = "Test Folder";
        var description = "This is a test KML folder.";
        // Act
        var kmlFolderItem = new KmlFolderItem(id, name, description);
        var kmlFolder = new KmlFolder(kmlFolderItem);
        // Assert
        Assert.AreEqual(id, kmlFolderItem.Id);
        Assert.AreEqual(name, kmlFolderItem.Name);
        Assert.AreEqual(description, kmlFolderItem.Description);
        Assert.AreEqual(KmlItemType.Folder, kmlFolderItem.ItemType);
        Assert.IsNotNull(kmlFolder.Placemarks);
        Assert.IsEmpty(kmlFolder.Placemarks);
    }
}
