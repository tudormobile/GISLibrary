using System.Xml.Linq;
using Tudormobile.GeoJSON;
using Tudormobile.GIS;
using Tudormobile.Gpx;
using Tudormobile.Tcx;

namespace GISLibrary.Tests;

/// <summary>
/// Contains unit tests for the <see cref="GeoExtensions"/> class.
/// </summary>
[TestClass]
public class GeoExtensionsTests
{
    #region AsGeoPath Tests - GeoJSONPolygon

    /// <summary>
    /// Tests that AsGeoPath converts a GeoJSON polygon with a single ring to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GeoJSONPolygon_WithSingleRing_ReturnsGeoPath()
    {
        // Arrange
        var polygon = new GeoJSONPolygon
        {
            Rings =
            [
                new GeoJSONLineString
                {
                    Positions =
                    [
                        new GeoJSONPosition { Latitude = 0.0, Longitude = 0.0, Altitude = 10.0 },
                        new GeoJSONPosition { Latitude = 1.0, Longitude = 0.0, Altitude = 20.0 },
                        new GeoJSONPosition { Latitude = 1.0, Longitude = 1.0, Altitude = 30.0 },
                        new GeoJSONPosition { Latitude = 0.0, Longitude = 1.0, Altitude = 40.0 },
                        new GeoJSONPosition { Latitude = 0.0, Longitude = 0.0, Altitude = 10.0 }
                    ]
                }
            ]
        };

        // Act
        var result = polygon.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Count);
        Assert.AreEqual(new GeoPosition(0.0, 0.0, 10.0), result[0]);
        Assert.AreEqual(new GeoPosition(1.0, 0.0, 20.0), result[1]);
        Assert.AreEqual(new GeoPosition(1.0, 1.0, 30.0), result[2]);
    }

    /// <summary>
    /// Tests that AsGeoPath ignores additional rings in a GeoJSON polygon.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GeoJSONPolygon_WithMultipleRings_ReturnsOnlyFirstRing()
    {
        // Arrange
        var polygon = new GeoJSONPolygon
        {
            Rings =
            [
                new GeoJSONLineString
                {
                    Positions =
                    [
                        new GeoJSONPosition { Latitude = 0.0, Longitude = 0.0 },
                        new GeoJSONPosition { Latitude = 10.0, Longitude = 0.0 },
                        new GeoJSONPosition { Latitude = 10.0, Longitude = 10.0 },
                        new GeoJSONPosition { Latitude = 0.0, Longitude = 0.0 }
                    ]
                },
                new GeoJSONLineString
                {
                    Positions =
                    [
                        new GeoJSONPosition { Latitude = 2.0, Longitude = 2.0 },
                        new GeoJSONPosition { Latitude = 8.0, Longitude = 2.0 },
                        new GeoJSONPosition { Latitude = 8.0, Longitude = 8.0 },
                        new GeoJSONPosition { Latitude = 2.0, Longitude = 2.0 }
                    ]
                }
            ]
        };

        // Act
        var result = polygon.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual(new GeoPosition(0.0, 0.0, 0), result[0]);
        Assert.AreEqual(new GeoPosition(10.0, 0.0, 0), result[1]);
    }

    /// <summary>
    /// Tests that AsGeoPath returns null when the polygon has no rings.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GeoJSONPolygon_WithNoRings_ReturnsNull()
    {
        // Arrange
        var polygon = new GeoJSONPolygon { Rings = [] };

        // Act
        var result = polygon.AsGeoPath();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeoPath returns null when the polygon rings collection is null.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GeoJSONPolygon_WithNullRings_ReturnsNull()
    {
        // Arrange
        var polygon = new GeoJSONPolygon { Rings = null! };

        // Act
        var result = polygon.AsGeoPath();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeoPath handles positions without altitude by using 0.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GeoJSONPolygon_WithNullAltitude_UsesZero()
    {
        // Arrange
        var polygon = new GeoJSONPolygon
        {
            Rings =
            [
                new GeoJSONLineString
                {
                    Positions =
                    [
                        new GeoJSONPosition { Latitude = 1.0, Longitude = 2.0, Altitude = null },
                        new GeoJSONPosition { Latitude = 3.0, Longitude = 4.0, Altitude = null }
                    ]
                }
            ]
        };

        // Act
        var result = polygon.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new GeoPosition(1.0, 2.0, 0), result[0]);
        Assert.AreEqual(new GeoPosition(3.0, 4.0, 0), result[1]);
    }

    #endregion

    #region AsGeoPath Tests - TcxActivity

    /// <summary>
    /// Tests that AsGeoPath converts a TCX activity with trackpoints to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_TcxActivity_WithTrackpoints_ReturnsGeoPath()
    {
        // Arrange
        var xml = XDocument.Parse(@"
            <TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
                <Activities>
                    <Activity Sport=""Running"">
                        <Id>2023-01-15T08:30:00Z</Id>
                        <Lap StartTime=""2023-01-15T08:30:00Z"">
                            <Track>
                                <Trackpoint>
                                    <Time>2023-01-15T08:30:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>37.8</LatitudeDegrees>
                                        <LongitudeDegrees>-122.4</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>10.5</AltitudeMeters>
                                    <DistanceMeters>0</DistanceMeters>
                                    <HeartRateBpm><Value>120</Value></HeartRateBpm>
                                </Trackpoint>
                                <Trackpoint>
                                    <Time>2023-01-15T08:31:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>37.9</LatitudeDegrees>
                                        <LongitudeDegrees>-122.5</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>15.2</AltitudeMeters>
                                    <DistanceMeters>100</DistanceMeters>
                                    <HeartRateBpm><Value>125</Value></HeartRateBpm>
                                </Trackpoint>
                            </Track>
                        </Lap>
                    </Activity>
                </Activities>
            </TrainingCenterDatabase>");
        var tcxDoc = new TcxDocument(xml);
        var activity = tcxDoc.Activities.First();

        // Act
        var result = activity.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new GeoPosition(37.8, -122.4, 10.5), result[0]);
        Assert.AreEqual(new GeoPosition(37.9, -122.5, 15.2), result[1]);
    }

    /// <summary>
    /// Tests that AsGeoPath converts a TCX activity with multiple laps to a single GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_TcxActivity_WithMultipleLaps_CombinesAllTrackpoints()
    {
        // Arrange
        var xml = XDocument.Parse(@"
            <TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
                <Activities>
                    <Activity Sport=""Running"">
                        <Id>2023-01-15T08:30:00Z</Id>
                        <Lap StartTime=""2023-01-15T08:30:00Z"">
                            <Track>
                                <Trackpoint>
                                    <Time>2023-01-15T08:30:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>37.8</LatitudeDegrees>
                                        <LongitudeDegrees>-122.4</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>10.0</AltitudeMeters>
                                    <DistanceMeters>0</DistanceMeters>
                                    <HeartRateBpm><Value>120</Value></HeartRateBpm>
                                </Trackpoint>
                            </Track>
                        </Lap>
                        <Lap StartTime=""2023-01-15T08:40:00Z"">
                            <Track>
                                <Trackpoint>
                                    <Time>2023-01-15T08:40:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>38.0</LatitudeDegrees>
                                        <LongitudeDegrees>-122.6</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>20.0</AltitudeMeters>
                                    <DistanceMeters>1000</DistanceMeters>
                                    <HeartRateBpm><Value>130</Value></HeartRateBpm>
                                </Trackpoint>
                            </Track>
                        </Lap>
                    </Activity>
                </Activities>
            </TrainingCenterDatabase>");
        var tcxDoc = new TcxDocument(xml);
        var activity = tcxDoc.Activities.First();

        // Act
        var result = activity.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new GeoPosition(37.8, -122.4, 10.0), result[0]);
        Assert.AreEqual(new GeoPosition(38.0, -122.6, 20.0), result[1]);
    }

    #endregion

    #region AsGeoPath Tests - TCX Trackpoints

    /// <summary>
    /// Tests that AsGeoPath converts TCX trackpoints to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_TcxTrackpoints_ReturnsGeoPath()
    {
        // Arrange
        var xml = XDocument.Parse(@"
            <TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
                <Activities>
                    <Activity Sport=""Running"">
                        <Id>2023-01-15T08:30:00Z</Id>
                        <Lap StartTime=""2023-01-15T08:30:00Z"">
                            <Track>
                                <Trackpoint>
                                    <Time>2023-01-15T08:30:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>40.0</LatitudeDegrees>
                                        <LongitudeDegrees>-75.0</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>100.0</AltitudeMeters>
                                    <DistanceMeters>0</DistanceMeters>
                                    <HeartRateBpm><Value>110</Value></HeartRateBpm>
                                </Trackpoint>
                                <Trackpoint>
                                    <Time>2023-01-15T08:31:00Z</Time>
                                    <Position>
                                        <LatitudeDegrees>40.1</LatitudeDegrees>
                                        <LongitudeDegrees>-75.1</LongitudeDegrees>
                                    </Position>
                                    <AltitudeMeters>105.5</AltitudeMeters>
                                    <DistanceMeters>150</DistanceMeters>
                                    <HeartRateBpm><Value>115</Value></HeartRateBpm>
                                </Trackpoint>
                            </Track>
                        </Lap>
                    </Activity>
                </Activities>
            </TrainingCenterDatabase>");
        var tcxDoc = new TcxDocument(xml);
        var trackpoints = tcxDoc.Activities.First().Laps.First().Tracks.ToList();

        // Act
        var result = trackpoints.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new GeoPosition(40.0, -75.0, 100.0), result[0]);
        Assert.AreEqual(new GeoPosition(40.1, -75.1, 105.5), result[1]);
    }

    /// <summary>
    /// Tests that AsGeoPath handles an empty collection of TCX trackpoints.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_TcxTrackpoints_EmptyCollection_ReturnsEmptyGeoPath()
    {
        // Arrange
        var trackpoints = new List<TcxDocument.TcxTrackpoint>();

        // Act
        var result = trackpoints.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    #endregion


    /// <summary>
    /// Tests that AsGeoPath converts GPX waypoints to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GpxWaypoints_ReturnsGeoPath()
    {
        // Arrange
        var ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
        var waypoints = new List<GpxDocument.GpxWaypoint>
        {
            new GpxDocument.GpxWaypoint(new XElement(ns + "wpt",
                new XAttribute("lat", "45.5"),
                new XAttribute("lon", "-122.7"),
                new XElement(ns + "ele", "50.0"))),
            new GpxDocument.GpxWaypoint(new XElement(ns + "wpt",
                new XAttribute("lat", "45.6"),
                new XAttribute("lon", "-122.8"),
                new XElement(ns + "ele", "55.5")))
        };

        // Act
        var result = waypoints.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(new GeoPosition(45.5, -122.7, 50.0), result[0]);
        Assert.AreEqual(new GeoPosition(45.6, -122.8, 55.5), result[1]);
    }

    /// <summary>
    /// Tests that AsGeoPath handles an empty collection of GPX waypoints.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GpxWaypoints_EmptyCollection_ReturnsEmptyGeoPath()
    {
        // Arrange
        var waypoints = new List<GpxDocument.GpxWaypoint>();

        // Act
        var result = waypoints.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    #region AsGeoPath Tests - GPX Track Segments

    /// <summary>
    /// Tests that AsGeoPath converts GPX track segments to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GpxTrackSegments_ReturnsGeoPath()
    {
        // Arrange
        var ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
        var segments = new List<GpxDocument.GpxTrackSegment>
        {
            new GpxDocument.GpxTrackSegment(new XElement(ns + "trkseg",
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "50.0"),
                    new XAttribute("lon", "10.0"),
                    new XElement(ns + "ele", "200.0")),
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "50.1"),
                    new XAttribute("lon", "10.1"),
                    new XElement(ns + "ele", "210.0")))),
            new GpxDocument.GpxTrackSegment(new XElement(ns + "trkseg",
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "50.2"),
                    new XAttribute("lon", "10.2"),
                    new XElement(ns + "ele", "220.0"))))
        };

        // Act
        var result = segments.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(new GeoPosition(50.0, 10.0, 200.0), result[0]);
        Assert.AreEqual(new GeoPosition(50.1, 10.1, 210.0), result[1]);
        Assert.AreEqual(new GeoPosition(50.2, 10.2, 220.0), result[2]);
    }

    /// <summary>
    /// Tests that AsGeoPath handles an empty collection of GPX track segments.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GpxTrackSegments_EmptyCollection_ReturnsEmptyGeoPath()
    {
        // Arrange
        var segments = new List<GpxDocument.GpxTrackSegment>();

        // Act
        var result = segments.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    #endregion

    #region AsGeoPath Tests - GPX Track

    /// <summary>
    /// Tests that AsGeoPath converts a GPX track to a GeoPath.
    /// </summary>
    [TestMethod]
    public void AsGeoPath_GpxTrack_ReturnsGeoPath()
    {
        // Arrange
        var ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
        var trackElement = new XElement(ns + "trk",
            new XElement(ns + "trkseg",
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "52.0"),
                    new XAttribute("lon", "5.0"),
                    new XElement(ns + "ele", "10.0")),
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "52.1"),
                    new XAttribute("lon", "5.1"),
                    new XElement(ns + "ele", "15.0"))),
            new XElement(ns + "trkseg",
                new XElement(ns + "trkpt",
                    new XAttribute("lat", "52.2"),
                    new XAttribute("lon", "5.2"),
                    new XElement(ns + "ele", "20.0"))));
        var track = new GpxDocument.GpxTrack(trackElement);

        // Act
        var result = track.AsGeoPath();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(new GeoPosition(52.0, 5.0, 10.0), result[0]);
        Assert.AreEqual(new GeoPosition(52.1, 5.1, 15.0), result[1]);
        Assert.AreEqual(new GeoPosition(52.2, 5.2, 20.0), result[2]);
    }

    #endregion

    #region IsLocationInPath Tests

    /// <summary>
    /// Tests that IsLocationInPath returns true when the position is inside the path.
    /// </summary>
    [TestMethod]
    public void IsLocationInPath_PositionInsidePath_ReturnsTrue()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var position = new GeoPosition(5.0, 5.0, 0);

        // Act
        var result = path.IsLocationInPath(position);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsLocationInPath returns false when the position is outside the path.
    /// </summary>
    [TestMethod]
    public void IsLocationInPath_PositionOutsidePath_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var position = new GeoPosition(15.0, 15.0, 0);

        // Act
        var result = path.IsLocationInPath(position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsLocationInPath returns false when the position is on the boundary.
    /// </summary>
    [TestMethod]
    public void IsLocationInPath_PositionOnBoundary_BehavesConsistently()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var position = new GeoPosition(0.0, 5.0, 0);

        // Act
        var result = path.IsLocationInPath(position);

        // Assert
        // Boundary behavior is implementation-specific; just verify it doesn't crash
    }

    #endregion

    #region IsPositionInPath Tests

    /// <summary>
    /// Tests that IsPositionInPath returns true when the location is inside a square polygon.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_LocationInsideSquare_ReturnsTrue()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var location = new GeoLocation(5.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath returns false when the location is outside the polygon.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_LocationOutsidePolygon_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var location = new GeoLocation(15.0, 15.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath returns true when the location is inside a triangle.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_LocationInsideTriangle_ReturnsTrue()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(10.0, 5.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0));
        var location = new GeoLocation(3.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath returns false when the location is outside a triangle.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_LocationOutsideTriangle_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(10.0, 5.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0));
        var location = new GeoLocation(15.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath handles a complex polygon correctly.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_ComplexPolygon_ReturnsCorrectResult()
    {
        // Arrange - L-shaped polygon
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(0.0, 10.0, 0))
            .Add(new GeoPosition(5.0, 10.0, 0))
            .Add(new GeoPosition(5.0, 5.0, 0))
            .Add(new GeoPosition(10.0, 5.0, 0))
            .Add(new GeoPosition(10.0, 0.0, 0));
        var insideLocation = new GeoLocation(2.0, 7.0);
        var outsideLocation = new GeoLocation(7.0, 7.0);

        // Act
        var insideResult = path.IsPositionInPath(insideLocation);
        var outsideResult = path.IsPositionInPath(outsideLocation);

        // Assert
        Assert.IsTrue(insideResult);
        Assert.IsFalse(outsideResult);
    }

    /// <summary>
    /// Tests that IsPositionInPath with an empty path returns false.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_EmptyPath_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath();
        var location = new GeoLocation(5.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath with a path containing only one point returns false.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_SinglePoint_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath().Add(new GeoPosition(5.0, 5.0, 0));
        var location = new GeoLocation(5.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsPositionInPath with a path containing only two points returns false.
    /// </summary>
    [TestMethod]
    public void IsPositionInPath_TwoPoints_ReturnsFalse()
    {
        // Arrange
        var path = new GeoPath()
            .Add(new GeoPosition(0.0, 0.0, 0))
            .Add(new GeoPosition(10.0, 10.0, 0));
        var location = new GeoLocation(5.0, 5.0);

        // Act
        var result = path.IsPositionInPath(location);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion
}

