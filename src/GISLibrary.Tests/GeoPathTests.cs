using System.Collections;
using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoPathTests
{
    [TestMethod]
    public void Constructor_CreatesEmptyPath()
    {
        var path = new GeoPath();

        Assert.IsTrue(path.IsEmpty);
        Assert.IsFalse(path.HasValues);
        Assert.HasCount(0, path);
    }

    [TestMethod]
    public void AddRange_AddsPositionsToPath()
    {
        var geoPath = new GeoPath();
        var positions = new List<GeoPosition>
        {
            new GeoPosition(10.0, 20.0, 100.0),
            new GeoPosition(30.0, 40.0, 200.0),
            new GeoPosition(50.0, 60.0, 300.0)
        };
        geoPath.AddRange(positions);
        Assert.IsFalse(geoPath.IsEmpty);
        Assert.IsTrue(geoPath.HasValues);
        Assert.AreEqual(3, geoPath.Count);
        Assert.HasCount(3, geoPath);
        CollectionAssert.AreEqual(positions, geoPath.ToList());
    }

    [TestMethod]
    public void AddRange_AddLocationRange_AddsPositionsToPath()
    {
        var geoPath = new GeoPath();
        var locations = new List<GeoLocation>
        {
            new GeoLocation(10.0, 20.0),
            new GeoLocation(30.0, 40.0),
            new GeoLocation(50.0, 60.0)
        };
        geoPath.AddRange(locations);
        Assert.IsFalse(geoPath.IsEmpty);
        Assert.IsTrue(geoPath.HasValues);
        Assert.AreEqual(3, geoPath.Count);
        Assert.HasCount(3, geoPath);

        var expectedPositions = locations.Select(loc => new GeoPosition(loc.Latitude, loc.Longitude, 0)).ToList();
        CollectionAssert.AreEqual(expectedPositions, geoPath.ToList());
    }

    [TestMethod]
    public void AddPosition_AddsPositionToPath()
    {
        var geoPath = new GeoPath();
        geoPath.Add(new GeoPosition(10.0, 20.0, 100.0));
        Assert.IsFalse(geoPath.IsEmpty);
        Assert.IsTrue(geoPath.HasValues);
        Assert.AreEqual(1, geoPath.Count);
        Assert.HasCount(1, geoPath);
        Assert.AreEqual(new GeoPosition(10.0, 20.0, 100.0), geoPath[0]);
    }

    [TestMethod]
    public void AddLocation_AddsPositionToPath()
    {
        var geoPath = new GeoPath();
        geoPath.Add(new GeoLocation(10.0, 20.0));
        Assert.IsFalse(geoPath.IsEmpty);
        Assert.IsTrue(geoPath.HasValues);
        Assert.AreEqual(1, geoPath.Count);
        Assert.HasCount(1, geoPath);
        Assert.AreEqual(new GeoPosition(10.0, 20.0, 0.0), geoPath[0]);
    }

    [TestMethod]
    public void Clear_RemovesAllPositionsFromPath()
    {
        var geoPath = new GeoPath()
            .Add(new GeoPosition(10.0, 20.0, 100.0))
            .Add(new GeoPosition(20.0, 30.0, 150.0))
            .Add(new GeoPosition(30.0, 40.0, 200.0))
            .Clear();

        Assert.IsTrue(geoPath.IsEmpty);
        Assert.IsFalse(geoPath.HasValues);
        Assert.AreEqual(0, geoPath.Count);
        Assert.HasCount(0, geoPath);
    }

    [TestMethod]
    public void Add_WithChaining_AddsPositions()
    {
        var position = new GeoPosition(10.0, 20.0, 100.0);
        var location = new GeoLocation(30.0, 40.0);
        var geoPath = new GeoPath()
            .Add(position)
            .Add(location)
            .AddRange([position])
            .AddRange([location]);

        Assert.IsTrue(geoPath.HasValues);
        Assert.IsFalse(geoPath.IsEmpty);
        Assert.AreEqual(4, geoPath.Count);
        Assert.HasCount(4, geoPath);
    }

    [TestMethod]
    public void Enumerate_AsIEnumerable_Enumerates()
    {
        var position = new GeoPosition(10.0, 20.0, 100.0);
        IEnumerable geoPath = new GeoPath().Add(position);
        var enumerator = geoPath.GetEnumerator();
        Assert.IsNotNull(enumerator);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual(position, enumerator.Current);
        Assert.IsFalse(enumerator.MoveNext());
    }


}

