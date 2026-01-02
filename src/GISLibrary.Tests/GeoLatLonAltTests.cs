using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLatLonAltTests
{
    [TestMethod]
    public void Constructor_WithLatitudeLongitudeAltitude_SetsProperties()
    {
        // arrange
        double latitude = 1.0;
        double longitude = 2.0;
        double altitude = 3.0;

        // act
        var pos = new GeoLatLonAlt(latitude, longitude, altitude);

        // assert
        Assert.AreEqual(longitude, pos.Longitude);
        Assert.AreEqual(latitude, pos.Latitude);
        Assert.AreEqual(altitude, pos.Altitude);
    }

    [TestMethod]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.0, 2.0, 3.0);

        // act
        bool result = pos1.Equals(pos2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_WithDifferentLatitude_ReturnsFalse()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.1, 2.0, 3.0);

        // act
        bool result = pos1.Equals(pos2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithDifferentLongitude_ReturnsFalse()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.0, 2.1, 3.0);

        // act
        bool result = pos1.Equals(pos2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithDifferentAltitude_ReturnsFalse()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.0, 2.0, 3.1);

        // act
        bool result = pos1.Equals(pos2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithNull_ReturnsFalse()
    {
        // arrange
        var pos = new GeoLatLonAlt(1.0, 2.0, 3.0);
        object pos2 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        object pos3 = new GeoLatLonAlt(2.0, 3.0, 4.0);
        object pos4 = pos;
        object pos5 = null!;

        // act & assert
        Assert.IsTrue(pos.Equals(pos4));
        Assert.IsFalse(pos.Equals(pos5));
        Assert.IsTrue(pos.Equals(pos2));
        Assert.IsFalse(pos.Equals(pos3));
    }

    [TestMethod]
    public void Equals_WithObject_ReturnsTrueOrFalse()
    {
        // arrange
        var pos = new GeoLatLonAlt(1.0, 2.0, 3.0);

        // act
        bool result = pos.Equals(null);

        // assert
        Assert.IsFalse(result);
    }


    [TestMethod]
    public void EqualsOperator_WithSameValues_ReturnsTrue()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.0, 2.0, 3.0);

        // act
        bool result = pos1 == pos2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void NotEqualsOperator_WithDifferentValues_ReturnsTrue()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.1, 2.0, 3.0);

        // act
        bool result = pos1 != pos2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // arrange
        var pos1 = new GeoLatLonAlt(1.0, 2.0, 3.0);
        var pos2 = new GeoLatLonAlt(1.0, 2.0, 3.0);

        // act
        int hash1 = pos1.GetHashCode();
        int hash2 = pos2.GetHashCode();

        // assert
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // arrange
        var pos = new GeoLatLonAlt(1.0, 2.0, 3.0);

        // act
        string result = pos.ToString();

        // assert
        Assert.IsFalse(string.IsNullOrEmpty(result));
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
    }
}
