using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONPolygonTests
{
    [TestMethod]
    public void CreateWithRingsTest()
    {
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
        Assert.HasCount(2, polygon.Rings);
        Assert.AreEqual(ring1, polygon.Rings[0]);
        Assert.AreEqual(ring2, polygon.Rings[1]);
    }

    [TestMethod]
    public void WriteCoordinatesToTest()
    {
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
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        polygon.WriteCoordinatesTo(writer);
        writer.Flush();

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var expectedJson = "[[[0,0],[10,0],[10,10],[0,10],[0,0]],[[2,2],[8,2],[8,8],[2,8],[2,2]]]";
        Assert.AreEqual(expectedJson, json);
    }
}
