using System.Text;
using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON
{
    [TestClass]
    public class GeoJSONMultiLineStringTests
    {
        [TestMethod]
        public void CreateWithPositionsTest()
        {
            var geoJSONLineString1 = new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0), new GeoJSONPosition(30.0, 40.0)]);
            var geoJSONLineString2 = new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0), new GeoJSONPosition(30.0, 40.0)]);
            var multiLineString = new GeoJSONMultiLineString()
            {
                LineStrings = [geoJSONLineString1, geoJSONLineString2]
            };
            Assert.HasCount(2, multiLineString.LineStrings);
        }

        [TestMethod]
        public void WriteCoordinatesToTest()
        {
            // Arrange
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            var geoJSONLineString1 = new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0), new GeoJSONPosition(30.0, 40.0)]);
            var geoJSONLineString2 = new GeoJSONLineString([new GeoJSONPosition(10.0, 20.0), new GeoJSONPosition(30.0, 40.0)]);
            var multiLineString = new GeoJSONMultiLineString()
            {
                LineStrings = [geoJSONLineString1, geoJSONLineString2]
            };

            // Act
            multiLineString.WriteCoordinatesTo(writer);
            writer.Flush();

            // Assert
            var json = Encoding.UTF8.GetString(stream.ToArray());
            var expectedJson = "[[[20,10],[40,30]],[[20,10],[40,30]]]";
            Assert.AreEqual(expectedJson, json);
        }

    }
}
