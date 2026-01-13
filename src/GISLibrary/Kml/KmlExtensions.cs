namespace Tudormobile.Kml;

/// <summary>
/// Provides extension methods for KML documents, folders, and placemarks to simplify filtering and geometry conversion.
/// </summary>
public static class KmlExtensions
{
    /// <summary>
    /// Gets all placemarks in the KML document that have Point geometry.
    /// </summary>
    /// <param name="doc">The KML document to search.</param>
    /// <returns>An enumerable collection of placemarks with Point geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllPointPlacemarks(this KmlDocument doc)
        => doc.AllPlacemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.Point);

    /// <summary>
    /// Gets all placemarks in the KML document that have LineString geometry.
    /// </summary>
    /// <param name="doc">The KML document to search.</param>
    /// <returns>An enumerable collection of placemarks with LineString geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllLineStringPlacemarks(this KmlDocument doc)
        => doc.AllPlacemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.LineString);

    /// <summary>
    /// Gets all placemarks in the KML document that have Polygon geometry.
    /// </summary>
    /// <param name="doc">The KML document to search.</param>
    /// <returns>An enumerable collection of placemarks with Polygon geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllPolygonPlacemarks(this KmlDocument doc)
        => doc.AllPlacemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.Polygon);

    /// <summary>
    /// Gets all placemarks in the KML folder that have Point geometry.
    /// </summary>
    /// <param name="folder">The KML folder to search.</param>
    /// <returns>An enumerable collection of placemarks with Point geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllPointPlacemarks(this KmlFolder folder)
        => folder.Placemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.Point);

    /// <summary>
    /// Gets all placemarks in the KML folder that have LineString geometry.
    /// </summary>
    /// <param name="folder">The KML folder to search.</param>
    /// <returns>An enumerable collection of placemarks with LineString geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllLineStringPlacemarks(this KmlFolder folder)
        => folder.Placemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.LineString);

    /// <summary>
    /// Gets all placemarks in the KML folder that have Polygon geometry.
    /// </summary>
    /// <param name="folder">The KML folder to search.</param>
    /// <returns>An enumerable collection of placemarks with Polygon geometry.</returns>
    public static IEnumerable<KmlPlacemark> AllPolygonPlacemarks(this KmlFolder folder)
        => folder.Placemarks.Where(x => x.Geometry.GeometryType == KmlGeometryType.Polygon);

    /// <summary>
    /// Converts the placemark's geometry to a <see cref="KmlLineString"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlLineString"/> instance.</returns>
    /// <exception cref="InvalidCastException">Thrown when the placemark's geometry is not a LineString.</exception>
    public static KmlLineString ToLineString(this KmlPlacemark placemark)
        => (KmlLineString)placemark.Geometry;

    /// <summary>
    /// Attempts to convert the placemark's geometry to a <see cref="KmlLineString"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlLineString"/> instance if the geometry is a LineString; otherwise, null.</returns>
    public static KmlLineString? AsLineString(this KmlPlacemark placemark)
        => placemark.Geometry.GeometryType == KmlGeometryType.LineString
        ? (KmlLineString)placemark.Geometry
        : null;

    /// <summary>
    /// Converts the placemark's geometry to a <see cref="KmlPolygon"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlPolygon"/> instance.</returns>
    /// <exception cref="InvalidCastException">Thrown when the placemark's geometry is not a Polygon.</exception>
    public static KmlPolygon ToPolygon(this KmlPlacemark placemark)
    => (KmlPolygon)placemark.Geometry;

    /// <summary>
    /// Attempts to convert the placemark's geometry to a <see cref="KmlPolygon"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlPolygon"/> instance if the geometry is a Polygon; otherwise, null.</returns>
    public static KmlPolygon? AsPolygon(this KmlPlacemark placemark)
        => placemark.Geometry.GeometryType == KmlGeometryType.Polygon
        ? (KmlPolygon)placemark.Geometry
        : null;

    /// <summary>
    /// Converts the placemark's geometry to a <see cref="KmlPoint"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlPoint"/> instance.</returns>
    /// <exception cref="InvalidCastException">Thrown when the placemark's geometry is not a Point.</exception>
    public static KmlPoint ToPoint(this KmlPlacemark placemark)
    => (KmlPoint)placemark.Geometry;

    /// <summary>
    /// Attempts to convert the placemark's geometry to a <see cref="KmlPoint"/>.
    /// </summary>
    /// <param name="placemark">The placemark whose geometry to convert.</param>
    /// <returns>A <see cref="KmlPoint"/> instance if the geometry is a Point; otherwise, null.</returns>
    public static KmlPoint? AsPoint(this KmlPlacemark placemark)
        => placemark.Geometry.GeometryType == KmlGeometryType.Point
        ? (KmlPoint)placemark.Geometry
        : null;
}
