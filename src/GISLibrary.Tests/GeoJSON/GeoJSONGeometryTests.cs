using System.Text.Json;

namespace Tudormobile.GeoJSON.Tests;

[TestClass]
public class GeoJSONGeometryTests
{
    [TestMethod]
    public void GeoJSONPoint_ConstructWithPosition()
    {
        var position = new GeoJSONPosition()
        {
            Latitude = 1.0,
            Longitude = 2.0,
            Altitude = 3.0
        };
        var p = new GeoJSONPoint() with { Position = position };
        Assert.AreEqual(1.0, p.Position.Latitude);
        Assert.AreEqual(2.0, p.Position.Longitude);
        Assert.AreEqual(3.0, p.Position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithPosition_NoAltitude()
    {
        var position = new GeoJSONPosition()
        {
            Latitude = 1.0,
            Longitude = 2.0,
            Altitude = null
        };
        var p = new GeoJSONPoint() with { Position = position };
        Assert.AreEqual(1.0, p.Position.Latitude);
        Assert.AreEqual(2.0, p.Position.Longitude);
        Assert.IsNull(p.Position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithJson_TwoCoordinates()
    {
        var json = @"{
            ""type"": ""Point"",
            ""coordinates"": [1.0, 2.0]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var p = (GeoJSONPoint)position.Coordinates;
        Assert.AreEqual("Point", position.Type);
        Assert.AreEqual(1.0, p.Position.Longitude);
        Assert.AreEqual(2.0, p.Position.Latitude);
        Assert.IsNull(p.Position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithJson_ThreeCoordinates()
    {
        var json = @"{
            ""type"": ""Point"",
            ""coordinates"": [2.0, 1.0, 3.0]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var p = (GeoJSONPoint)position.Coordinates;
        Assert.AreEqual(1.0, p.Position.Latitude);
        Assert.AreEqual(2.0, p.Position.Longitude);
        Assert.AreEqual(3.0, p.Position.Altitude);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithJson_OneCoordinateThrows()
    {
        var json = @"{
            ""type"": ""Point"",
            ""coordinates"": [1.0]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var p = Assert.ThrowsExactly<IndexOutOfRangeException>(() => (GeoJSONPoint)position.Coordinates);
    }

    [TestMethod]
    public void GeoJSONPoint_ConstructWithJson_FourCoordinatesThrows()
    {
        var json = @"{
            ""type"": ""Point"",
            ""coordinates"": [1.0, 2.0, 3.0, 4.0]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var p = Assert.ThrowsExactly<IndexOutOfRangeException>(() => (GeoJSONPoint)position.Coordinates);
    }

    [TestMethod]
    public void GeoJSONMultiPoint_ConstructWithJson()
    {
        var p1 = new GeoJSONPoint() with { Position = new GeoJSONPosition() { Longitude = 1.0, Latitude = 2.0 } };
        var p2 = new GeoJSONPoint() with { Position = new GeoJSONPosition() { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 } };
        var expected = new GeoJSONMultiPoint() with { Points = [p1, p2] };

        var json = @"{
            ""type"": ""MultiPoint"",
            ""coordinates"": [
                [1.0, 2.0],
                [3.0, 4.0, 5.0]
            ]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var mp = (GeoJSONMultiPoint)position.Coordinates;
        Assert.HasCount(2, mp.Points);
        Assert.AreEqual(2.0, mp.Points[0].Position.Latitude);
        Assert.AreEqual(1.0, mp.Points[0].Position.Longitude);
        Assert.IsNull(mp.Points[0].Position.Altitude);
        Assert.AreEqual(4.0, mp.Points[1].Position.Latitude);
        Assert.AreEqual(3.0, mp.Points[1].Position.Longitude);
        Assert.AreEqual(5.0, mp.Points[1].Position.Altitude);

        CollectionAssert.AreEqual(expected.Points, mp.Points);
    }

    [TestMethod]
    public void GeoJSONLineString_ConstructWithJson()
    {
        var p1 = new GeoJSONPosition() with { Longitude = 1.0, Latitude = 2.0 };
        var p2 = new GeoJSONPosition() with { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 };
        var p3 = new GeoJSONPosition() with { Longitude = 6.0, Latitude = 7.0 };
        var expected = new GeoJSONLineString() with { Positions = [p1, p2, p3] };

        var json = @"{
            ""type"": ""LineString"",
            ""coordinates"": [
                [1.0, 2.0],
                [3.0, 4.0, 5.0],
                [6.0, 7.0]
            ]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var actual = (GeoJSONLineString)position.Coordinates;
        Assert.HasCount(3, actual.Positions);
        Assert.AreEqual(2.0, actual.Positions[0].Latitude);
        Assert.AreEqual(1.0, actual.Positions[0].Longitude);
        Assert.IsNull(actual.Positions[0].Altitude);
        Assert.AreEqual(4.0, actual.Positions[1].Latitude);
        Assert.AreEqual(3.0, actual.Positions[1].Longitude);
        Assert.AreEqual(5.0, actual.Positions[1].Altitude);
        Assert.AreEqual(7.0, actual.Positions[2].Latitude);
        Assert.AreEqual(6.0, actual.Positions[2].Longitude);
        Assert.IsNull(actual.Positions[2].Altitude);

        CollectionAssert.AreEqual(expected.Positions, actual.Positions);
    }

    [TestMethod]
    public void GeoJSONMultiLineString_ConstructWithJson()
    {
        var p1 = new GeoJSONPosition() with { Longitude = 1.0, Latitude = 2.0 };
        var p2 = new GeoJSONPosition() with { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 };
        var p3 = new GeoJSONPosition() with { Longitude = 6.0, Latitude = 7.0 };

        var ls1 = new GeoJSONLineString() with { Positions = [p1, p2] };
        var ls2 = new GeoJSONLineString() with { Positions = [p2, p3] };
        var ls3 = new GeoJSONLineString() with { Positions = [p1, p3] };

        var expected = new GeoJSONMultiLineString() with { LineStrings = [ls1, ls2, ls3] };

        var json = @"
        {
            ""type"": ""MultiLineString"",
            ""coordinates"": [
                [
                    [1.0, 2.0],
                    [3.0, 4.0, 5.0]
                ],
                [
                    [3.0, 4.0, 5.0],
                    [6.0, 7.0]
                ],
                [
                    [1.0, 2.0],
                    [6.0, 7.0]
                ]
            ]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var actual = (GeoJSONMultiLineString)position.Coordinates;

        Assert.HasCount(3, actual.LineStrings);
        for (int i = 0; i < expected.LineStrings.Count; i++)
        {
            var expectedLineString = expected.LineStrings[i];
            var actualLineString = actual.LineStrings[i];
            Assert.HasCount(expectedLineString.Positions.Count, actualLineString.Positions);
            CollectionAssert.AreEqual(expectedLineString.Positions, actualLineString.Positions);
        }
    }

    [TestMethod]
    public void GeoJSONPolygon_ConstructWithJson()
    {
        var p1 = new GeoJSONPosition() with { Longitude = 1.0, Latitude = 2.0 };
        var p2 = new GeoJSONPosition() with { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 };
        var p3 = new GeoJSONPosition() with { Longitude = 6.0, Latitude = 7.0 };
        var p4 = p1; // Closing the polygon

        var r1 = new GeoJSONLineString() with { Positions = [p1, p2, p3, p4] };

        var expected = new GeoJSONPolygon() with { Rings = [r1] };

        var json = @"
        {
            ""type"": ""Polygon"",
            ""coordinates"": [
                [
                    [1.0, 2.0],
                    [3.0, 4.0, 5.0],
                    [6.0, 7.0],
                    [1.0, 2.0]
                ]
            ]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var actual = (GeoJSONPolygon)position.Coordinates;

        Assert.HasCount(1, actual.Rings);
        CollectionAssert.AreEqual(expected.Rings[0].Positions, actual.Rings[0].Positions);

    }

    [TestMethod]
    public void GeoJSONMultiPolygon_ConstructWithJson()
    {
        var p1 = new GeoJSONPosition() with { Longitude = 1.0, Latitude = 2.0 };
        var p1x = new GeoJSONPosition() with { Longitude = 2.5, Latitude = 3.5 };
        var p2 = new GeoJSONPosition() with { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 };
        var p2x = new GeoJSONPosition() with { Longitude = 3.5, Latitude = 4.5 };
        var p3 = new GeoJSONPosition() with { Longitude = 6.0, Latitude = 7.0 };
        var p4 = p1; // Closing the polygon

        var r1 = new GeoJSONLineString() with { Positions = [p1, p2, p3, p4] };
        var poly1 = new GeoJSONPolygon() with { Rings = [r1] };

        var r2 = new GeoJSONLineString() with { Positions = [p1, p1x, p2x, p1] };
        var poly2 = new GeoJSONPolygon() with { Rings = [r2] };

        var expected = new GeoJSONMultiPolygon() with { Polygons = [poly1, poly2] };

        var json = @"
        {
            ""type"": ""MultiPolygon"",
            ""coordinates"": [
                [
                    [
                        [1.0, 2.0],
                        [3.0, 4.0, 5.0],
                        [6.0, 7.0],
                        [1.0, 2.0]
                    ]
                ],
                [
                    [
                        [1.0, 2.0],
                        [2.5, 3.5],
                        [3.5, 4.5],
                        [1.0, 2.0]
                    ]
                ]
            ]
        }";
        var element = JsonElement.Parse(json);
        var position = new GeoJSONGeometry(element);
        var actual = (GeoJSONMultiPolygon)position.Coordinates;

        Assert.HasCount(2, actual.Polygons);
        for (int i = 0; i < expected.Polygons.Count; i++)
        {
            var expectedPolygon = expected.Polygons[i];
            var actualPolygon = actual.Polygons[i];
            Assert.HasCount(expectedPolygon.Rings.Count, actualPolygon.Rings);
            for (int j = 0; j < expectedPolygon.Rings.Count; j++)
            {
                var expectedRing = expectedPolygon.Rings[j];
                var actualRing = actualPolygon.Rings[j];
                Assert.HasCount(expectedRing.Positions.Count, actualRing.Positions);
                CollectionAssert.AreEqual(expectedRing.Positions, actualRing.Positions);
            }
        }
    }

    [TestMethod]
    public void GeoJSONGeometeryCollection_ConstructWithJson()
    {
        var p1 = new GeoJSONPoint() with { Position = new GeoJSONPosition() { Longitude = 1.0, Latitude = 2.0 } };
        var p2 = new GeoJSONPoint() with { Position = new GeoJSONPosition() { Longitude = 3.0, Latitude = 4.0, Altitude = 5.0 } };

        var g1 = p1;
        var g2 = new GeoJSONMultiPoint() with { Points = [p1, p2] };

        var expected = new GeoJSONGeometryCollection()
            .AddGeometry(g1).AddGeometry(g2);

        var json = @"
        {
            ""type"": ""GeometryCollection"",
            ""geometries"": [
                {
                    ""type"": ""Point"",
                    ""coordinates"": [1.0, 2.0]
                },
                {
                    ""type"": ""MultiPoint"",
                    ""coordinates"": [
                        [1.0, 2.0],
                        [3.0, 4.0, 5.0]
                    ]
                }
            ]
        }";
        var element = JsonElement.Parse(json);
        var geometry = new GeoJSONGeometry(element);
        var actual = (GeoJSONGeometryCollection)geometry.Coordinates;

        Assert.HasCount(2, actual.Geometries);
        var actualG1 = (GeoJSONPoint)actual.Geometries[0];
        Assert.AreEqual(2.0, actualG1.Position.Latitude);
        Assert.AreEqual(1.0, actualG1.Position.Longitude);
        Assert.IsNull(actualG1.Position.Altitude);
        var actualG2 = (GeoJSONMultiPoint)actual.Geometries[1];
        Assert.HasCount(2, actualG2.Points);
        Assert.AreEqual(2.0, actualG2.Points[0].Position.Latitude);
        Assert.AreEqual(1.0, actualG2.Points[0].Position.Longitude);
        Assert.IsNull(actualG2.Points[0].Position.Altitude);
        Assert.AreEqual(4.0, actualG2.Points[1].Position.Latitude);
        Assert.AreEqual(3.0, actualG2.Points[1].Position.Longitude);
        Assert.AreEqual(5.0, actualG2.Points[1].Position.Altitude);

    }

    [TestMethod]
    public void GeoJSONGeometry_ConstructWithInvalidTypeThrows()
    {
        var json = @"{
            ""type"": ""InvalidType"",
            ""coordinates"": [1.0, 2.0]
        }";
        var element = JsonElement.Parse(json);
        var exception = Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONGeometry(element));
        Assert.AreEqual("Type", exception.ParamName);
    }

    [TestMethod]
    public void GeoJSONGeometry_ConstructWithMissingTypeThrows()
    {
        var json = @"{
            ""coordinates"": [1.0, 2.0]
        }";
        var element = JsonElement.Parse(json);
        var exception = Assert.ThrowsExactly<ArgumentException>(() => new GeoJSONGeometry(element));
        Assert.AreEqual("Type", exception.ParamName);
    }

    [TestMethod]
    public void GeoJSONGeometry_ConstructWithForcedInvalidTypeThrows()
    {
        var json = @"{
            ""type"" : ""Point"",
            ""coordinates"": [1.0, 2.0]
        }";
        var element = JsonElement.Parse(json);
        var geometry = new GeoJSONGeometry(element);
        geometry.TestGeometryType = (GeoJSONDocument.GeoJSONGeometryType)999;
        Assert.AreEqual((GeoJSONDocument.GeoJSONGeometryType)999, geometry.TestGeometryType);
        var exception = Assert.ThrowsExactly<NotSupportedException>(() => geometry.Coordinates);
        Assert.AreEqual("Geometry type '999' is not supported.", exception.Message);
    }


}
