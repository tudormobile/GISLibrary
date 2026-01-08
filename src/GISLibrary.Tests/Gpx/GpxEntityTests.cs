using System.Xml.Linq;
using Tudormobile.Gpx;

namespace GISLibrary.Tests.Gpx;

[TestClass]
public class GpxEntityTests
{
    [TestMethod]
    public void PropertyAccessors_ReturnsExpectedValues()
    {
        // Arrange
        var gpxEntityXml = @"
            <gpx>
                <name>Test Name</name>
                <cmt>Test Comment</cmt>
                <desc>Test Description</desc>
                <src>Test Source</src>
                <sym>Test Symbol</sym>
                <type>Test Type</type>
            </gpx>";
        var xElement = XElement.Parse(gpxEntityXml);
        var gpxEntity = new GpxDocument.GpxEntity(xElement);
        // Act & Assert
        Assert.AreEqual("Test Name", gpxEntity.Name);
        Assert.AreEqual("Test Comment", gpxEntity.Comment);
        Assert.AreEqual("Test Description", gpxEntity.Description);
        Assert.AreEqual("Test Source", gpxEntity.Source);
        Assert.AreEqual("Test Symbol", gpxEntity.SymbolName);
        Assert.AreEqual("Test Type", gpxEntity.ClassificationType);
    }

    [TestMethod]
    public void PropertyAccessors_WithEmptyElement_ReturnsExpectedValues()
    {
        // Arrange
        var gpxEntityXml = @"
            <gpx>
            </gpx>";
        var xElement = XElement.Parse(gpxEntityXml);
        var gpxEntity = new GpxDocument.GpxEntity(xElement);
        // Act & Assert
        Assert.AreEqual(string.Empty, gpxEntity.Name);
        Assert.AreEqual(string.Empty, gpxEntity.Comment);
        Assert.AreEqual(string.Empty, gpxEntity.Description);
        Assert.AreEqual(string.Empty, gpxEntity.Source);
        Assert.AreEqual(string.Empty, gpxEntity.SymbolName);
        Assert.AreEqual(string.Empty, gpxEntity.ClassificationType);
    }
}
