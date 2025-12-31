using System.Text.Json;

namespace Tudormobile.GeoJSON.Tests
{
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
                var doc = await GeoJSONDocument.ParseAsync(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(SimpleFeatureCollection)), TestContext.CancellationToken);
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

        public TestContext TestContext { get; set; }
    }
}
