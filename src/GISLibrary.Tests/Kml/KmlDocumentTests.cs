using System.Text;
using System.Xml;
using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlDocumentTests
{
    [TestMethod]
    public void Load_FromValidKmlFile_ShouldReturnKmlDocument()
    {
        // Arrange
        var kmlFilePath = "TestData/KML_Samples.kml";
        // Act
        var kmlDocument = KmlDocument.Load(kmlFilePath);
        // Assert
        Assert.IsNotNull(kmlDocument);
        Assert.IsInstanceOfType<KmlDocument>(kmlDocument);
        Assert.AreEqual("KML Samples", kmlDocument.Name);
        Assert.AreEqual("Unleash your creativity with the help of these examples!", kmlDocument.Description);

        Assert.IsNotEmpty(kmlDocument.Folders);
        Assert.HasCount(6, kmlDocument.Folders);
        Assert.IsEmpty(kmlDocument.Placemarks);

        var firstFolder = kmlDocument.Folders[0];
        Assert.AreEqual("Placemarks", firstFolder.Name);
        Assert.AreEqual("These are just some of the different kinds of placemarks with which you can mark your favorite places", firstFolder.Description);
        Assert.HasCount(3, firstFolder.Placemarks);
        Assert.AreEqual("Placemarks", firstFolder.Name);
        Assert.AreEqual("These are just some of the different kinds of placemarks with which you can mark your favorite places", firstFolder.Description);
        Assert.HasCount(3, firstFolder.Placemarks);

        var firstPlacemark = firstFolder.Placemarks[0];
        Assert.AreEqual("Simple placemark", firstPlacemark.Name);
        Assert.AreEqual("Attached to the ground.", firstPlacemark.Description);
        Assert.AreEqual(-122.0822035425683, ((KmlPoint)firstPlacemark.Geometry).Longitude);
        Assert.AreEqual(37.42228990140251, ((KmlPoint)firstPlacemark.Geometry).Latitude);
        Assert.AreEqual(0, ((KmlPoint)firstPlacemark.Geometry).Altitude);

        Assert.HasCount(10, kmlDocument.AllPlacemarks);
    }

    [TestMethod]
    public async Task LoadAsync_FromMinimalKmlFile_ShouldReturnKmlDocument()
    {
        // Arrange
        var content = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2""></kml>";
        var path = Path.GetTempFileName();
        await File.WriteAllTextAsync(path, content, TestContext.CancellationToken);

        try
        {
            // Act
            var kmlDocument = await KmlDocument.LoadAsync(path, TestContext.CancellationToken);
            // Assert
            Assert.IsNotNull(kmlDocument);
            Assert.IsInstanceOfType<KmlDocument>(kmlDocument);
            Assert.IsEmpty(kmlDocument.Folders);
            Assert.IsEmpty(kmlDocument.Placemarks);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public async Task WriteAsync_FromMinimalKmlFile_ShouldWriteKmlDocument()
    {
        // Arrange
        var kmlDocument = new KmlDocument(new KmlDocumentItem("1", "Test Document", "This is a test document."));
        var path = Path.GetTempFileName();

        try
        {
            // Act
            await kmlDocument.SaveAsync(path, TestContext.CancellationToken);
            // Assert
            Assert.IsTrue(File.Exists(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void Parse_InvalidKmlContent_ShouldThrowException()
    {
        // Arrange
        var invalidKmlContent = "<kmlInvalid></kmlInvalid>";
        // Act & Assert
        Assert.ThrowsExactly<FormatException>(() => KmlDocument.Parse(invalidKmlContent));
    }

    [TestMethod]
    public void Parse_ValidKmlContent_ShouldReturnKmlDocument()
    {
        // Arrange
        var validKmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2""></kml>";
        // Act
        var kmlDocument = KmlDocument.Parse(validKmlContent);
        // Assert
        Assert.IsEmpty(kmlDocument.Folders);
        Assert.IsEmpty(kmlDocument.Placemarks);
    }

    [TestMethod]
    public void Save_ValidKmlDocument_ShouldWriteKmlFile()
    {
        var path = Path.GetTempFileName();
        try
        {
            // Arrange
            var kmlDocument = new KmlDocument(new KmlDocumentItem("1", "Test Document", "This is a test document."));
            // Act
            kmlDocument.Save(path);
            // Assert
            Assert.IsTrue(File.Exists(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void WriteTo_ValidKmlDocument_ShouldWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var kmlDocument = new KmlDocument(new KmlDocumentItem("1", "Test Document", "This is a test document."));
        // Act
        kmlDocument.WriteTo(stream);
        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("<kml", content);
        Assert.Contains("id=\"1\"", content);
        Assert.Contains("Test Document", content);
        Assert.Contains("This is a test document.", content);
    }

    [TestMethod]
    public async Task WriteToAsync_ValidKmlDocument_ShouldWrite()
    {
        // Arrange
        var stream = new MemoryStream();
        var kmlDocument = new KmlDocument(new KmlDocumentItem("1", "Test Document", "This is a test document."));
        // Act
        await kmlDocument.WriteToAsync(stream, TestContext.CancellationToken);
        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("<kml", content);
        Assert.Contains("id=\"1\"", content);
        Assert.Contains("Test Document", content);
        Assert.Contains("This is a test document.", content);
    }

    [TestMethod]
    public void Parse_PolygonKmlContent_ShouldReturnKmlDocumentWithPolygon()
    {
        // Arrange
        var polygonKmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document>
<Placemark>
<name>Polygon Example</name>
<Polygon>
<outerBoundaryIs>
<LinearRing>
<coordinates>
-122.0,37.0,0 -122.0,38.0,0 -121.0,38.0,0 -121.0,37.0,0 -122.0,37.0,0
</coordinates>
</LinearRing>
</outerBoundaryIs>
</Polygon>
</Placemark>
</Document>
</kml>";
        // Act
        var kmlDocument = KmlDocument.Parse(polygonKmlContent);
        // Assert
        Assert.HasCount(1, kmlDocument.Placemarks);
        var placemark = kmlDocument.Placemarks.First();
        Assert.AreEqual("Polygon Example", placemark.Name);

        var geometry = placemark.Geometry;
        Assert.AreEqual(KmlGeometryType.Polygon, geometry.GeometryType);
    }

    [TestMethod]
    public void Parse_ContentWithUnsupportedGeometry_ReturnsDocument()
    {
        var content = @"<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document>
<Placemark><Polygon><outerBoundaryIs><LinearRing></LinearRing></outerBoundaryIs></Polygon></Placemark>
<Placemark><Polygon><outerBoundaryIs><LinearRing><coordinates></coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark>
<Placemark><Polygon></Polygon></Placemark>
<Placemark><Polygon><outerBoundaryIs></outerBoundaryIs></Polygon></Placemark>
<Placemark><Point></Point></Placemark>
<Placemark><Point><coordinates></coordinates></Point></Placemark>
<Placemark><Point><coordinates>0</coordinates></Point></Placemark>
<Placemark><Point><coordinates>0,1</coordinates></Point></Placemark>
<Placemark><Point><coordinates>a,b</coordinates></Point></Placemark>
<Placemark><LineString></LineString></Placemark>
<Placemark><LineString><coordinates></coordinates></LineString></Placemark>
<Placemark><LineString><coordinates>1</coordinates></LineString></Placemark>
</Document>
</kml>";
        // Act
        var kmlDocument = KmlDocument.Parse(content);
        Assert.HasCount(12, kmlDocument.Placemarks);
    }

    [TestMethod]
    public void Parse_PlacemarkAndFolderWithAttributes_ShouldReturnKmlDocumentWithPlacemarksAndFolders()
    {
        // Arrange
        var content = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document>
<Placemark id=""1""><name>Polygon Example</name><description>test</description><Point><coordinates>-122.0,37.0</coordinates></Point></Placemark>
<Folder id=""1""><name>Polygon Example</name><description>test</description>
<Placemark id=""1""><name>Polygon Example</name><description>test</description><Point><coordinates>-122.0,37.0</coordinates></Point></Placemark>
</Folder>
<Folder></Folder>
</Document>
</kml>";
        // Act
        var kmlDocument = KmlDocument.Parse(content);
        // Assert
        Assert.HasCount(1, kmlDocument.Placemarks);
        Assert.HasCount(2, kmlDocument.Folders);
        Assert.HasCount(1, kmlDocument.Folders[0].Placemarks);
        Assert.HasCount(2, kmlDocument.AllPlacemarks);
    }

    [TestMethod]
    public void Parse_EmptyDocument_ThrowsXmlException()
    {
        // Arrange
        var content = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
        // Act & Assert
        _ = Assert.ThrowsExactly<XmlException>(() => KmlDocument.Parse(content));
    }

    public TestContext TestContext { get; set; }
}
