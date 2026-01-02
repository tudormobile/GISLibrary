using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLineTests
{
    [TestMethod]
    public void Constructor_WithPoints_SetsEndpoints()
    {
        // arrange
        var p1 = new GeoPoint(1, 2);
        var p2 = new GeoPoint(3, 4);

        // act
        var line = new GeoLine(p1, p2);

        // assert
        Assert.AreEqual(p1.X, line.StartPoint.X);
        Assert.AreEqual(p1.Y, line.StartPoint.Y);
        Assert.AreEqual(p2.X, line.EndPoint.X);
        Assert.AreEqual(p2.Y, line.EndPoint.Y);
    }

    [TestMethod]
    public void Constructor_WithDoubles_SetsEndpoints()
    {
        // arrange
        double x1 = 1, y1 = 2, x2 = 3, y2 = 4;

        // act
        var line = new GeoLine(x1, y1, x2, y2);

        // assert
        Assert.AreEqual(x1, line.StartPoint.X);
        Assert.AreEqual(y1, line.StartPoint.Y);
        Assert.AreEqual(x2, line.EndPoint.X);
        Assert.AreEqual(y2, line.EndPoint.Y);
    }

    [TestMethod]
    public void Implicit_From4Tuple_CreatesLine()
    {
        // arrange
        (double, double, double, double) t = (1.0, 2.0, 3.0, 4.0);

        // act
        GeoLine line = t;

        // assert
        Assert.AreEqual(1.0, line.StartPoint.X);
        Assert.AreEqual(2.0, line.StartPoint.Y);
        Assert.AreEqual(3.0, line.EndPoint.X);
        Assert.AreEqual(4.0, line.EndPoint.Y);
    }

    [TestMethod]
    public void Implicit_FromNestedTuple_CreatesLine()
    {
        // arrange
        var t = ((1.0, 2.0), (3.0, 4.0));

        // act
        GeoLine line = t;

        // assert
        Assert.AreEqual(1.0, line.StartPoint.X);
        Assert.AreEqual(2.0, line.StartPoint.Y);
        Assert.AreEqual(3.0, line.EndPoint.X);
        Assert.AreEqual(4.0, line.EndPoint.Y);
    }

    [TestMethod]
    public void Equals_WithEqualLines_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine(1, 2, 3, 4);
        var line2 = new GeoLine(1, 2, 3, 4);
        object line3 = new GeoLine(1, 2, 3, 4);
        object notALine = "Not a line";

        // act
        var result = line1.Equals(line2);

        // assert
        Assert.IsTrue(result);
        Assert.IsTrue(line1.Equals(line3));
        Assert.IsFalse(line1.Equals(notALine));
    }

    [TestMethod]
    public void Equals_WithDifferentLines_ReturnsFalse()
    {
        // arrange
        var line1 = new GeoLine(1, 2, 3, 4);
        var line2 = new GeoLine(5, 6, 7, 8);

        // act
        var result = line1.Equals(line2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithNull_ReturnsFalse()
    {
        // arrange
        var line = new GeoLine(1, 2, 3, 4);

        // act
        var result = line.Equals(null);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsOperator_WithEqualLines_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine(1, 2, 3, 4);
        var line2 = new GeoLine(1, 2, 3, 4);

        // act
        var result = line1 == line2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void NotEqualsOperator_WithDifferentLines_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine(1, 2, 3, 4);
        var line2 = new GeoLine(5, 6, 7, 8);

        // act
        var result = line1 != line2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetHashCode_WithEqualLines_ReturnsSameHash()
    {
        // arrange
        var line1 = new GeoLine(1, 2, 3, 4);
        var line2 = new GeoLine(1, 2, 3, 4);

        // act
        var hash1 = line1.GetHashCode();
        var hash2 = line2.GetHashCode();

        // assert
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // arrange
        var line = new GeoLine(1.5, 2.5, 3.5, 4.5);

        // act
        var result = line.ToString();

        // assert
        Assert.Contains("1.5", result);
        Assert.Contains("2.5", result);
        Assert.Contains("3.5", result);
        Assert.Contains("4.5", result);
    }
}
