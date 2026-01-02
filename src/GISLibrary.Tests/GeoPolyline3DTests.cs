using Tudormobile.GIS;

namespace GISLibrary.Tests;

[TestClass]
public class GeoPolyline3DTests
{
    [TestMethod]
    public void Points_Default_IsNullOrEmpty()
    {
        // arrange
        var poly = new GeoPolyline3D();

        // act
        var pts = poly.Points;

        // assert
        Assert.IsEmpty(pts);
    }

    [TestMethod]
    public void Points_AssignList_HasCount()
    {
        // arrange
        var list = new List<GeoPoint3D> { new(), new() };
        var poly = new GeoPolyline3D();
        poly.AddRange(list);

        // act
        var pts = poly.Points;

        // assert
        Assert.HasCount(2, pts);
    }


}
