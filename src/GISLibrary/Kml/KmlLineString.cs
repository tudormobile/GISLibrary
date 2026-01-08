namespace Tudormobile.Kml;

/// <summary>
/// Represents a line string geometry consisting of multiple coordinate points.
/// </summary>
/// <param name="Coordinates">A list of coordinates defining the line string path.</param>
public record class KmlLineString(List<(double Latitude, double Longitude, double Altitude)> Coordinates) : KmlGeometry
{
    /// <summary>
    /// Gets the type of this geometry.
    /// </summary>
    /// <value>Always returns <see cref="KmlGeometryType.LineString"/>.</value>
    public override KmlGeometryType GeometryType => KmlGeometryType.LineString;
}
