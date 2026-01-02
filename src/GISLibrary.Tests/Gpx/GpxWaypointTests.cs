using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxWaypointTests
{
    [TestMethod]
    public void Latitude_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var latitude = waypoint.Latitude;

        // Assert
        Assert.AreEqual(37.8, latitude);
    }

    [TestMethod]
    public void Longitude_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var longitude = waypoint.Longitude;

        // Assert
        Assert.AreEqual(-122.4, longitude);
    }

    [TestMethod]
    public void Elevation_WithElement_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("ele", "123.45"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var elevation = waypoint.Elevation;

        // Assert
        Assert.AreEqual(123.45, elevation);
    }

    [TestMethod]
    public void Elevation_WithoutElement_ReturnsZero()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var elevation = waypoint.Elevation;

        // Assert
        Assert.AreEqual(0.0, elevation);
    }

    [TestMethod]
    public void Time_WithValidElement_ReturnsCorrectDateTime()
    {
        // Arrange
        var expectedTime = new DateTime(2023, 12, 15, 10, 30, 0);
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("time", "2023-12-15T10:30:00Z"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var time = waypoint.Time;

        // Assert
        Assert.AreEqual(expectedTime, time.ToUniversalTime());
    }

    [TestMethod]
    public void Time_WithInvalidElement_ReturnsMinValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("time", "invalid-date"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var time = waypoint.Time;

        // Assert
        Assert.AreEqual(DateTime.MinValue, time);
    }

    [TestMethod]
    public void Name_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("name", "Summit"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var name = waypoint.Name;

        // Assert
        Assert.AreEqual("Summit", name);
    }

    [TestMethod]
    public void Comment_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("cmt", "Great viewpoint"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var comment = waypoint.Comment;

        // Assert
        Assert.AreEqual("Great viewpoint", comment);
    }

    [TestMethod]
    public void Description_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("desc", "Mountain peak with panoramic views"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var description = waypoint.Description;

        // Assert
        Assert.AreEqual("Mountain peak with panoramic views", description);
    }

    [TestMethod]
    public void SymbolName_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("sym", "Flag, Blue"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var symbolName = waypoint.SymbolName;

        // Assert
        Assert.AreEqual("Flag, Blue", symbolName);
    }

    [TestMethod]
    public void ClassificationType_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("wpt",
            new XAttribute("lat", "37.8"),
            new XAttribute("lon", "-122.4"),
            new XElement("type", "Summit"));
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Act
        var type = waypoint.ClassificationType;

        // Assert
        Assert.AreEqual("Summit", type);
    }

    [TestMethod]
    public void MissingAttributes_ReturnDefaults()
    {
        // Arrange
        var element = new XElement("wpt");

        // Act
        var waypoint = new GpxDocument.GpxWaypoint(element);

        // Assert
        Assert.AreEqual(0.0, waypoint.Latitude);
        Assert.AreEqual(0.0, waypoint.Longitude);
        Assert.AreEqual(0.0, waypoint.Elevation);
        Assert.AreEqual(string.Empty, waypoint.Name);
    }
}