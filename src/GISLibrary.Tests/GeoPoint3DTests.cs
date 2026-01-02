using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoPoint3DTests
{
    [TestMethod]
    public void Default_IsZero_IfTypeExists()
    {
        // arrange & act
        var pt = default(GeoPoint3D);

        // assert
        Assert.AreEqual(0, pt.X);
        Assert.AreEqual(0, pt.Y);
        Assert.AreEqual(0, pt.Z);
    }

    [TestMethod]
    public void Constructor_SetsCoordinates()
    {
        // arrange & act
        var pt = new GeoPoint3D(1.0, 2.0, 3.0);

        // assert
        Assert.AreEqual(1.0, pt.X);
        Assert.AreEqual(2.0, pt.Y);
        Assert.AreEqual(3.0, pt.Z);
    }

    [TestMethod]
    public void Equals_SameCoordinates_ReturnsTrue()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_DifferentX_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.1, 2.0, 3.0);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_DifferentY_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.1, 3.0);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_DifferentZ_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.1);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_Object_SameCoordinates_ReturnsTrue()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        object pt2 = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_Object_Null_ReturnsFalse()
    {
        // arrange
        var pt = new GeoPoint3D(1.0, 2.0, 3.0);
        object? nullObj = null;

        // act
        var result = pt.Equals(nullObj);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_Object_DifferentType_ReturnsFalse()
    {
        // arrange
        var pt = new GeoPoint3D(1.0, 2.0, 3.0);
        object obj = "not a GeoPoint3D";

        // act
        var result = pt.Equals(obj);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsOperator_SameCoordinates_ReturnsTrue()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        var result = pt1 == pt2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EqualsOperator_DifferentCoordinates_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.1, 2.1, 3.1);

        // act
        var result = pt1 == pt2;

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void NotEqualsOperator_SameCoordinates_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        var result = pt1 != pt2;

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void NotEqualsOperator_DifferentCoordinates_ReturnsTrue()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.1, 2.1, 3.1);

        // act
        var result = pt1 != pt2;

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetHashCode_SameCoordinates_ReturnsSameHash()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        var hash1 = pt1.GetHashCode();
        var hash2 = pt2.GetHashCode();

        // assert
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void GetHashCode_DifferentCoordinates_ReturnsDifferentHash()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.1, 2.1, 3.1);

        // act
        var hash1 = pt1.GetHashCode();
        var hash2 = pt2.GetHashCode();

        // assert
        Assert.AreNotEqual(hash1, hash2);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // arrange
        var pt = new GeoPoint3D(1.5, 2.5, 3.5);

        // act
        var result = pt.ToString();

        // assert
        Assert.AreEqual("(1.5, 2.5, 3.5)", result);
    }

    [TestMethod]
    public void ToString_NegativeCoordinates_ReturnsFormattedString()
    {
        // arrange
        var pt = new GeoPoint3D(-1.5, -2.5, -3.5);

        // act
        var result = pt.ToString();

        // assert
        Assert.AreEqual("(-1.5, -2.5, -3.5)", result);
    }

    [TestMethod]
    public void ToString_ZeroCoordinates_ReturnsFormattedString()
    {
        // arrange
        var pt = new GeoPoint3D(0, 0, 0);

        // act
        var result = pt.ToString();

        // assert
        Assert.AreEqual("(0, 0, 0)", result);
    }

    [TestMethod]
    public void ToString_MixedCoordinates_ReturnsFormattedString()
    {
        // arrange
        var pt = new GeoPoint3D(-10.5, 0, 20.75);

        // act
        var result = pt.ToString();

        // assert
        Assert.AreEqual("(-10.5, 0, 20.75)", result);
    }

    [TestMethod]
    public void HashSet_CanStoreAndRetrievePoints()
    {
        // arrange
        var set = new HashSet<GeoPoint3D>();
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt3 = new GeoPoint3D(3.0, 4.0, 5.0);

        // act
        set.Add(pt1);
        set.Add(pt2); // Should not add duplicate
        set.Add(pt3);

        // assert
        Assert.HasCount(2, set);
        Assert.Contains(pt1, set);
        Assert.Contains(pt3, set);
    }

    [TestMethod]
    public void Dictionary_CanUsePointAsKey()
    {
        // arrange
        var dict = new Dictionary<GeoPoint3D, string>();
        var pt = new GeoPoint3D(1.0, 2.0, 3.0);

        // act
        dict[pt] = "Test Location";
        var result = dict[new GeoPoint3D(1.0, 2.0, 3.0)];

        // assert
        Assert.AreEqual("Test Location", result);
    }

    [TestMethod]
    public void Constructor_WithLargeValues_StoresCorrectly()
    {
        // arrange
        double x = 1234567.89;
        double y = 9876543.21;
        double z = 5555555.55;

        // act
        var pt = new GeoPoint3D(x, y, z);

        // assert
        Assert.AreEqual(x, pt.X);
        Assert.AreEqual(y, pt.Y);
        Assert.AreEqual(z, pt.Z);
    }

    [TestMethod]
    public void Constructor_WithVerySmallValues_StoresCorrectly()
    {
        // arrange
        double x = 0.0000001;
        double y = 0.0000002;
        double z = 0.0000003;

        // act
        var pt = new GeoPoint3D(x, y, z);

        // assert
        Assert.AreEqual(x, pt.X);
        Assert.AreEqual(y, pt.Y);
        Assert.AreEqual(z, pt.Z);
    }

    [TestMethod]
    public void Equals_WithDoubleMaxValue_WorksCorrectly()
    {
        // arrange
        var pt1 = new GeoPoint3D(double.MaxValue, double.MaxValue, double.MaxValue);
        var pt2 = new GeoPoint3D(double.MaxValue, double.MaxValue, double.MaxValue);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_WithDoubleMinValue_WorksCorrectly()
    {
        // arrange
        var pt1 = new GeoPoint3D(double.MinValue, double.MinValue, double.MinValue);
        var pt2 = new GeoPoint3D(double.MinValue, double.MinValue, double.MinValue);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_WithNaN_WorksCorrectly()
    {
        // arrange
        var pt1 = new GeoPoint3D(double.NaN, 1.0, 2.0);
        var pt2 = new GeoPoint3D(double.NaN, 1.0, 2.0);

        // act
        var result = pt1.Equals(pt2);

        // assert
        // NaN != NaN by IEEE 754 standard
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithInfinity_WorksCorrectly()
    {
        // arrange
        var pt1 = new GeoPoint3D(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
        var pt2 = new GeoPoint3D(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_OnlyZDifferent_ReturnsFalse()
    {
        // arrange
        var pt1 = new GeoPoint3D(1.0, 2.0, 3.0);
        var pt2 = new GeoPoint3D(1.0, 2.0, 3.0001);

        // act
        var result = pt1.Equals(pt2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ToString_WithScientificNotation_FormatsCorrectly()
    {
        // arrange
        var pt = new GeoPoint3D(1.23e10, 4.56e-5, 7.89e2);

        // act
        var result = pt.ToString();

        // assert
        Assert.AreEqual($"({1.23e10}, {4.56e-5}, {7.89e2})", result);
    }
}
