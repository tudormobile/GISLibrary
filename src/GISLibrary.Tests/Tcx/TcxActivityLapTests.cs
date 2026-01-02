using System.Xml.Linq;
using Tudormobile.Tcx;

namespace GISLibrary.Tests.Tcx;

[TestClass]
public class TcxActivityLapTests
{
    [TestMethod]
    public void Constructor_WithValidElement_ShouldInitializeLap()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);

        // Act
        var lap = new TcxDocument.TcxActivityLap(element);

        // Assert
        Assert.IsNotNull(lap);
    }

    [TestMethod]
    public void StartTime_WithValidData_ReturnsCorrectDateTime()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var startTime = lap.StartTime;

        // Assert
        Assert.AreEqual(new DateTime(2023, 5, 15, 10, 30, 0, DateTimeKind.Utc), startTime);
    }

    [TestMethod]
    public void TotalTimeSeconds_WithValidData_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.5</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var totalTime = lap.TotalTimeSeconds;

        // Assert
        Assert.AreEqual(1800.5, totalTime, 0.01);
    }

    [TestMethod]
    public void TotalTimeSeconds_WithZeroValue_ReturnsZero()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>0.0</TotalTimeSeconds>
    <DistanceMeters>0.0</DistanceMeters>
    <MaximumSpeed>0.0</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>0.0</LatitudeDegrees>
                <LongitudeDegrees>0.0</LongitudeDegrees>
            </Position>
            <AltitudeMeters>0.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>100</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var totalTime = lap.TotalTimeSeconds;

        // Assert
        Assert.AreEqual(0.0, totalTime, 0.01);
    }

    [TestMethod]
    public void TotalTimeSeconds_WithLongDuration_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>7200.0</TotalTimeSeconds>
    <DistanceMeters>20000.0</DistanceMeters>
    <MaximumSpeed>8.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var totalTime = lap.TotalTimeSeconds;

        // Assert
        Assert.AreEqual(7200.0, totalTime, 0.01);
    }

    [TestMethod]
    public void DistanceMeters_WithValidData_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5280.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var distance = lap.DistanceMeters;

        // Assert
        Assert.AreEqual(5280.0, distance, 0.01);
    }

    [TestMethod]
    public void DistanceMeters_WithLargeValue_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>7200.0</TotalTimeSeconds>
    <DistanceMeters>42195.0</DistanceMeters>
    <MaximumSpeed>10.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var distance = lap.DistanceMeters;

        // Assert
        Assert.AreEqual(42195.0, distance, 0.01);
    }

    [TestMethod]
    public void DistanceMeters_WithZeroValue_ReturnsZero()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>0.0</TotalTimeSeconds>
    <DistanceMeters>0.0</DistanceMeters>
    <MaximumSpeed>0.0</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var distance = lap.DistanceMeters;

        // Assert
        Assert.AreEqual(0.0, distance, 0.01);
    }

    [TestMethod]
    public void MaximumSpeed_WithValidData_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>8.33</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var maxSpeed = lap.MaximumSpeed;

        // Assert
        Assert.AreEqual(8.33, maxSpeed, 0.01);
    }

    [TestMethod]
    public void MaximumSpeed_WithHighValue_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>600.0</TotalTimeSeconds>
    <DistanceMeters>2000.0</DistanceMeters>
    <MaximumSpeed>15.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var maxSpeed = lap.MaximumSpeed;

        // Assert
        Assert.AreEqual(15.5, maxSpeed, 0.01);
    }

    [TestMethod]
    public void MaximumSpeed_WithZeroValue_ReturnsZero()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>0.0</DistanceMeters>
    <MaximumSpeed>0.0</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var maxSpeed = lap.MaximumSpeed;

        // Assert
        Assert.AreEqual(0.0, maxSpeed, 0.01);
    }

    [TestMethod]
    public void Tracks_WithNoTrackpoints_ReturnsEmptyCollection()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks = lap.Tracks.ToList();

        // Assert
        Assert.IsEmpty(tracks);
    }

    [TestMethod]
    public void Tracks_WithSingleTrackpoint_ReturnsOneTrackpoint()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(1, tracks);
        Assert.IsNotNull(tracks[0]);
    }

    [TestMethod]
    public void Tracks_WithMultipleTrackpoints_ReturnsAllTrackpoints()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
        <Trackpoint>
            <Time>2023-05-15T10:31:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7750</LatitudeDegrees>
                <LongitudeDegrees>-122.4195</LongitudeDegrees>
            </Position>
            <AltitudeMeters>51.0</AltitudeMeters>
            <DistanceMeters>100.0</DistanceMeters>
            <HeartRateBpm>
                <Value>125</Value>
            </HeartRateBpm>
        </Trackpoint>
        <Trackpoint>
            <Time>2023-05-15T10:32:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7751</LatitudeDegrees>
                <LongitudeDegrees>-122.4196</LongitudeDegrees>
            </Position>
            <AltitudeMeters>52.0</AltitudeMeters>
            <DistanceMeters>200.0</DistanceMeters>
            <HeartRateBpm>
                <Value>130</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(3, tracks);
        Assert.IsNotNull(tracks[0]);
        Assert.IsNotNull(tracks[1]);
        Assert.IsNotNull(tracks[2]);
    }

    [TestMethod]
    public void Tracks_WithMultipleTracks_ReturnsAllTrackpoints()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:31:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7750</LatitudeDegrees>
                <LongitudeDegrees>-122.4195</LongitudeDegrees>
            </Position>
            <AltitudeMeters>51.0</AltitudeMeters>
            <DistanceMeters>100.0</DistanceMeters>
            <HeartRateBpm>
                <Value>125</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(2, tracks);
    }

    [TestMethod]
    public void AllProperties_WithCompleteData_ReturnCorrectValues()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T14:22:30Z"">
    <TotalTimeSeconds>3600.0</TotalTimeSeconds>
    <DistanceMeters>10000.0</DistanceMeters>
    <MaximumSpeed>12.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T14:22:30Z</Time>
            <Position>
                <LatitudeDegrees>51.5074</LatitudeDegrees>
                <LongitudeDegrees>-0.1278</LongitudeDegrees>
            </Position>
            <AltitudeMeters>11.5</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>140</Value>
            </HeartRateBpm>
        </Trackpoint>
        <Trackpoint>
            <Time>2023-05-15T15:22:30Z</Time>
            <Position>
                <LatitudeDegrees>51.5075</LatitudeDegrees>
                <LongitudeDegrees>-0.1279</LongitudeDegrees>
            </Position>
            <AltitudeMeters>12.0</AltitudeMeters>
            <DistanceMeters>10000.0</DistanceMeters>
            <HeartRateBpm>
                <Value>155</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act & Assert
        Assert.AreEqual(new DateTime(2023, 5, 15, 14, 22, 30, DateTimeKind.Utc), lap.StartTime);
        Assert.AreEqual(3600.0, lap.TotalTimeSeconds, 0.01);
        Assert.AreEqual(10000.0, lap.DistanceMeters, 0.01);
        Assert.AreEqual(12.5, lap.MaximumSpeed, 0.01);

        var tracks = lap.Tracks.ToList();
        Assert.HasCount(2, tracks);
        Assert.AreEqual(new DateTime(2023, 5, 15, 14, 22, 30, DateTimeKind.Utc), tracks[0].Time);
        Assert.AreEqual(new DateTime(2023, 5, 15, 15, 22, 30, DateTimeKind.Utc), tracks[1].Time);
    }

    [TestMethod]
    public void StartTime_WithLocalTime_ReturnsCorrectDateTime()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00-07:00"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track />
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var startTime = lap.StartTime;

        // Assert
        Assert.AreEqual(new DateTime(2023, 5, 15, 17, 30, 0, DateTimeKind.Utc), startTime.ToUniversalTime());
    }

    [TestMethod]
    public void Tracks_EnumerationIsLazy_DoesNotEvaluateImmediately()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks = lap.Tracks;

        // Assert - just getting the enumerable shouldn't evaluate it
        Assert.IsNotNull(tracks);
        Assert.IsInstanceOfType<IEnumerable<TcxDocument.TcxTrackpoint>>(tracks);
    }

    [TestMethod]
    public void Tracks_MultipleEnumerations_ReturnSameTrackpoints()
    {
        // Arrange
        var xml = @"
<Lap StartTime=""2023-05-15T10:30:00Z"">
    <TotalTimeSeconds>1800.0</TotalTimeSeconds>
    <DistanceMeters>5000.0</DistanceMeters>
    <MaximumSpeed>5.5</MaximumSpeed>
    <Track>
        <Trackpoint>
            <Time>2023-05-15T10:30:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7749</LatitudeDegrees>
                <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
                <Value>120</Value>
            </HeartRateBpm>
        </Trackpoint>
        <Trackpoint>
            <Time>2023-05-15T10:31:00Z</Time>
            <Position>
                <LatitudeDegrees>37.7750</LatitudeDegrees>
                <LongitudeDegrees>-122.4195</LongitudeDegrees>
            </Position>
            <AltitudeMeters>51.0</AltitudeMeters>
            <DistanceMeters>100.0</DistanceMeters>
            <HeartRateBpm>
                <Value>125</Value>
            </HeartRateBpm>
        </Trackpoint>
    </Track>
</Lap>";
        var element = XElement.Parse(xml);
        var lap = new TcxDocument.TcxActivityLap(element);

        // Act
        var tracks1 = lap.Tracks.ToList();
        var tracks2 = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(2, tracks1);
        Assert.HasCount(2, tracks2);
        Assert.AreEqual(tracks1[0].Time, tracks2[0].Time);
        Assert.AreEqual(tracks1[1].Time, tracks2[1].Time);
    }
}
