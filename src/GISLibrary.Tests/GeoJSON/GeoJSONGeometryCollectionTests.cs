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
}
