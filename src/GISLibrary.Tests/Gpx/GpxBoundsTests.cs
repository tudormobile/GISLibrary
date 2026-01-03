using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxBoundsTests
{
    [TestMethod]
    public void Constructor_WithNullElement_InitializesWithZeroValues()
    {
        // Arrange & Act
        var bounds = new GpxDocument.GpxBounds(null);

        // Assert
        Assert.AreEqual(0.0, bounds.MinLat);
        Assert.AreEqual(0.0, bounds.MinLon);
        Assert.AreEqual(0.0, bounds.MaxLat);
        Assert.AreEqual(0.0, bounds.MaxLon);
    }

    [TestMethod]
    public void Constructor_WithValidElement_ParsesCoordinates()
    {
        // Arrange
        var element = new XElement("bounds",
            new XAttribute("minlat", "37.5"),
            new XAttribute("minlon", "-122.5"),
            new XAttribute("maxlat", "38.5"),
            new XAttribute("maxlon", "-121.5"));

        // Act
        var bounds = new GpxDocument.GpxBounds(element);

        // Assert
        Assert.AreEqual(37.5, bounds.MinLat);
        Assert.AreEqual(-122.5, bounds.MinLon);
        Assert.AreEqual(38.5, bounds.MaxLat);
        Assert.AreEqual(-121.5, bounds.MaxLon);
    }

    [TestMethod]
    public void Constructor_WithMissingAttributes_UsesZeroDefaults()
    {
        // Arrange
        var element = new XElement("bounds");

        // Act
        var bounds = new GpxDocument.GpxBounds(element);

        // Assert
        Assert.AreEqual(0.0, bounds.MinLat);
        Assert.AreEqual(0.0, bounds.MinLon);
        Assert.AreEqual(0.0, bounds.MaxLat);
        Assert.AreEqual(0.0, bounds.MaxLon);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var element = new XElement("bounds",
            new XAttribute("minlat", "37.5"),
            new XAttribute("minlon", "-122.5"),
            new XAttribute("maxlat", "38.5"),
            new XAttribute("maxlon", "-121.5"));
        var bounds = new GpxDocument.GpxBounds(element);

        // Act
        var result = bounds.ToString();

        // Assert
        Assert.AreEqual("(MinLat: 37.5, MinLon: -122.5, MaxLat: 38.5, MaxLon: -121.5)", result);
    }

    [TestMethod]
    public void Properties_AreSettable()
    {
        // Arrange
        var bounds = new GpxDocument.GpxBounds(null)
        {
            // Act
            MinLat = 10.5,
            MinLon = 20.5,
            MaxLat = 30.5,
            MaxLon = 40.5
        };

        // Assert
        Assert.AreEqual(10.5, bounds.MinLat);
        Assert.AreEqual(20.5, bounds.MinLon);
        Assert.AreEqual(30.5, bounds.MaxLat);
        Assert.AreEqual(40.5, bounds.MaxLon);
    }
}