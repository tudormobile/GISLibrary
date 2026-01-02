using System.Xml.Linq;
using static Tudormobile.Tcx.TcxDocument;

namespace GISLibrary.Tests.Tcx;

[TestClass]
public class TcxTrackpointTests
{
    [TestMethod]
    public void Constructor_WithValidElement_ShouldInitializeTrackpoint()
    {
        // Arrange
        var xml = @"
<Trackpoint>
    <Time>2023-05-15T10:30:00Z</Time>
    <Position>
        <LatitudeDegrees>37.7749</LatitudeDegrees>
        <LongitudeDegrees>-122.4194</LongitudeDegrees>
    </Position>
    <AltitudeMeters>50.0</AltitudeMeters>
    <DistanceMeters>1000.0</DistanceMeters>
    <HeartRateBpm>
        <Value>145</Value>
    </HeartRateBpm>
</Trackpoint>";
        var element = XElement.Parse(xml);

        // Act
        var trackpoint = new TcxTrackpoint(element);

        // Assert
        Assert.IsNotNull(trackpoint);
    }

    [TestMethod]
    public void Time_WithValidData_ReturnsCorrectDateTime()
    {
        // Arrange
        var xml = @"
<Trackpoint>
    <Time>2023-05-15T10:30:00Z</Time>
    <Position>
        <LatitudeDegrees>37.7749</LatitudeDegrees>
        <LongitudeDegrees>-122.4194</LongitudeDegrees>
    </Position>
    <AltitudeMeters>50.0</AltitudeMeters>
    <DistanceMeters>1000.0</DistanceMeters>
    <HeartRateBpm>
        <Value>145</Value>
    </HeartRateBpm>
</Trackpoint>";
        var element = XElement.Parse(xml);
        var trackpoint = new TcxTrackpoint(element);

        // Act
        var time = trackpoint.Time;

        // Assert
        Assert.AreEqual(new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc), time);
    }

    [TestMethod]
    public void Position_WithValidData_ReturnsCorrectLatLon()
    {
        // Arrange
        var xml = @"
<Trackpoint>
    <Time>2023-05-15T10:30:00Z</Time>
    <Position>
        <LatitudeDegrees>37.7749</LatitudeDegrees>
        <LongitudeDegrees>-122.4194</LongitudeDegrees>
    </Position>
    <AltitudeMeters>50.0</AltitudeMeters>
    <DistanceMeters>1000.0</DistanceMeters>
    <HeartRateBpm>
        <Value>145</Value>
    </HeartRateBpm>
</Trackpoint>";
        var element = XElement.Parse(xml);
        var trackpoint = new TcxTrackpoint(element);

        // Act
        var (lat, lon) = trackpoint.Position;

        // Assert
        Assert.AreEqual(37.7749, lat, 0.0001);
        Assert.AreEqual(-122.4194, lon, 0.0001);
    }

    [TestMethod]
    public void Position_WithNegativeLatitude_ReturnsCorrectValues()
    {
        // Arrange
        var xml = @"
<Trackpoint>
    <Time>2023-05-15T10:30:00Z</Time>
    <Position>
        <LatitudeDegrees>-33.8688</LatitudeDegrees>
        <LongitudeDegrees>151.2093</LongitudeDegrees>
    </Position>
    <AltitudeMeters>10.0</AltitudeMeters>
    <DistanceMeters>500.0</DistanceMeters>
    <HeartRateBpm>
        <Value>120</Value>
    </HeartRateBpm>
</Trackpoint>";
        var element = XElement.Parse(xml);
        var trackpoint = new TcxTrackpoint(element);

        // Act
        var (lat, lon) = trackpoint.Position;

        // Assert
        Assert.AreEqual(-33.8688, lat, 0.0001);
        Assert.AreEqual(151.2093, lon, 0.0001);
    }

    [TestMethod]
    public void AltitudeMeters_WithValidData_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Trackpoint>
    <Time>2023-05-15T10:30:00Z</Time>
    <Position>
        <LatitudeDegrees>37.7749</LatitudeDegrees>
        <LongitudeDegrees>-122.4194</LongitudeDegrees>
    </Position>
    <AltitudeMeters>50.5</AltitudeMeters>
    <DistanceMeters>1000.0</DistanceMeters>
    <HeartRateBpm>
        <Value>145</Value>
    </HeartRateBpm>
</Trackpoint>";
        var element = XElement.Parse(xml);
        var trackpoint = new TcxTrackpoint(element);

        // Act
        var altitude = trackpoint.AltitudeMeters;

        // Assert
        Assert.AreEqual(50.5, altitude, 0.01);
    }
}
