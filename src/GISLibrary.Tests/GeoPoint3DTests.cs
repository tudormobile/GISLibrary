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
        Assert.AreEqual(0, pt.X);
        Assert.AreEqual(0, pt.Y);
        Assert.AreEqual(0, pt.Z);
    }
    [TestMethod]
    public void Constructor_SetsCoordinates()
    {
        // arrange & act
        var pt = new GeoPoint3D(1.0, 2.0, 3.0);
        Assert.AreEqual(1.0, pt.X);
        Assert.AreEqual(2.0, pt.Y);
        Assert.AreEqual(3.0, pt.Z);
    }
}
