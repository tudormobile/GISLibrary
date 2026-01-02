using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONBoundingBoxTests
{
    [TestMethod]
    public void Parse_ValidTwoDimensionalBoundingBox_ReturnsCorrectValues()
    {
        // Arrange
        string json = "[100.0, 0.0, 105.0, 1.0]";

        // Act
        var bbox = GeoJSONBoundingBox.Parse(json);

        // Assert
        Assert.IsNotNull(bbox);
        Assert.AreEqual(100.0, bbox.Value[0]);
        Assert.AreEqual(0.0, bbox.Value[1]);
        Assert.AreEqual(105.0, bbox.Value[2]);
        Assert.AreEqual(1.0, bbox.Value[3]);
    }

    [TestMethod]
    public void Parse_ValidEmptyObject_ReturnsNull()
    {
        // Arrange
        string json = "{}";

        // Act
        var bbox = GeoJSONBoundingBox.Parse(json);

        // Assert
        Assert.IsNull(bbox);
    }

    [TestMethod]
    public void Parse_InvalidStringArray_ReturnsEmptyArray()
    {
        // Arrange
        string json = "[\"test\"]";

        // Act
        var bbox = GeoJSONBoundingBox.Parse(json);

        // Assert
        Assert.IsNotNull(bbox);
        Assert.IsEmpty(bbox.Value);
    }


}