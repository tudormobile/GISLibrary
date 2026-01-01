using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONDocumentTests
{
    private const string SimpleFeatureCollection = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[1.0,2.0]},\"properties\":{\"name\":\"A\"}}]}";

    [TestMethod]
    public async Task ParseFeatureCollection_LoadsFeatures()
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(SimpleFeatureCollection));
        var doc = await GeoJSONDocument.ParseAsync(stream, TestContext.CancellationToken);
        Assert.IsNotNull(doc);
        var features = doc.FeatureCollection.Features;
        Assert.HasCount(1, features);
        var feature = features[0];
        Assert.AreEqual("Point", feature.Geometry.GetProperty("type").GetString());
        Assert.AreEqual("A", feature.Properties["name"].GetString());
    }

    [TestMethod]
    public void FeatureConstructor_InvalidType_Throws()
    {
        var json = JsonDocument.Parse("{\"type\":\"NotAFeature\"}");
        Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONFeature(json.RootElement));
    }

    [TestMethod]
    public void FeatureCollectionConstructor_InvalidType_Throws()
    {
        var json = JsonDocument.Parse("{\"type\":\"NotACollection\"}");
        Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONFeatureCollection(json.RootElement));
    }

    [TestMethod]
    public void GeometryConstructor_InvalidType_Throws()
    {
        var json = JsonDocument.Parse("{\"type\":\"Unknown\"}");
        Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONGeometry(json.RootElement));
    }

    [TestMethod]
    public async Task GeoJSONFile_ReadWrite_Roundtrip()
    {
        var temp = Path.GetTempFileName();
        try
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SimpleFeatureCollection));
            var doc = await GeoJSONDocument.ParseAsync(stream, TestContext.CancellationToken);
            var file = new GeoJSONFile(temp);
            await file.WriteDocumentAsync(doc, TestContext.CancellationToken);
            Assert.IsTrue(file.Exists());
            var read = await file.ReadDocumentAsync(TestContext.CancellationToken);
            Assert.IsNotNull(read);
            Assert.AreEqual("A", read.FeatureCollection.Features[0].Properties["name"].GetString());
        }
        finally
        {
            if (File.Exists(temp))
            {
                File.Delete(temp);
            }
        }
    }

    [TestMethod]
    public async Task GetJSONDocument_SaveAsyncTest()
    {
        // Arrange
        var stream = new MemoryStream();
        var strings = new GeoJSONMultiLineString()
        {
            LineStrings = [
                new GeoJSONLineString(
                    [
                        new GeoJSONPosition(10.0, 20.0),
                        new GeoJSONPosition(30.0, 40.0)
                    ]),
                new GeoJSONLineString(
                    [
                        new GeoJSONPosition(50.0, 60.0),
                        new GeoJSONPosition(70.0, 80.0)
                    ])
            ]
        };
        var polygon = new GeoJSONPolygon
        {
            Rings = [
                new GeoJSONLineString(
                    [
                        new GeoJSONPosition(0.0, 0.0),
                        new GeoJSONPosition(0.0, 10.0),
                        new GeoJSONPosition(10.0, 10.0),
                        new GeoJSONPosition(10.0, 0.0),
                        new GeoJSONPosition(0.0, 0.0)
                    ])
            ]
        };
        var multipoly = new GeoJSONMultiPolygon
        {
            Polygons = [polygon]
        };


        var document = GeoJSONDocument.Create()
        .AddFeature(f => f
            .SetGeometry([multipoly, polygon, strings])
            .AddObject("id", 4)
            .AddProperty("description", "This is a multi-geometry feature"))
        .Build();

        //Act
        await document.SaveAsync(stream, TestContext.CancellationToken);

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("GeometryCollection", json);
    }

    [TestMethod]
    public async Task GetJSONDocument_SaveAsync_WithINVALIDGeometryTest()
    {
        // Arrange
        var stream = new MemoryStream();
        var badGeo = new BadGeometry();
        var document = GeoJSONDocument.Create()
        .AddFeature(f => f.SetGeometry(badGeo))
        .Build();

        //Act & Assert
        _ = Assert.ThrowsExactlyAsync<NotImplementedException>(async () => await document.SaveAsync(stream, TestContext.CancellationToken));
    }

    public TestContext TestContext { get; set; }
}

[ExcludeFromCodeCoverage]
public record BadGeometry : GeoJSONCoordinates
{
    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        Assert.Fail("Should never get here!)");
    }
}
