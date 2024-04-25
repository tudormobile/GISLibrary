using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.GISLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Diagnostics;

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
            Assert.AreEqual(27, ((GeoMultiPolygon)actual).Polygons.Length);
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