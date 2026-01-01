using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONGeometryCollectionTests
{
    [TestMethod]
    public void GeoJSONGeometryCollection_CreateWithEmptyGeometry()
    {
        var collection = new GeoJSONGeometryCollection() with { Geometries = [] };
        Assert.IsEmpty(collection.Geometries);
    }

    [TestMethod]
    public void GeoJSONGeometryCollection_WriteCoordinatesToTest()
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        var collection = new GeoJSONGeometryCollection();
        collection.WriteCoordinatesTo(writer);  // should write nothing and not throw
        writer.Flush();
        Assert.AreEqual(0, stream.Length);
    }
}
