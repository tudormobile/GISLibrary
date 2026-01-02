using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxTrackTests
{
    [TestMethod]
    public void Number_WithElement_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("trk",
            new XElement("number", "5"));
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var number = track.Number;

        // Assert
        Assert.AreEqual(5, number);
    }

    [TestMethod]
    public void Number_WithoutElement_ReturnsZero()
    {
        // Arrange
        var element = new XElement("trk");
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var number = track.Number;

        // Assert
        Assert.AreEqual(0, number);
    }

    [TestMethod]
    public void TrackSegments_WithNoSegments_ReturnsEmptyList()
    {
        // Arrange
        var element = new XElement("trk");
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var segments = track.TrackSegments;

        // Assert
        Assert.IsNotNull(segments);
        Assert.IsEmpty(segments);
    }

    [TestMethod]
    public void TrackSegments_WithSegments_ReturnsCorrectList()
    {
        // Arrange
        var element = new XElement("trk",
            new XElement("trkseg",
                new XElement("trkpt",
                    new XAttribute("lat", "37.8"),
                    new XAttribute("lon", "-122.4"))),
            new XElement("trkseg",
                new XElement("trkpt",
                    new XAttribute("lat", "37.9"),
                    new XAttribute("lon", "-122.5"))));
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var segments = track.TrackSegments;

        // Assert
        Assert.HasCount(2, segments);
    }

    [TestMethod]
    public void Name_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("trk",
            new XElement("name", "Morning Ride"));
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var name = track.Name;

        // Assert
        Assert.AreEqual("Morning Ride", name);
    }

    [TestMethod]
    public void Description_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("trk",
            new XElement("desc", "A scenic ride through the hills"));
        var track = new GpxDocument.GpxTrack(element);

        // Act
        var description = track.Description;

        // Assert
        Assert.AreEqual("A scenic ride through the hills", description);
    }
}