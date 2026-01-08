using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmzDocumentTests
{
    [TestMethod]
    public void Load_FromValidKmzFile_ShouldReturnKmlDocument()
    {
        // Arrange
        var kmzFilePath = "TestData/example.kmz";
        // Act
        var kmzDocument = KmzDocument.Load(kmzFilePath);
        // Assert
        Assert.IsNotNull(kmzDocument);
        Assert.IsInstanceOfType(kmzDocument, typeof(KmzDocument));
        Assert.IsEmpty(kmzDocument.Document.Folders);
        Assert.AreEqual("example.kmz", kmzDocument.Document.Name);

        Assert.HasCount(1, kmzDocument.Document.Placemarks);
        var placemark = kmzDocument.Document.Placemarks.First();
        Assert.AreEqual("KMZ Test File", placemark.Name);
        Assert.Contains("Feel free to use and share the file according to the license above.", placemark.Description);
        Assert.IsNotNull(placemark.Geometry);
        Assert.IsInstanceOfType(placemark.Geometry, typeof(KmlPoint));
        Assert.AreEqual(KmlGeometryType.Point, placemark.Geometry.GeometryType);

        var point = (KmlPoint)placemark.Geometry;
        Assert.AreEqual(38.95938755218669, point.Latitude, double.Epsilon);
        Assert.AreEqual(-95.26548316128246, point.Longitude, double.Epsilon);
        Assert.AreEqual(0.0, point.Altitude, double.Epsilon);
    }

    [TestMethod]
    public void Load_FromEmptyKmzFile_ShouldThrowFileNotFoundException()
    {
        // Arrange
        var kmzFilePath = "TestData/invalid.kmz";
        // Act & Assert
        var exception = Assert.ThrowsExactly<FileNotFoundException>(() => KmzDocument.Load(kmzFilePath));
        Assert.AreEqual("No KML file found in the KMZ archive.", exception.Message);
    }
    [TestMethod]
    public void Save_ThenLoad_ShouldPreserveKmlDocument()
    {
        // Arrange
        var content = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kml xmlns=""http://www.opengis.net/kml/2.2""><Document id=""1""><name>test name</name><description>test description</description></Document></kml>";
        var tempKmzFilePath = Path.GetTempFileName();
        // Act
        try
        {
            var kmlDoc = KmlDocument.Parse(content);
            var kmzDocument = new KmzDocument(tempKmzFilePath, kmlDoc);
            kmzDocument.Save(tempKmzFilePath);
            // Assert
            var loadedKmzDocument = KmzDocument.Load(tempKmzFilePath);
            var loadedKmlDoc = loadedKmzDocument.Document;

            Assert.HasCount(kmlDoc.Folders.Count, loadedKmlDoc.Folders);
            Assert.HasCount(kmlDoc.Placemarks.Count, loadedKmlDoc.Placemarks);
            Assert.AreEqual(kmlDoc.Name, loadedKmlDoc.Name);
            Assert.AreEqual(kmlDoc.Description, loadedKmlDoc.Description);
            Assert.AreEqual(kmlDoc.Id, loadedKmlDoc.Id);
            Assert.AreEqual("test name", loadedKmlDoc.Name);
            Assert.AreEqual("test description", loadedKmlDoc.Description);
            Assert.AreEqual("1", loadedKmlDoc.Id);
        }
        finally
        {
            // Cleanup
            File.Delete(tempKmzFilePath);
        }
    }

}