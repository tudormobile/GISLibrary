namespace Tudormobile.Kml;

/// <summary>
/// Represents a point geometry with latitude, longitude, and altitude.
/// </summary>
/// <param name="Latitude">The latitude coordinate in decimal degrees.</param>
/// <param name="Longitude">The longitude coordinate in decimal degrees.</param>
/// <param name="Altitude">The altitude value in meters.</param>
public record class KmlPoint(double Latitude, double Longitude, double Altitude) : KmlGeometry
{
    /// <summary>
    /// Gets the type of this geometry.
    /// </summary>
    /// <value>Always returns <see cref="KmlGeometryType.Point"/>.</value>
    public override KmlGeometryType GeometryType => KmlGeometryType.Point;
}
