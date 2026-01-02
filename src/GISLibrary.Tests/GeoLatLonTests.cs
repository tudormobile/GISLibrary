using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLatLonTests
{
    [TestMethod]
    public void Constructor_WithLatitudeLongitude_SetsProperties()
    {
        // arrange
        double latitude = 1.0;
        double longitude = 2.0;
        // act
        var pos = new GeoLatLon(latitude, longitude);
        // assert
        Assert.AreEqual(longitude, pos.Longitude);
        Assert.AreEqual(latitude, pos.Latitude);
    }

    [TestMethod]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // arrange
        var pos1 = new GeoLatLon(1.0, 2.0);
        var pos2 = new GeoLatLon(1.0, 2.0);
        // act
        var result = pos1.Equals(pos2);
        // assert
        Assert.IsTrue(result);
        Assert.IsTrue(pos1 == pos2);
        Assert.IsFalse(pos1 != pos2);
    }

    [TestMethod]
    public void Equals_WithDifferentLatitude_ReturnsFalse()
    {
        // arrange
        var pos1 = new GeoLatLon(1.0, 2.0);
        var pos2 = new GeoLatLon(1.5, 2.0);
        // act
        var result = pos1.Equals(pos2);
        // assert
        Assert.IsFalse(result);
        Assert.IsFalse(pos1 == pos2);
        Assert.IsTrue(pos1 != pos2);
    }

    [TestMethod]
    public void Equals_WithDifferentLongitude_ReturnsFalse()
    {
        // arrange
        var pos1 = new GeoLatLon(1.0, 2.0);
        var pos2 = new GeoLatLon(1.0, 2.5);
        // act
        var result = pos1.Equals(pos2);
        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithObject_ReturnsTrueOrFalse()
    {
        // arrange
        var pos1 = new GeoLatLon(1.0, 2.0);
        object pos2 = new GeoLatLon(1.0, 2.5);
        object pos3 = pos1;
        // act
        var result = pos1.Equals(pos2);
        // assert
        Assert.IsFalse(result);
        Assert.IsTrue(pos1.Equals(pos3));
    }


    [TestMethod]
    public void Equals_WithNull_ReturnsFalse()
    {
        // arrange
        var pos = new GeoLatLon(1.0, 2.0);
        // act
        var result = pos.Equals(null);
        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // arrange
        var pos1 = new GeoLatLon(1.0, 2.0);
        var pos2 = new GeoLatLon(1.0, 2.0);
        // act
        var hash1 = pos1.GetHashCode();
        var hash2 = pos2.GetHashCode();
        // assert
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // arrange
        var pos = new GeoLatLon(1.0, 2.0);
        // act
        var result = pos.ToString();
        // assert
        Assert.IsNotNull(result);
        Assert.Contains("1", result);
        Assert.Contains("2", result);
    }
}
