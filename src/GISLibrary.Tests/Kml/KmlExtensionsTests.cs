using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlExtensionsTests
{
    [TestMethod]
    public void AllPointPlacemarks_WithNoPoints_ReturnsEmptyEnumerable()
    {
        // Arrange
        var kmlDoc = new KmlDocument(new KmlDocumentItem("", "", ""));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var pointPlacemarks = kmlDoc.AllPointPlacemarks();
        // Assert
        Assert.IsNotNull(pointPlacemarks);
        Assert.IsEmpty(pointPlacemarks);
    }

    [TestMethod]
    public void AllLineStringPlacemarks_ReturnsLineStrings()
    {
        // Arrange
        var kmlDoc = new KmlDocument(new KmlDocumentItem("", "", ""));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var lineStringPlacemarks = kmlDoc.AllLineStringPlacemarks();
        // Assert
        Assert.IsNotNull(lineStringPlacemarks);
        Assert.HasCount(1, lineStringPlacemarks);
    }

    [TestMethod]
    public void AllPolygonPlacemarks_ReturnsPolygons()
    {
        // Arrange
        var kmlDoc = new KmlDocument(new KmlDocumentItem("", "", ""));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var polygonPlacemarks = kmlDoc.AllPolygonPlacemarks();
        // Assert
        Assert.IsNotNull(polygonPlacemarks);
        Assert.HasCount(1, polygonPlacemarks);
    }

    [TestMethod]
    public void KmlFolder_AllPointPlacemarks_WithNoPoints_ReturnsEmptyEnumerable()
    {
        // Arrange
        var kmlFolder = new KmlFolder(new KmlFolderItem("", "", ""));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var pointPlacemarks = kmlFolder.AllPointPlacemarks();
        // Assert
        Assert.IsNotNull(pointPlacemarks);
        Assert.IsEmpty(pointPlacemarks);
    }

    [TestMethod]
    public void KmlFolder_AllLineStringPlacemarks_ReturnsLineStrings()
    {
        // Arrange
        var kmlFolder = new KmlFolder(new KmlFolderItem("", "", ""));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var lineStringPlacemarks = kmlFolder.AllLineStringPlacemarks();
        // Assert
        Assert.IsNotNull(lineStringPlacemarks);
        Assert.HasCount(1, lineStringPlacemarks);
    }

    [TestMethod]
    public void KmlFolder_AllPolygonPlacemarks_ReturnsPolygons()
    {
        // Arrange
        var kmlFolder = new KmlFolder(new KmlFolderItem("", "", ""));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlFolder.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var polygonPlacemarks = kmlFolder.AllPolygonPlacemarks();
        // Assert
        Assert.IsNotNull(polygonPlacemarks);
        Assert.HasCount(1, polygonPlacemarks);
    }

    [TestMethod]
    public void AllPointPlacemarks_WithPoints_ReturnsPlacemarks()
    {
        // Arrange
        var kmlDoc = new KmlDocument(new KmlDocumentItem("", "", ""));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Point Placemark", new KmlPoint(0, 0, 0))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Point Placemark", new KmlPoint(0, 0, 0))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", new KmlLineString([]))));
        kmlDoc.Placemarks.Add(new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", new KmlPolygon([], [[]]))));
        // Act
        var pointPlacemarks = kmlDoc.AllPointPlacemarks();
        // Assert
        Assert.IsNotNull(pointPlacemarks);
        Assert.HasCount(2, pointPlacemarks);
    }

    [TestMethod]
    public void ToLineString_WithLineStringGeometry_ReturnsLineString()
    {
        // Arrange
        var lineString = new KmlLineString([(0, 0, 0), (1, 1, 0)]);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", lineString));
        // Act
        var result = placemark.ToLineString();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(lineString, result);
    }

    [TestMethod]
    public void ToPoint_WithPointGeometry_ReturnsPoint()
    {
        // Arrange
        var point = new KmlPoint(0, 0, 0);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "Point Placemark", point));
        // Act
        var result = placemark.ToPoint();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(point, result);
    }

    [TestMethod]
    public void ToPolygon_WithPolygonGeometry_ReturnsPolygon()
    {
        // Arrange
        var polygon = new KmlPolygon([], [[]]);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", polygon));
        // Act
        var result = placemark.ToPolygon();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(polygon, result);
    }

    [TestMethod]
    public void AsLineString_WithLineStringGeometry_ReturnsLineString()
    {
        // Arrange
        var lineString = new KmlLineString([(0, 0, 0), (1, 1, 0)]);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "LineString Placemark", lineString));
        // Act
        var result = placemark.AsLineString();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(lineString, result);
    }

    [TestMethod]
    public void AsPolygon_WithPolygonGeometry_ReturnsPolygon()
    {
        // Arrange
        var polygon = new KmlPolygon([], [[]]);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "Polygon Placemark", polygon));
        // Act
        var result = placemark.AsPolygon();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(polygon, result);
    }

    [TestMethod]
    public void AsPoint_WithPointGeometry_ReturnsPoint()
    {
        // Arrange
        var point = new KmlPoint(0, 0, 0);
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "Point Placemark", point));
        // Act
        var result = placemark.AsPoint();
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(point, result);
    }

    [TestMethod]
    public void AsLineString_WithoutLineStringGeometry_ReturnsNull()
    {
        // Arrange
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "", new KmlUnsupportedGeometry()));
        // Act
        var result = placemark.AsLineString();
        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void AsPolygon_WithoutPolygonGeometry_ReturnsNull()
    {
        // Arrange
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "", new KmlUnsupportedGeometry()));
        // Act
        var result = placemark.AsPolygon();
        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void AsPoint_WithoutPointGeometry_ReturnsNull()
    {
        // Arrange
        var placemark = new KmlPlacemark(new KmlPlacemarkItem("", "", "", new KmlUnsupportedGeometry()));
        // Act
        var result = placemark.AsPoint();
        // Assert
        Assert.IsNull(result);
    }





}

