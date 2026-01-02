using System.Xml.Linq;
using Tudormobile.Tcx;

namespace GISLibrary.Tests.Tcx;

[TestClass]
public class TcxDocumentTests
{
    [TestMethod]
    public void Constructor_WithValidTcxDocument_ShouldInitialize()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);

        // Act
        var tcxDoc = new TcxDocument(xdoc);

        // Assert
        Assert.IsNotNull(tcxDoc);
    }

    [TestMethod]
    public void Constructor_WithNullDocument_ShouldThrowArgumentNullException()
    {
        // Arrange
        XDocument nullDoc = null!;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new TcxDocument(nullDoc));
    }

    [TestMethod]
    public void Constructor_WithInvalidRootElement_ShouldThrowArgumentException()
    {
        // Arrange
        var xdoc = XDocument.Parse("<InvalidRoot></InvalidRoot>");

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => new TcxDocument(xdoc));
    }

    [TestMethod]
    public void Constructor_WithNoRootElement_ShouldThrowArgumentException()
    {
        // Arrange
        var xdoc = new XDocument();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => new TcxDocument(xdoc));
    }

    [TestMethod]
    public void Activities_WithValidDocument_ShouldReturnActivities()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activities = tcxDoc.Activities.ToList();

        // Assert
        Assert.IsNotNull(activities);
        Assert.HasCount(1, activities);
    }

    [TestMethod]
    public void Activities_WithMultipleActivities_ShouldReturnAllActivities()
    {
        // Arrange
        var xdoc = XDocument.Parse(MultipleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activities = tcxDoc.Activities.ToList();

        // Assert
        Assert.HasCount(2, activities);
    }

    [TestMethod]
    public void Activities_WithNoActivities_ShouldReturnEmptyCollection()
    {
        // Arrange
        var xdoc = XDocument.Parse(EmptyTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activities = tcxDoc.Activities.ToList();

        // Assert
        Assert.HasCount(0, activities);
    }

    [TestMethod]
    public void TcxActivity_Id_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activity = tcxDoc.Activities.First();

        // Assert
        Assert.AreEqual("2024-12-20T10:30:00Z", activity.Id);
    }

    [TestMethod]
    public void TcxActivity_ActivityId_ShouldReturnParsedDateTime()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activity = tcxDoc.Activities.First();

        // Assert
        Assert.AreEqual(DateTime.Parse("2024-12-20T10:30:00Z").ToUniversalTime(), activity.ActivityId.ToUniversalTime());
    }

    [TestMethod]
    public void TcxActivity_Sport_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activity = tcxDoc.Activities.First();

        // Assert
        Assert.AreEqual("Running", activity.Sport);
    }

    [TestMethod]
    public void TcxActivity_Sport_WithMissingSportAttribute_ShouldReturnUnknown()
    {
        // Arrange
        var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
    <Activity>
      <Id>2024-12-20T10:30:00Z</Id>
    </Activity>
  </Activities>
</TrainingCenterDatabase>";
        var xdoc = XDocument.Parse(xml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activity = tcxDoc.Activities.First();

        // Assert
        Assert.AreEqual("Unknown", activity.Sport);
    }

    [TestMethod]
    public void TcxActivity_Laps_ShouldReturnCorrectCount()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();

        // Act
        var laps = activity.Laps.ToList();

        // Assert
        Assert.HasCount(1, laps);
    }

    [TestMethod]
    public void TcxActivityLap_StartTime_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var startTime = lap.StartTime;

        // Assert
        Assert.AreEqual(DateTime.Parse("2024-12-20T10:30:00Z").ToUniversalTime(), startTime.ToUniversalTime());
    }

    [TestMethod]
    public void TcxActivityLap_TotalTimeSeconds_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var totalTime = lap.TotalTimeSeconds;

        // Assert
        Assert.AreEqual(600.5, totalTime, 0.01);
    }

    [TestMethod]
    public void TcxActivityLap_DistanceMeters_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var distance = lap.DistanceMeters;

        // Assert
        Assert.AreEqual(1500.0, distance, 0.01);
    }

    [TestMethod]
    public void TcxActivityLap_MaximumSpeed_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var maxSpeed = lap.MaximumSpeed;

        // Assert
        Assert.AreEqual(3.5, maxSpeed, 0.01);
    }

    [TestMethod]
    public void TcxActivityLap_Tracks_ShouldReturnCorrectCount()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var trackpoints = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(2, trackpoints);
    }

    [TestMethod]
    public void TcxTrackpoint_Time_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var time = trackpoint.Time;

        // Assert
        Assert.AreEqual(DateTime.Parse("2024-12-20T10:30:00Z").ToUniversalTime(), time.ToUniversalTime());
    }

    [TestMethod]
    public void TcxTrackpoint_Position_ShouldReturnCorrectValues()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var (lat, lon) = trackpoint.Position;

        // Assert
        Assert.AreEqual(37.8331119, lat, 0.0000001);
        Assert.AreEqual(-122.4834356, lon, 0.0000001);
    }

    [TestMethod]
    public void TcxTrackpoint_AltitudeMeters_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var altitude = trackpoint.AltitudeMeters;

        // Assert
        Assert.AreEqual(100.5, altitude, 0.01);
    }

    [TestMethod]
    public void TcxTrackpoint_DistanceMeters_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var distance = trackpoint.DistanceMeters;

        // Assert
        Assert.AreEqual(0.0, distance, 0.01);
    }

    [TestMethod]
    public void TcxTrackpoint_HeartRateBpm_ShouldReturnCorrectValue()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var heartRate = trackpoint.HeartRateBpm;

        // Assert
        Assert.AreEqual(150.0, heartRate, 0.01);
    }

    [TestMethod]
    public void Save_WithFileName_ShouldSaveToFile()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            tcxDoc.Save(tempFile);

            // Assert
            Assert.IsTrue(File.Exists(tempFile));
            var savedContent = File.ReadAllText(tempFile);
            Assert.Contains("TrainingCenterDatabase", savedContent);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [TestMethod]
    public void Save_WithStream_ShouldWriteToStream()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        using var stream = new MemoryStream();

        // Act
        tcxDoc.Save(stream);

        // Assert
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        Assert.Contains("TrainingCenterDatabase", content);
    }

    [TestMethod]
    public async Task SaveAsync_WithStream_ShouldWriteToStream()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        using var stream = new MemoryStream();

        // Act
        await tcxDoc.SaveAsync(stream, cancellationToken: TestContext.CancellationToken);

        // Assert
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync(TestContext.CancellationToken);
        Assert.Contains("TrainingCenterDatabase", content);
    }

    [TestMethod]
    public void TcxActivity_Id_WithMissingId_ShouldReturnMinValue()
    {
        // Arrange
        var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
    <Activity Sport=""Running"">
    </Activity>
  </Activities>
</TrainingCenterDatabase>";
        var xdoc = XDocument.Parse(xml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();

        // Act
        var id = activity.Id;

        // Assert
        Assert.Contains(DateTime.MinValue.Year.ToString(), id);
    }

    [TestMethod]
    public void TcxTrackpoint_AsElement_ShouldReturnUnderlyingXElement()
    {
        // Arrange
        var xdoc = XDocument.Parse(SimpleTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();
        var trackpoint = lap.Tracks.First();

        // Act
        var element = trackpoint.AsElement();

        // Assert
        Assert.IsNotNull(element);
        Assert.AreEqual("Trackpoint", element.Name.LocalName);
    }

    [TestMethod]
    public void TcxActivity_WithBikingSport_ShouldReturnBiking()
    {
        // Arrange
        var xml = SimpleTcxXml.Replace("Sport=\"Running\"", "Sport=\"Biking\"");
        var xdoc = XDocument.Parse(xml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();

        // Act
        var sport = activity.Sport;

        // Assert
        Assert.AreEqual("Biking", sport);
    }

    [TestMethod]
    public void TcxActivityLap_WithMultipleTracksAndTrackpoints_ShouldReturnAllTrackpoints()
    {
        // Arrange
        var xdoc = XDocument.Parse(ComplexTcxXml);
        var tcxDoc = new TcxDocument(xdoc);
        var activity = tcxDoc.Activities.First();
        var lap = activity.Laps.First();

        // Act
        var trackpoints = lap.Tracks.ToList();

        // Assert
        Assert.HasCount(4, trackpoints);
    }

    [TestMethod]
    public void TcxDocument_WithComplexDocument_ShouldHandleMultipleActivitiesAndLaps()
    {
        // Arrange
        var xdoc = XDocument.Parse(ComplexTcxXml);
        var tcxDoc = new TcxDocument(xdoc);

        // Act
        var activities = tcxDoc.Activities.ToList();

        // Assert
        Assert.HasCount(1, activities);
        var activity = activities[0];
        Assert.AreEqual("Running", activity.Sport);

        var laps = activity.Laps.ToList();
        Assert.HasCount(2, laps);

        var lap1 = laps[0];
        Assert.AreEqual(600.5, lap1.TotalTimeSeconds, 0.01);
        Assert.HasCount(4, lap1.Tracks.ToList());

        var lap2 = laps[1];
        Assert.AreEqual(550.0, lap2.TotalTimeSeconds, 0.01);
        Assert.HasCount(2, lap2.Tracks.ToList());
    }

    public TestContext TestContext { get; set; }

    private static readonly string SimpleTcxXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
    <Activity Sport=""Running"">
      <Id>2024-12-20T10:30:00Z</Id>
      <Lap StartTime=""2024-12-20T10:30:00Z"">
        <TotalTimeSeconds>600.5</TotalTimeSeconds>
        <DistanceMeters>1500.0</DistanceMeters>
        <MaximumSpeed>3.5</MaximumSpeed>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T10:30:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8331119</LatitudeDegrees>
              <LongitudeDegrees>-122.4834356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>100.5</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
              <Value>150</Value>
            </HeartRateBpm>
          </Trackpoint>
          <Trackpoint>
            <Time>2024-12-20T10:30:30Z</Time>
            <Position>
              <LatitudeDegrees>37.8341119</LatitudeDegrees>
              <LongitudeDegrees>-122.4844356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>105.0</AltitudeMeters>
            <DistanceMeters>50.0</DistanceMeters>
            <HeartRateBpm>
              <Value>155</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
      </Lap>
    </Activity>
  </Activities>
</TrainingCenterDatabase>";

    private static readonly string EmptyTcxXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
  </Activities>
</TrainingCenterDatabase>";

    private static readonly string MultipleTcxXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
    <Activity Sport=""Running"">
      <Id>2024-12-20T10:30:00Z</Id>
      <Lap StartTime=""2024-12-20T10:30:00Z"">
        <TotalTimeSeconds>600.5</TotalTimeSeconds>
        <DistanceMeters>1500.0</DistanceMeters>
        <MaximumSpeed>3.5</MaximumSpeed>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T10:30:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8331119</LatitudeDegrees>
              <LongitudeDegrees>-122.4834356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>100.5</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
              <Value>150</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
      </Lap>
    </Activity>
    <Activity Sport=""Biking"">
      <Id>2024-12-20T14:00:00Z</Id>
      <Lap StartTime=""2024-12-20T14:00:00Z"">
        <TotalTimeSeconds>1200.0</TotalTimeSeconds>
        <DistanceMeters>5000.0</DistanceMeters>
        <MaximumSpeed>8.0</MaximumSpeed>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T14:00:00Z</Time>
            <Position>
              <LatitudeDegrees>37.7749</LatitudeDegrees>
              <LongitudeDegrees>-122.4194</LongitudeDegrees>
            </Position>
            <AltitudeMeters>50.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
              <Value>140</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
      </Lap>
    </Activity>
  </Activities>
</TrainingCenterDatabase>";

    private static readonly string ComplexTcxXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TrainingCenterDatabase xmlns=""http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2"">
  <Activities>
    <Activity Sport=""Running"">
      <Id>2024-12-20T10:30:00Z</Id>
      <Lap StartTime=""2024-12-20T10:30:00Z"">
        <TotalTimeSeconds>600.5</TotalTimeSeconds>
        <DistanceMeters>1500.0</DistanceMeters>
        <MaximumSpeed>3.5</MaximumSpeed>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T10:30:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8331119</LatitudeDegrees>
              <LongitudeDegrees>-122.4834356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>100.5</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
              <Value>150</Value>
            </HeartRateBpm>
          </Trackpoint>
          <Trackpoint>
            <Time>2024-12-20T10:32:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8341119</LatitudeDegrees>
              <LongitudeDegrees>-122.4844356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>105.0</AltitudeMeters>
            <DistanceMeters>250.0</DistanceMeters>
            <HeartRateBpm>
              <Value>155</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T10:34:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8351119</LatitudeDegrees>
              <LongitudeDegrees>-122.4854356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>110.0</AltitudeMeters>
            <DistanceMeters>500.0</DistanceMeters>
            <HeartRateBpm>
              <Value>160</Value>
            </HeartRateBpm>
          </Trackpoint>
          <Trackpoint>
            <Time>2024-12-20T10:36:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8361119</LatitudeDegrees>
              <LongitudeDegrees>-122.4864356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>115.0</AltitudeMeters>
            <DistanceMeters>750.0</DistanceMeters>
            <HeartRateBpm>
              <Value>165</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
      </Lap>
      <Lap StartTime=""2024-12-20T10:40:00Z"">
        <TotalTimeSeconds>550.0</TotalTimeSeconds>
        <DistanceMeters>1400.0</DistanceMeters>
        <MaximumSpeed>3.3</MaximumSpeed>
        <Track>
          <Trackpoint>
            <Time>2024-12-20T10:40:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8371119</LatitudeDegrees>
              <LongitudeDegrees>-122.4874356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>120.0</AltitudeMeters>
            <DistanceMeters>0.0</DistanceMeters>
            <HeartRateBpm>
              <Value>145</Value>
            </HeartRateBpm>
          </Trackpoint>
          <Trackpoint>
            <Time>2024-12-20T10:49:00Z</Time>
            <Position>
              <LatitudeDegrees>37.8381119</LatitudeDegrees>
              <LongitudeDegrees>-122.4884356</LongitudeDegrees>
            </Position>
            <AltitudeMeters>125.0</AltitudeMeters>
            <DistanceMeters>1400.0</DistanceMeters>
            <HeartRateBpm>
              <Value>150</Value>
            </HeartRateBpm>
          </Trackpoint>
        </Track>
      </Lap>
    </Activity>
  </Activities>
</TrainingCenterDatabase>";
}
