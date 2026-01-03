using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxDocumentTests
{
    [TestMethod]
    public void Constructor_WithNullDocument_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
#pragma warning disable CS8625
        Assert.ThrowsExactly<ArgumentNullException>(() => new GpxDocument(null));
#pragma warning restore CS8625
    }

    [TestMethod]
    public void Constructor_WithNullRoot_ThrowsArgumentException()
    {
        // Arrange
        var doc = new XDocument();

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new GpxDocument(doc));
        Assert.Contains("does not have a root element", ex.Message);
    }

    [TestMethod]
    public void Constructor_WithInvalidRootElement_ThrowsArgumentException()
    {
        // Arrange
        var doc = new XDocument(new XElement("invalid"));

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new GpxDocument(doc));
        Assert.Contains("The provided XDocument does not have the expected root element.", ex.Message);
    }

    [TestMethod]
    public void Constructor_WithMissingVersionAttribute_ThrowsArgumentException()
    {
        // Arrange
        var doc = new XDocument(new XElement("gpx"));

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new GpxDocument(doc));
        Assert.Contains("does not have a version attribute", ex.Message);
    }

    [TestMethod]
    public void Constructor_WithUnsupportedVersion_ThrowsNotSupportedException()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "2.0"))
        );

        // Act & Assert
        var ex = Assert.ThrowsExactly<NotSupportedException>(() => new GpxDocument(doc));
        Assert.Contains("version 2.0 is not supported", ex.Message);
    }

    [TestMethod]
    public void Constructor_WithVersion10_InitializesSuccessfully()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );

        // Act
        var gpx = new GpxDocument(doc);

        // Assert
        Assert.IsNotNull(gpx);
        Assert.AreEqual("1.0", gpx.Version);
        Assert.IsFalse(gpx.IsVersion11);
    }

    [TestMethod]
    public void Constructor_WithVersion11_InitializesSuccessfully()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.1"))
        );

        // Act
        var gpx = new GpxDocument(doc);

        // Assert
        Assert.IsNotNull(gpx);
        Assert.AreEqual("1.1", gpx.Version);
        Assert.IsTrue(gpx.IsVersion11);
    }

    [TestMethod]
    public void Name_WithVersion10Metadata_ReturnsName()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("name", "My Track"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var name = gpx.Name;

        // Assert
        Assert.AreEqual("My Track", name);
    }

    [TestMethod]
    public void Name_WithVersion11Metadata_ReturnsName()
    {
        // Arrange
        var ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
        var doc = new XDocument(
            new XElement(ns + "gpx",
                new XAttribute("version", "1.1"),
                new XElement(ns + "metadata",
                    new XElement(ns + "name", "My Track")))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var name = gpx.Name;

        // Assert
        Assert.AreEqual("My Track", name);
    }

    [TestMethod]
    public void Name_WithMissingElement_ReturnsEmptyString()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var name = gpx.Name;

        // Assert
        Assert.AreEqual(string.Empty, name);
    }

    [TestMethod]
    public void Description_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("desc", "Test description"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var description = gpx.Description;

        // Assert
        Assert.AreEqual("Test description", description);
    }

    [TestMethod]
    public void Author_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("author", "John Doe"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var author = gpx.Author;

        // Assert
        Assert.AreEqual("John Doe", author);
    }

    [TestMethod]
    public void Email_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("email", "test@example.com"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var email = gpx.Email;

        // Assert
        Assert.AreEqual("test@example.com", email);
    }

    [TestMethod]
    public void Url_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("url", "https://example.com"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var url = gpx.Url;

        // Assert
        Assert.AreEqual("https://example.com", url);
    }

    [TestMethod]
    public void UrlName_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("urlname", "Example Site"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var urlName = gpx.UrlName;

        // Assert
        Assert.AreEqual("Example Site", urlName);
    }

    [TestMethod]
    public void Time_WithValidDate_ReturnsCorrectDateTime()
    {
        // Arrange
        var expectedTime = new DateTime(2023, 12, 15, 10, 30, 0);
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("time", "2023-12-15T10:30:00Z"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var time = gpx.Time;

        // Assert
        Assert.AreEqual(expectedTime, time.ToUniversalTime());
    }

    [TestMethod]
    public void Time_WithInvalidDate_ReturnsMinValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("time", "invalid-date"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var time = gpx.Time;

        // Assert
        Assert.AreEqual(DateTime.MinValue, time);
    }

    [TestMethod]
    public void Time_WithMissingElement_ReturnsMinValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var time = gpx.Time;

        // Assert
        Assert.AreEqual(DateTime.MinValue, time);
    }

    [TestMethod]
    public void Keywords_ReturnsCorrectValue()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("keywords", "hiking, trail, mountain"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var keywords = gpx.Keywords;

        // Assert
        Assert.AreEqual("hiking, trail, mountain", keywords);
    }

    [TestMethod]
    public void Waypoints_WithNoWaypoints_ReturnsEmptyList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var waypoints = gpx.Waypoints;

        // Assert
        Assert.IsNotNull(waypoints);
        Assert.IsEmpty(waypoints);
    }

    [TestMethod]
    public void Waypoints_WithWaypoints_ReturnsCorrectList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("wpt",
                    new XAttribute("lat", "37.8"),
                    new XAttribute("lon", "-122.4"),
                    new XElement("name", "Point 1")),
                new XElement("wpt",
                    new XAttribute("lat", "37.9"),
                    new XAttribute("lon", "-122.5"),
                    new XElement("name", "Point 2")))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var waypoints = gpx.Waypoints;

        // Assert
        Assert.HasCount(2, waypoints);
        Assert.AreEqual("Point 1", waypoints[0].Name);
        Assert.AreEqual("Point 2", waypoints[1].Name);
    }

    [TestMethod]
    public void Routes_WithNoRoutes_ReturnsEmptyList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var routes = gpx.Routes;

        // Assert
        Assert.IsNotNull(routes);
        Assert.IsEmpty(routes);
    }

    [TestMethod]
    public void Routes_WithRoutes_ReturnsCorrectList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("rte",
                    new XElement("name", "Route 1")),
                new XElement("rte",
                    new XElement("name", "Route 2")))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var routes = gpx.Routes;

        // Assert
        Assert.HasCount(2, routes);
        Assert.AreEqual("Route 1", routes[0].Name);
        Assert.AreEqual("Route 2", routes[1].Name);
    }

    [TestMethod]
    public void Tracks_WithNoTracks_ReturnsEmptyList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var tracks = gpx.Tracks;

        // Assert
        Assert.IsNotNull(tracks);
        Assert.IsEmpty(tracks);
    }

    [TestMethod]
    public void Tracks_WithTracks_ReturnsCorrectList()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("trk",
                    new XElement("name", "Track 1")),
                new XElement("trk",
                    new XElement("name", "Track 2")))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var tracks = gpx.Tracks;

        // Assert
        Assert.HasCount(2, tracks);
        Assert.AreEqual("Track 1", tracks[0].Name);
        Assert.AreEqual("Track 2", tracks[1].Name);
    }

    [TestMethod]
    public void Bounds_Version10_WithBounds_ReturnsCorrectBounds()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("bounds",
                    new XAttribute("minlat", "37.0"),
                    new XAttribute("minlon", "-123.0"),
                    new XAttribute("maxlat", "38.0"),
                    new XAttribute("maxlon", "-122.0")))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var bounds = gpx.Bounds;

        // Assert
        Assert.IsNotNull(bounds);
        Assert.AreEqual(37.0, bounds.MinLat);
        Assert.AreEqual(-123.0, bounds.MinLon);
        Assert.AreEqual(38.0, bounds.MaxLat);
        Assert.AreEqual(-122.0, bounds.MaxLon);
    }

    [TestMethod]
    public void Bounds_Version11_WithBounds_ReturnsCorrectBounds()
    {
        // Arrange
        var ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
        var doc = new XDocument(
            new XElement(ns + "gpx",
                new XAttribute("version", "1.1"),
                new XElement(ns + "metadata",
                    new XElement(ns + "bounds",
                        new XAttribute("minlat", "37.0"),
                        new XAttribute("minlon", "-123.0"),
                        new XAttribute("maxlat", "38.0"),
                        new XAttribute("maxlon", "-122.0"))))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var bounds = gpx.Bounds;

        // Assert
        Assert.IsNotNull(bounds);
        Assert.AreEqual(37.0, bounds.MinLat);
        Assert.AreEqual(-123.0, bounds.MinLon);
        Assert.AreEqual(38.0, bounds.MaxLat);
        Assert.AreEqual(-122.0, bounds.MaxLon);
    }

    [TestMethod]
    public void Bounds_WithNoBounds_ReturnsDefaultBounds()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx", new XAttribute("version", "1.0"))
        );
        var gpx = new GpxDocument(doc);

        // Act
        var bounds = gpx.Bounds;

        // Assert
        Assert.IsNotNull(bounds);
        Assert.AreEqual(0.0, bounds.MinLat);
        Assert.AreEqual(0.0, bounds.MinLon);
        Assert.AreEqual(0.0, bounds.MaxLat);
        Assert.AreEqual(0.0, bounds.MaxLon);
    }

    [TestMethod]
    public void Save_ToFile_SavesSuccessfully()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("name", "Test Track"))
        );
        var gpx = new GpxDocument(doc);
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            gpx.Save(tempFile);

            // Assert
            Assert.IsTrue(File.Exists(tempFile));
            var loadedDoc = XDocument.Load(tempFile);
            Assert.AreEqual("gpx", loadedDoc.Root?.Name.LocalName);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [TestMethod]
    public void Save_ToStream_SavesSuccessfully()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("name", "Test Track"))
        );
        var gpx = new GpxDocument(doc);

        using var stream = new MemoryStream();

        // Act
        gpx.Save(stream);

        // Assert
        Assert.IsGreaterThan(0, stream.Length);
        stream.Position = 0;
        var loadedDoc = XDocument.Load(stream);
        Assert.AreEqual("gpx", loadedDoc.Root?.Name.LocalName);
    }

    [TestMethod]
    public async Task SaveAsync_ToStream_SavesSuccessfully()
    {
        // Arrange
        var doc = new XDocument(
            new XElement("gpx",
                new XAttribute("version", "1.0"),
                new XElement("name", "Test Track"))
        );
        var gpx = new GpxDocument(doc);

        using var stream = new MemoryStream();

        // Act
        await gpx.SaveAsync(stream, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsGreaterThan(0, stream.Length);
        stream.Position = 0;
        var loadedDoc = XDocument.Load(stream);
        Assert.AreEqual("gpx", loadedDoc.Root?.Name.LocalName);
    }

    public TestContext TestContext { get; set; }

}
