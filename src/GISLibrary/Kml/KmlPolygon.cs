namespace Tudormobile.Kml;

/// <summary>
/// Represents a polygon geometry with an outer boundary and optional inner boundaries (holes).
/// </summary>
/// <param name="OuterBoundary">A list of coordinates defining the outer boundary of the polygon.</param>
/// <param name="InnerBoundaries">A list of inner boundary rings representing holes in the polygon.</param>
public record class KmlPolygon(List<(double Latitude, double Longitude, double Altitude)> OuterBoundary,
    List<List<(double Latitude, double Longitude, double Altitude)>> InnerBoundaries) : KmlGeometry
{
    /// <summary>
    /// Gets the type of this geometry.
    /// </summary>
    /// <value>Always returns <see cref="KmlGeometryType.Polygon"/>.</value>
    public override KmlGeometryType GeometryType => KmlGeometryType.Polygon;
}
