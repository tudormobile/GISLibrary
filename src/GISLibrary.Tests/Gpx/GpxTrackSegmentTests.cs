using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxTrackSegmentTests
{
    [TestMethod]
    public void TrackPoints_WithNoPoints_ReturnsEmptyList()
    {
        // Arrange
        var element = new XElement("trkseg");
        var segment = new GpxDocument.GpxTrackSegment(element);

        // Act
        var points = segment.TrackPoints;

        // Assert
        Assert.IsNotNull(points);
        Assert.HasCount(0, points);
    }

    [TestMethod]
    public void TrackPoints_WithPoints_ReturnsCorrectList()
    {
        // Arrange
        var element = new XElement("trkseg",
            new XElement("trkpt",
                new XAttribute("lat", "37.8"),
                new XAttribute("lon", "-122.4"),
                new XElement("ele", "10.5")),
            new XElement("trkpt",
                new XAttribute("lat", "37.9"),
                new XAttribute("lon", "-122.5"),
                new XElement("ele", "15.2")),
            new XElement("trkpt",
                new XAttribute("lat", "38.0"),
                new XAttribute("lon", "-122.6"),
                new XElement("ele", "20.8")));
        var segment = new GpxDocument.GpxTrackSegment(element);

        // Act
        var points = segment.TrackPoints;

        // Assert
        Assert.HasCount(3, points);
        Assert.AreEqual(37.8, points[0].Latitude);
        Assert.AreEqual(-122.4, points[0].Longitude);
        Assert.AreEqual(10.5, points[0].Elevation);
        Assert.AreEqual(38.0, points[2].Latitude);
        Assert.AreEqual(-122.6, points[2].Longitude);
        Assert.AreEqual(20.8, points[2].Elevation);
    }
}