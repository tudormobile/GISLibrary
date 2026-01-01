using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

[TestClass]
public class GeoJSONFileTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DeploymentItem("NYS_Congressional_Districts.geojson")]
    public async Task GeoJSONFile_WithSampleFile_ReadDocumentSucceeds()
    {
        var path = "NYS_Congressional_Districts.geojson";
        var file = new GeoJSONFile(path);
        Assert.IsTrue(File.Exists(path));
        Assert.IsTrue(file.Exists());
        var doc = await file.ReadDocumentAsync(TestContext.CancellationToken);
        Assert.HasCount(26, doc.FeatureCollection.Features);
        Assert.AreEqual(1, doc.FeatureCollection.Features[0].Properties["District"].GetInt64());
        Assert.AreEqual(7, doc.FeatureCollection.Features.Count(f => f.Properties["Party"].GetString() == "Republican"));
        Assert.AreEqual(19, doc.FeatureCollection.Features.Count(f => f.Properties["Party"].GetString() == "Democratic"));
    }

    [TestMethod]
    [DeploymentItem("test.geojson")]
    public void GeoJSONFile_WithSampleFile_HasProperties()
    {
        var path = "test.geojson";
        var file = new GeoJSONFile(path);
        Assert.IsTrue(file.Exists());
        Assert.IsGreaterThan(0, file.GetFileSize());
        Assert.IsTrue(file.GetLastModifiedTime() > DateTime.MinValue);
    }

    [TestMethod]
    public void GeoJSONFile_WithMissingFile_NoErrors()
    {
        var path = "missing.geojson";
        var file = new GeoJSONFile(path);
        Assert.IsFalse(file.Exists());
        Assert.AreEqual(DateTime.MinValue, file.GetLastModifiedTime());
        Assert.AreEqual(0L, file.GetFileSize());
    }

    [TestMethod]
    public async Task GeoJSONFile_WriteDocument_CreatesFile()
    {
        var temp = Path.GetTempFileName();
        try
        {
            // Different ways to create GeoJSON geometries
            var position1 = new GeoJSONPosition([1.0, 2.0, 3.0]);
            var position2 = new GeoJSONPosition(1.0, 2.0);
            var point1 = new GeoJSONPoint(4.0, 5.0, 6.0);
            var point2 = new GeoJSONPoint([7.0, 8.0]);
            var point3 = new GeoJSONPoint(position1);

            var multipoint1 = new GeoJSONMultiPoint([point1, point2, point3]);
            var multipoint2 = new GeoJSONMultiPoint([position1]);

            var linestring1 = new GeoJSONLineString([position1, point2, point3]);

            var file = new GeoJSONFile(temp);
            var document = GeoJSONDocument.Create()
                .AddObject("crs", new { type = "name", properties = new { name = "EPSG:4326" } })
                .AddProperty("name", "Test GeoJSON Document")
                .AddFeature(f => f
                    .SetGeometry(point1)
                    .AddObject("id", 1)
                    .AddProperty("description", "This is point 1"))
                .AddFeature(f => f
                    .SetGeometry(multipoint1)
                    .AddObject("id", 2)
                    .AddProperty("description", "This is a multipoint"))
                .AddFeature(f => f
                    .SetGeometry(linestring1)
                    .AddObject("id", 3)
                    .AddProperty("description", "This is a linestring"))
                .AddFeature(f => f
                    .SetGeometry([point1, point2, point3, multipoint2, linestring1])
                    .AddObject("id", 4)
                    .AddProperty("description", "This is a multi-geometry feature"))
                .Build();
            await file.WriteDocumentAsync(document, TestContext.CancellationToken);

            Assert.IsTrue(File.Exists(temp));
        }
        finally
        {
            if (File.Exists(temp))
            {
                File.Delete(temp);
            }
        }
    }

}
