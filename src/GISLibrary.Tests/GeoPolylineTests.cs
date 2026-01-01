using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoPolylineTests
{
    [TestMethod]
    public void Points_Default_IsNullOrEmpty()
    {
        // arrange
        var poly = new GeoPolyline();

        // act
        var pts = poly.Points;

        // assert
        Assert.IsEmpty(pts);
    }

    [TestMethod]
    public void Points_AssignList_HasCount()
    {
        // arrange
        var list = new List<GeoPoint> { new(1, 2), new(3, 4) };
        var poly = new GeoPolyline();
        poly.AddRange(list);

        // act
        var pts = poly.Points;

        // assert
        Assert.HasCount(2, pts);
    }
}
