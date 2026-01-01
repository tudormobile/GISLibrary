namespace Tudormobile.GIS;

/// <summary>
/// 'Point' geometry object
/// </summary>
public struct GeoPoint3D
{
    /// <summary>
    /// 'X' coordinate of the point.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// 'Y' coordinate of the point.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the Z-coordinate value.
    /// </summary>
    public double Z { get; set; }

    /// <summary>
    /// Initializes a new instance of the GeoPoint class with the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate of the point.</param>
    /// <param name="y">The Y coordinate of the point.</param>
    /// <param name="z">The Z coordinate of the point.</param>
    public GeoPoint3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
