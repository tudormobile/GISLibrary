using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoLine3DTests
{
    [TestMethod]
    public void Default_IsZero()
    {
        // arrange
        var line = default(GeoLine3D);

        // act
        var sx = line.StartPoint.X;
        var sy = line.StartPoint.Y;

        // assert
        Assert.AreEqual(0, sx);
        Assert.AreEqual(0, sy);
    }

    [TestMethod]
    public void Equals_SameValues_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        var line2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));

        // act
        var result = line1.Equals(line2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // arrange
        var line1 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        var line2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(7, 8, 9));

        // act
        var result = line1.Equals(line2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_Null_ReturnsFalse()
    {
        // arrange
        var line = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));

        // act
        var result = line.Equals(null);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsOperator_SameValues_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        var line2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));

        // act
        var result = line1 == line2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void NotEqualsOperator_DifferentValues_ReturnsTrue()
    {
        // arrange
        var line1 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        var line2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(7, 8, 9));

        // act
        var result = line1 != line2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        // arrange
        var line1 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        var line2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));

        // act
        var hash1 = line1.GetHashCode();
        var hash2 = line2.GetHashCode();

        // assert
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void ToString_ReturnsExpectedFormat()
    {
        // arrange
        var line = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));

        // act
        var result = line.ToString();

        // assert
        Assert.IsNotNull(result);
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
        Assert.Contains("4", result);
        Assert.Contains("5", result);
        Assert.Contains("6", result);
    }

    [TestMethod]
    public void Equals_WithNull_ReturnsFalse()
    {
        // arrange
        var pos = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        object pos2 = new GeoLine3D(new GeoPoint3D(1, 2, 3), new GeoPoint3D(4, 5, 6));
        object pos3 = new GeoLine3D(new GeoPoint3D(31, 42, 53), new GeoPoint3D(64, 75, 86));
        object pos4 = pos;
        object pos5 = null!;

        // act & assert
        Assert.IsTrue(pos.Equals(pos4));
        Assert.IsFalse(pos.Equals(pos5));
        Assert.IsTrue(pos.Equals(pos2));
        Assert.IsFalse(pos.Equals(pos3));

    }
}
