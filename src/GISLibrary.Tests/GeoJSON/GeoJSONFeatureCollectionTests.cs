using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONFeatureCollectionTests
{
    [TestMethod]
    public void GeoJSONFeatureCollection_CreateWithEmptyFeatures()
    {
        var json = @"
        {
            ""type"": ""FeatureCollection"",
            ""features"": []
        }";
        var rootElement = JsonElement.Parse(json);
        var collection = new GeoJSONFeatureCollection(rootElement);
        Assert.IsNotNull(collection.Features);
        Assert.HasCount(0, collection.Features);
    }

    [TestMethod]
    public void GeoJSONFeatureCollection_CreateWithMissingFeatures()
    {
        var json = @"
        {
            ""type"": ""FeatureCollection""
        }";
        var rootElement = JsonElement.Parse(json);
        var collection = new GeoJSONFeatureCollection(rootElement);
        Assert.IsNotNull(collection.Features);
        Assert.HasCount(0, collection.Features);
    }

}
