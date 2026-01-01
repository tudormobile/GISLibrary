using System.Text;
using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONMultiPolygonTests
{
    [TestMethod]
    public void GeoJSONMultiPolygon_WriteCoordinatesTest()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        var ring1 = new GeoJSONLineString(
                [
                    new GeoJSONPosition(0.0, 0.0),
            new GeoJSONPosition(0.0, 10.0),
            new GeoJSONPosition(10.0, 10.0),
            new GeoJSONPosition(10.0, 0.0),
            new GeoJSONPosition(0.0, 0.0)
                ]);
        var ring2 = new GeoJSONLineString(
        [
            new  GeoJSONPosition(2.0, 2.0),
            new  GeoJSONPosition(2.0, 8.0),
            new  GeoJSONPosition(8.0, 8.0),
            new  GeoJSONPosition(8.0, 2.0),
            new  GeoJSONPosition(2.0, 2.0)
        ]);
        var polygon = new GeoJSONPolygon
        {
            Rings = [ring1, ring2]
        };
        var multiPolygon = new GeoJSONMultiPolygon
        {
            Polygons = [polygon]
        };
        // Act
        multiPolygon.WriteCoordinatesTo(writer);
        writer.Flush();
        var json = Encoding.UTF8.GetString(stream.ToArray());
        // Assert
        var expectedJson = "[[[[0,0],[10,0],[10,10],[0,10],[0,0]],[[2,2],[8,2],[8,8],[2,8],[2,2]]]]";
        Assert.AreEqual(expectedJson, json);
    }
}
