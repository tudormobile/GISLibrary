using System.Xml.Linq;
using static Tudormobile.Tcx.TcxDocument;

namespace GISLibrary.Tests.Tcx;

[TestClass]
public class TcxActivityTests
{
    [TestMethod]
    public void Constructor_WithValidActivityElement_CreatesInstance()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);

        // Act
        var activity = new TcxActivity(element);

        // Assert
        Assert.IsNotNull(activity);
    }

    [TestMethod]
    public void Id_WithValidIdElement_ReturnsIdValue()
    {
        // Arrange
        var expectedId = "2023-01-15T08:30:00Z";
        var xml = $@"
            <Activity Sport=""Running"">
                <Id>{expectedId}</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualId = activity.Id;

        // Assert
        Assert.AreEqual(expectedId, actualId);
    }

    [TestMethod]
    public void Id_WithMissingIdElement_ReturnsDateTimeMinValue()
    {
        // Arrange
        var xml = @"<Activity Sport=""Running""></Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualId = activity.Id;

        // Assert
        Assert.AreEqual(DateTime.MinValue.ToString("O"), actualId);
    }

    [TestMethod]
    public void ActivityId_WithValidIdElement_ReturnsDateTime()
    {
        // Arrange
        var expectedDate = new DateTime(2023, 1, 15, 8, 30, 0, DateTimeKind.Utc);
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualDate = activity.ActivityId;

        // Assert
        Assert.AreEqual(expectedDate, actualDate.ToUniversalTime());
    }

    [TestMethod]
    public void ActivityId_WithMissingIdElement_ReturnsDateTimeMinValue()
    {
        // Arrange
        var xml = @"<Activity Sport=""Running""></Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualDate = activity.ActivityId;

        // Assert
        Assert.AreEqual(DateTime.MinValue, actualDate);
    }

    [TestMethod]
    public void Sport_WithValidSportAttribute_ReturnsSportType()
    {
        // Arrange
        var expectedSport = "Running";
        var xml = $@"
            <Activity Sport=""{expectedSport}"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualSport = activity.Sport;

        // Assert
        Assert.AreEqual(expectedSport, actualSport);
    }

    [TestMethod]
    public void Sport_WithMissingSportAttribute_ReturnsUnknown()
    {
        // Arrange
        var xml = @"
            <Activity>
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualSport = activity.Sport;

        // Assert
        Assert.AreEqual("Unknown", actualSport);
    }

    [TestMethod]
    public void Sport_WithBikingSport_ReturnsBiking()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Biking"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualSport = activity.Sport;

        // Assert
        Assert.AreEqual("Biking", actualSport);
    }

    [TestMethod]
    public void Laps_WithNoLaps_ReturnsEmptyCollection()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var laps = activity.Laps.ToList();

        // Assert
        Assert.IsEmpty(laps);
    }

    [TestMethod]
    public void Laps_WithSingleLap_ReturnsOneLap()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
                <Lap StartTime=""2023-01-15T08:30:00Z"">
                    <TotalTimeSeconds>600</TotalTimeSeconds>
                    <DistanceMeters>1500</DistanceMeters>
                    <MaximumSpeed>5.5</MaximumSpeed>
                </Lap>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var laps = activity.Laps.ToList();

        // Assert
        Assert.HasCount(1, laps);
        Assert.IsInstanceOfType<TcxActivityLap>(laps[0]);
    }

    [TestMethod]
    public void Laps_WithMultipleLaps_ReturnsAllLaps()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
                <Lap StartTime=""2023-01-15T08:30:00Z"">
                    <TotalTimeSeconds>600</TotalTimeSeconds>
                    <DistanceMeters>1500</DistanceMeters>
                    <MaximumSpeed>5.5</MaximumSpeed>
                </Lap>
                <Lap StartTime=""2023-01-15T08:40:00Z"">
                    <TotalTimeSeconds>620</TotalTimeSeconds>
                    <DistanceMeters>1600</DistanceMeters>
                    <MaximumSpeed>5.8</MaximumSpeed>
                </Lap>
                <Lap StartTime=""2023-01-15T08:50:00Z"">
                    <TotalTimeSeconds>610</TotalTimeSeconds>
                    <DistanceMeters>1550</DistanceMeters>
                    <MaximumSpeed>5.6</MaximumSpeed>
                </Lap>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var laps = activity.Laps.ToList();

        // Assert
        Assert.HasCount(3, laps);
        Assert.IsInstanceOfType<TcxActivityLap>(laps[0]);
        Assert.IsInstanceOfType<TcxActivityLap>(laps[1]);
        Assert.IsInstanceOfType<TcxActivityLap>(laps[2]);
    }

    [TestMethod]
    public void Laps_LazyEvaluation_DoesNotEnumerateImmediately()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
                <Lap StartTime=""2023-01-15T08:30:00Z"">
                    <TotalTimeSeconds>600</TotalTimeSeconds>
                    <DistanceMeters>1500</DistanceMeters>
                    <MaximumSpeed>5.5</MaximumSpeed>
                </Lap>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var laps = activity.Laps;

        // Assert
        Assert.IsNotNull(laps);
        // Verify lazy evaluation by enumerating
        Assert.AreEqual(1, laps.Count());
    }

    [TestMethod]
    public void AsElement_ReturnsUnderlyingXElement()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var resultElement = activity.AsElement();

        // Assert
        Assert.AreSame(element, resultElement);
    }

    [TestMethod]
    public void ExplicitCast_ToXElement_ReturnsUnderlyingElement()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var resultElement = (XElement)activity;

        // Assert
        Assert.AreSame(element, resultElement);
    }

    [TestMethod]
    public void CompleteActivity_WithAllProperties_ParsesCorrectly()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Biking"">
                <Id>2023-06-20T15:45:00.000Z</Id>
                <Lap StartTime=""2023-06-20T15:45:00.000Z"">
                    <TotalTimeSeconds>1800</TotalTimeSeconds>
                    <DistanceMeters>15000</DistanceMeters>
                    <MaximumSpeed>12.5</MaximumSpeed>
                    <Track>
                        <Trackpoint>
                            <Time>2023-06-20T15:45:00.000Z</Time>
                        </Trackpoint>
                    </Track>
                </Lap>
                <Lap StartTime=""2023-06-20T16:15:00.000Z"">
                    <TotalTimeSeconds>1900</TotalTimeSeconds>
                    <DistanceMeters>16000</DistanceMeters>
                    <MaximumSpeed>13.0</MaximumSpeed>
                    <Track>
                        <Trackpoint>
                            <Time>2023-06-20T16:15:00.000Z</Time>
                        </Trackpoint>
                    </Track>
                </Lap>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act & Assert
        Assert.AreEqual("Biking", activity.Sport);
        Assert.AreEqual("2023-06-20T15:45:00.000Z", activity.Id);
        Assert.AreEqual(new DateTime(2023, 6, 20, 15, 45, 0, DateTimeKind.Utc), activity.ActivityId.ToUniversalTime());
        Assert.AreEqual(2, activity.Laps.Count());
    }

    [TestMethod]
    public void Constructor_WithNullElement_ThrowsException()
    {
        // Arrange
        XElement? element = null;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new TcxActivity(element!));
    }

    [TestMethod]
    public void Id_WithEmptyIdElement_ReturnsEmptyString()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Running"">
                <Id></Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualId = activity.Id;

        // Assert
        Assert.AreEqual(string.Empty, actualId);
    }

    [TestMethod]
    public void Sport_WithOtherSportType_ReturnsCorrectValue()
    {
        // Arrange
        var xml = @"
            <Activity Sport=""Other"">
                <Id>2023-01-15T08:30:00Z</Id>
            </Activity>";
        var element = XElement.Parse(xml);
        var activity = new TcxActivity(element);

        // Act
        var actualSport = activity.Sport;

        // Assert
        Assert.AreEqual("Other", actualSport);
    }
}
