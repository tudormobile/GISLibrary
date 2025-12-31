using System.Text.Json;

namespace Tudormobile.GeoJSON.Tests
{
    [TestClass]
    public class GeoJSONModelsTests
    {
        [TestMethod]
        public void GeoJSONCoordinates_CanInstantiate()
        {
            var c = new GeoJSONCoordinates();
            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void GeoJSONPosition_GetSet()
        {
            var pos = new GeoJSONPosition { Longitude = 10.5, Latitude = -20.25, Altitude = 100.0 };
            Assert.AreEqual(10.5, pos.Longitude);
            Assert.AreEqual(-20.25, pos.Latitude);
            Assert.AreEqual(100.0, pos.Altitude);
        }

        [TestMethod]
        public void GeoJSONPoint_SetPosition()
        {
            var p = new GeoJSONPoint { Position = new GeoJSONPosition { Longitude = 1, Latitude = 2 } };
            Assert.AreEqual(1, p.Position.Longitude);
            Assert.AreEqual(2, p.Position.Latitude);
        }

        [TestMethod]
        public void GeoJSONMultiPoint_SetPoints()
        {
            var mp = new GeoJSONMultiPoint
            {
                Points =
                [
                    new GeoJSONPoint { Position = new GeoJSONPosition { Longitude = 1, Latitude = 2 } },
                    new GeoJSONPoint { Position = new GeoJSONPosition { Longitude = 3, Latitude = 4 } }
                ]
            };
            Assert.HasCount(2, mp.Points);
            Assert.AreEqual(3, mp.Points[1].Position.Longitude);
        }

        [TestMethod]
        public void GeoJSONLineString_SetPositions()
        {
            var ls = new GeoJSONLineString
            {
                Positions =
                [
                    new GeoJSONPosition { Longitude = 0, Latitude = 0 },
                    new GeoJSONPosition { Longitude = 1, Latitude = 1 }
                ]
            };
            Assert.HasCount(2, ls.Positions);
            Assert.AreEqual(1, ls.Positions[1].Latitude);
        }

        [TestMethod]
        public void GeoJSONMultiLineString_SetLineStrings()
        {
            var mls = new GeoJSONMultiLineString
            {
                LineStrings =
                [
                    new GeoJSONLineString { Positions = [new GeoJSONPosition { Longitude = 1, Latitude = 1 }] }
                ]
            };
            Assert.HasCount(1, mls.LineStrings);
            Assert.HasCount(1, mls.LineStrings[0].Positions);
        }

        [TestMethod]
        public void GeoJSONPolygon_SetRings()
        {
            var poly = new GeoJSONPolygon
            {
                Rings =
                [
                    new GeoJSONLineString { Positions = [new GeoJSONPosition { Longitude = 1, Latitude = 2 }] }
                ]
            };
            Assert.HasCount(1, poly.Rings);
        }

        [TestMethod]
        public void GeoJSONMultiPolygon_SetPolygons()
        {
            var mp = new GeoJSONMultiPolygon
            {
                Polygons =
                [
                    new GeoJSONPolygon { Rings = [new GeoJSONLineString { Positions = [] }] }
                ]
            };
            Assert.HasCount(1, mp.Polygons);
        }

        //[TestMethod]
        //public void GeoJSONGeometryCollection_SetGeometries()
        //{
        //    using var doc = JsonDocument.Parse("{\"type\":\"Point\",\"coordinates\":[1,2]}");
        //    var geom = new GeoJSONGeometry(doc.RootElement);
        //    var coll = new GeoJSONGeometryCollection
        //    {
        //        Geometries = [geom]
        //    };
        //    Assert.HasCount(1, coll.Geometries);
        //    Assert.AreEqual("Point", coll.Geometries[0].Type);
        //}

        [TestMethod]
        public void GeoJSONGeometry_ValidPoint_CreatesPointCoordinates()
        {
            using var doc = JsonDocument.Parse("{\"type\":\"Point\",\"coordinates\":[10.1,20.2,30.3]}");
            var geom = new GeoJSONGeometry(doc.RootElement);
            Assert.AreEqual("Point", geom.Type);
            Assert.IsInstanceOfType<GeoJSONPoint>(geom.Coordinates);
            var p = (GeoJSONPoint)geom.Coordinates;
            Assert.AreEqual(10.1, p.Position.Longitude);
            Assert.AreEqual(20.2, p.Position.Latitude);
            Assert.AreEqual(30.3, p.Position.Altitude);
        }
    }
}
