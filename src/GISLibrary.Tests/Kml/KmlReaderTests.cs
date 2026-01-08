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
    public void ReadDocumentStart_WithoutIdNameDescription_ReturnsDocumentItemWithDefaults()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
    <Document>
<Placemark></Placemark>
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
        Assert.AreEqual(string.Empty, documentItem.Id);
        Assert.AreEqual(string.Empty, documentItem.Name);
        Assert.AreEqual(string.Empty, documentItem.Description);
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
    public void ReadFolderStart_WithNoIdNameDescription_ReturnsFolderItem()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document id=""documentId"">
<Folder>
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
        Assert.AreEqual(string.Empty, folderItem.Id);
        Assert.AreEqual(string.Empty, folderItem.Name);
        Assert.AreEqual(string.Empty, folderItem.Description);
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
            var placemarks = await reader.ReadPlacemarksAsync(TestContext.CancellationToken).ToListAsync(TestContext.CancellationToken);
            // Assert
            Assert.IsNotNull(placemarks);
            Assert.IsEmpty(placemarks);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void MoveToFolder_WithNoFolder_ReturnsFalse()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document id=""documentId"">
</Document>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);
        // Act
        var success = reader.MoveToFolder();
        // Assert
        Assert.IsFalse(success);
        Assert.AreEqual(KmlReadState.EndOfFile, reader.ReadState);
    }

    [TestMethod]
    public void MoveToDocument_WithNoDocument_ReturnsFalse()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);
        // Act
        var success = reader.MoveToDocument();
        // Assert
        Assert.IsFalse(success);
        Assert.AreEqual(KmlReadState.EndOfFile, reader.ReadState);
    }

    [TestMethod]
    public void MoveToPlacemark_WithNoPlacemark_ReturnsFalse()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms);
        // Act
        var success = reader.MoveToPlacemark();
        // Assert
        Assert.IsFalse(success);
        Assert.AreEqual(KmlReadState.EndOfFile, reader.ReadState);
    }

    [TestMethod]
    public async Task ReadPlacemarksAsync_WithNoPlacemark_ReturnsEmpty()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
<Document><Folder></Folder></Document>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms, allowAsync: true);
        // Act
        var count = await reader.ReadPlacemarksAsync(TestContext.CancellationToken).CountAsync(TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public async Task ReadPlacemarksAsync_ReturnsPlacemarks()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
  <Placemark id=""placemarkId"">
    <name>Simple placemark</name>
    <description>Simple description.</description>
    <Point><coordinates>-122.0822035425683,37.42228990140251,1.234</coordinates></Point>
</Placemark>
<Placemark><Point><coordinates>-122.0822035425683,37.42228990140251</coordinates></Point></Placemark>
<Placemark><Point><extra></extra><coordinates>-122.0822035425683,37.42228990140251</coordinates></Point></Placemark>
<Placemark><LinearRing></LinearRing></Placemark>
<Placemark><Polygon>
<outerBoundaryIs><LinearRing>
<coordinates>
-122.0848938459612,37.42257124044786,17
-122.0849580979198,37.42211922626856,17
</coordinates>
</LinearRing></outerBoundaryIs>
<extraElement></extraElement>
<innerBoundaryIs><LinearRing>
<coordinates>
-122.0848938459612,37.42257124044786,17
-122.0849580979198,37.42211922626856,17
</coordinates>
</LinearRing></innerBoundaryIs>
</Polygon></Placemark>
<Placemark>
    <name>unextruded</name>
    <LineString>
      <extrude>1</extrude>
      <tessellate>1</tessellate>
      <coordinates>
        -122.364383,37.824664,0 -122.364152,37.824322,0
      </coordinates>
    </LineString>
  </Placemark>
</kml>";

        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms, allowAsync: true);
        // Act
        var placemarks = await reader.ReadPlacemarksAsync(TestContext.CancellationToken).ToArrayAsync(TestContext.CancellationToken);

        // Assert
        Assert.HasCount(6, placemarks);
        Assert.AreEqual("placemarkId", placemarks[0].Id);
        Assert.AreEqual("Simple placemark", placemarks[0].Name);
        Assert.AreEqual("Simple description.", placemarks[0].Description);
        Assert.AreEqual(-122.0822035425683, ((KmlPoint)placemarks[0].Geometry).Longitude);
        Assert.AreEqual(37.42228990140251, ((KmlPoint)placemarks[0].Geometry).Latitude);
        Assert.AreEqual(1.234, ((KmlPoint)placemarks[0].Geometry).Altitude);

        Assert.AreEqual(KmlGeometryType.Unsupported, placemarks[3].Geometry.GeometryType);

        var lineString = (KmlLineString)placemarks[5].Geometry;
        Assert.HasCount(2, lineString.Coordinates);
        Assert.AreEqual(37.824664, lineString.Coordinates[0].Latitude);
        Assert.AreEqual(-122.364383, lineString.Coordinates[0].Longitude);
        Assert.AreEqual(0, lineString.Coordinates[0].Altitude);
        Assert.AreEqual(37.824322, lineString.Coordinates[1].Latitude);
        Assert.AreEqual(-122.364152, lineString.Coordinates[1].Longitude);
        Assert.AreEqual(0, lineString.Coordinates[1].Altitude);
    }

    [TestMethod]
    public async Task ReadPlacemark_WithInvalidGeometry_ReturnsPlacemarkWithEmptyPoint()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
  <Placemark><Point><coordinates>-122.0822035425683</coordinates></Point></Placemark>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms, allowAsync: true);
        // Act & Assert
        var placemarks = await reader.ReadPlacemarksAsync(TestContext.CancellationToken).ToArrayAsync(TestContext.CancellationToken);
        Assert.HasCount(1, placemarks);
        Assert.AreEqual(KmlGeometryType.Point, placemarks[0].Geometry.GeometryType);
        var point = (KmlPoint)placemarks[0].Geometry;
        Assert.AreEqual(0, point.Latitude);
        Assert.AreEqual(0, point.Longitude);
        Assert.AreEqual(0, point.Altitude);
    }

    [TestMethod]
    public async Task ReadPlacemark_WithMissingCoordinates_Throws()
    {
        // Arrange
        var kmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2"">
  <Placemark><Point></Point></Placemark>
</kml>";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(kmlContent));
        using var reader = KmlReader.Create(ms, allowAsync: true);
        // Act & Assert
        var exception = await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
        {
            var placemarks = await reader.ReadPlacemarksAsync(TestContext.CancellationToken).ToArrayAsync(TestContext.CancellationToken);
        });
        Assert.AreEqual("Placemark geometry could not be read.", exception.Message);
    }


    public TestContext TestContext { get; set; }
}