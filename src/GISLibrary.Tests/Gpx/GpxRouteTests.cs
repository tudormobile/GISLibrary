using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxRouteTests
{
    [TestMethod]
    public void Number_WithElement_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("rte",
            new XElement("number", "3"));
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var number = route.Number;

        // Assert
        Assert.AreEqual(3, number);
    }

    [TestMethod]
    public void Number_WithoutElement_ReturnsZero()
    {
        // Arrange
        var element = new XElement("rte");
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var number = route.Number;

        // Assert
        Assert.AreEqual(0, number);
    }

    [TestMethod]
    public void RoutePoints_WithNoPoints_ReturnsEmptyList()
    {
        // Arrange
        var element = new XElement("rte");
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var points = route.RoutePoints;

        // Assert
        Assert.IsNotNull(points);
        Assert.IsEmpty(points);
    }

    [TestMethod]
    public void RoutePoints_WithPoints_ReturnsCorrectList()
    {
        // Arrange
        var element = new XElement("rte",
            new XElement("rtept",
                new XAttribute("lat", "37.8"),
                new XAttribute("lon", "-122.4"),
                new XElement("name", "Start")),
            new XElement("rtept",
                new XAttribute("lat", "37.9"),
                new XAttribute("lon", "-122.5"),
                new XElement("name", "Midpoint")),
            new XElement("rtept",
                new XAttribute("lat", "38.0"),
                new XAttribute("lon", "-122.6"),
                new XElement("name", "End")));
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var points = route.RoutePoints;

        // Assert
        Assert.HasCount(3, points);
        Assert.AreEqual("Start", points[0].Name);
        Assert.AreEqual("Midpoint", points[1].Name);
        Assert.AreEqual("End", points[2].Name);
        Assert.AreEqual(37.8, points[0].Latitude);
        Assert.AreEqual(38.0, points[2].Latitude);
    }

    [TestMethod]
    public void Name_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("rte",
            new XElement("name", "Commute Route"));
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var name = route.Name;

        // Assert
        Assert.AreEqual("Commute Route", name);
    }

    [TestMethod]
    public void Comment_ReturnsCorrectValue()
    {
        // Arrange
        var element = new XElement("rte",
            new XElement("cmt", "Avoid highway"));
        var route = new GpxDocument.GpxRoute(element);

        // Act
        var comment = route.Comment;

        // Assert
        Assert.AreEqual("Avoid highway", comment);
    }
}