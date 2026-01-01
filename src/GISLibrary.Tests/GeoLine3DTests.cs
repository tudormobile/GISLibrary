using Microsoft.VisualStudio.TestTools.UnitTesting;
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
}
