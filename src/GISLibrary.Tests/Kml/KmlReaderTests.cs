using System.Text;
using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlReaderTests
{
    [TestMethod]
    public void Create_SetsInitialState()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var reader = KmlReader.Create(ms);

        // Assert
        Assert.AreEqual(KmlReadState.Initial, reader.ReadState);
    }

    [TestMethod]
    public void ReadDocumentStart_ReturnsDocumentItem()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
    <Document id=""documentId"">
        <name>Test Document</name>
        <description>This is a test KML document.</description>
    </Document>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);
        // Act
        var success = reader.MoveToDocument();
        var documentItem = reader.ReadDocumentStart();
        // Assert
        Assert.AreEqual(KmlReadState.Document, reader.ReadState);
        Assert.IsNotNull(documentItem);
        Assert.AreEqual("documentId", documentItem.Id);
        Assert.AreEqual("Test Document", documentItem.Name);
        Assert.AreEqual("This is a test KML document.", documentItem.Description);
    }

    [TestMethod]
    public void ReadFolderStart_ReturnsFolderItem()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
    <Document id=""documentId"">
<Folder id=""FeatureLayer4"">
    <name>Election Districts</name>
    <snippet></snippet>
    <description><![CDATA[The election districts dataset (as of 6/12/25) is a combination of data from the NYC and County boards of elections. 
    ]]></description>
</Folder>
    </Document>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);
        // Act
        var success = reader.MoveToFolder();
        var isSuccess = reader.IsFolder();
        var folderItem = reader.ReadFolderStart();
        // Assert
        Assert.AreEqual(KmlReadState.Folder, reader.ReadState);
        Assert.IsNotNull(folderItem);
        Assert.IsTrue(isSuccess);
        Assert.AreEqual("FeatureLayer4", folderItem.Id);
        Assert.AreEqual("Election Districts", folderItem.Name);
        Assert.AreEqual("The election districts dataset (as of 6/12/25) is a combination of data from the NYC and County boards of elections.", folderItem.Description.Trim());
    }

    [TestMethod]
    public void ReadPlacemark_AfterMoveToPlacemark_ReadsPlacemark()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
  <Placemark>
    <name>Simple placemark</name>
    <description>Simple description.</description>
    <Point>
      <coordinates>-122.0822035425683,37.42228990140251</coordinates>
    </Point>
  </Placemark>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);

        // Act
        var success = reader.MoveToPlacemark();
        var isSuccess = reader.IsPlacemark();
        var placemark = reader.ReadPlacemark();

        // Assert
        Assert.AreEqual(KmlReadState.Placemark, reader.ReadState);
        Assert.IsTrue(success);
        Assert.IsTrue(isSuccess);
        Assert.IsNotNull(placemark);
        Assert.AreEqual(string.Empty, placemark.Id);
        Assert.AreEqual("Simple placemark", placemark.Name);
        Assert.AreEqual("Simple description.", placemark.Description);
        Assert.IsNotNull(placemark.Geometry);
        Assert.AreEqual(KmlGeometryType.Point, placemark.Geometry.GeometryType);

        var point = (KmlPoint)placemark.Geometry;
        Assert.AreEqual(37.42228990140251, point.Latitude);
        Assert.AreEqual(-122.0822035425683, point.Longitude);
        Assert.AreEqual(0, point.Altitude);
    }

    [TestMethod]
    public void ReadPlacemarkWithAltitude_AfterMoveToPlacemark_ReadsPlacemark()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
  <Placemark>
    <name>Simple placemark</name>
    <description>Simple description.</description>
    <Point>
      <coordinates>-122.0822035425683,37.42228990140251,1.234</coordinates>
    </Point>
  </Placemark>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);

        // Act
        var success = reader.MoveToPlacemark();
        var isSuccess = reader.IsPlacemark();
        var placemark = reader.ReadPlacemark();

        // Assert
        Assert.AreEqual(KmlReadState.Placemark, reader.ReadState);
        Assert.IsTrue(success);
        Assert.IsTrue(isSuccess);
        Assert.IsFalse(reader.IsFolder());
        Assert.IsFalse(reader.IsDocument());
        Assert.IsNotNull(placemark);
        Assert.AreEqual(string.Empty, placemark.Id);
        Assert.AreEqual("Simple placemark", placemark.Name);
        Assert.AreEqual("Simple description.", placemark.Description);
        Assert.IsNotNull(placemark.Geometry);
        Assert.AreEqual(KmlGeometryType.Point, placemark.Geometry.GeometryType);

        var point = (KmlPoint)placemark.Geometry;
        Assert.AreEqual(37.42228990140251, point.Latitude);
        Assert.AreEqual(-122.0822035425683, point.Longitude);
        Assert.AreEqual(1.234, point.Altitude);
    }

    [TestMethod]
    public async Task ReadPlacemarksAsync_EmptyDocument_ReturnsEmptyEnumerable()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?><kml xmlns=""http://www.opengis.net/kml/2.2""></kml>";
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, kmlContent);
            using var reader = KmlReader.Create(path, allowAsync: true);
            // Act
            var placemarks = await reader.ReadPlacemarksAsync().ToListAsync();
            // Assert
            Assert.IsNotNull(placemarks);
            Assert.IsEmpty(placemarks);
        }
        finally
        {
            File.Delete(path);
        }
    }
}