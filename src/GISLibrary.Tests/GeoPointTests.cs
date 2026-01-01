using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoPointTests
{
    [TestMethod]
    public void Constructor_SetsCoordinates()
    {
        // arrange
        double x = 1.234;
        double y = 5.678;

        // act
        var pt = new GeoPoint(x, y);

        // assert
        Assert.AreEqual(x, pt.X);
        Assert.AreEqual(y, pt.Y);
    }

    [TestMethod]
    public void Default_IsZero()
    {
        // arrange
        var pt = default(GeoPoint);

        // act
        var x = pt.X;
        var y = pt.Y;

        // assert
        Assert.AreEqual(0, x);
        Assert.AreEqual(0, y);
    }
}
