using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitTrackpointTests
{
    [TestMethod]
    public void Constructor_SetsProperties()
    {
        var timestamp = new DateTime(2021, 4, 4, 18, 1, 13, DateTimeKind.Utc);
        var position = new FitPosition(42.84601, -73.82947, 76.6);

        var trackpoint = new FitTrackpoint(timestamp, position);

        Assert.AreEqual(timestamp, trackpoint.Timestamp);
        Assert.AreEqual(position, trackpoint.Position);
    }

    [TestMethod]
    public void Constructor_WithNullTimestampAndPosition_LeavesBothNull()
    {
        var trackpoint = new FitTrackpoint(null, null);

        Assert.IsNull(trackpoint.Timestamp);
        Assert.IsNull(trackpoint.Position);
    }
}
