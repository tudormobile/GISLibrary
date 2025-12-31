using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Tudormobile.GISLibrary.Tests
{
    [TestClass]
    public class GeometryBuilderTests
    {
        [TestMethod, DeploymentItem("test.geojson")]
        public void FromJsonTest()
        {
            var filename = "test.geojson";
            var json = File.ReadAllText(filename);
            var jo = JsonObject.Parse(json)?["features"]?[0]?["geometry"];

            // Now, test the builder

            var actual = GeometryBuilder.FromJson(jo!.ToString());
            Assert.AreEqual(GeometryType.MultiPolygon, actual.GeometryType);
            Assert.HasCount(27, ((GeoMultiPolygon)actual).Polygons);
        }

        [TestMethod, DeploymentItem("test.geojson")]
        public void FeaturesFromJsonTest()
        {
            var filename = "test.geojson";
            var json = File.ReadAllText(filename);

            // Now, test the builder

            var actual = GeometryBuilder.FeaturesFromJson(json);
            foreach (var f in actual)
            {
                Debug.WriteLine($"{f.Properties["District"]} {f.Properties["Name"]} {f.Properties["Email"]} {f.Geometry.GeometryType}");
            }
        }
        [TestMethod, DeploymentItem("nys_counties.geojson")]
        public void CountiesFromJsonTest()
        {
            var filename = "nys_counties.geojson";
            var json = File.ReadAllText(filename);

            // Now, test the builder

            var actual = GeometryBuilder.FeaturesFromJson(json);
            foreach (var f in actual)
            {
                Debug.WriteLine($"{f.Properties["NAME"]} {f.Properties["ABBREV"]} {f.Properties["DATEMOD"]} {f.Geometry.GeometryType}");
            }
        }

    }
}