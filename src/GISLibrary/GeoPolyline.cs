namespace Tudormobile.GIS;


/// <summary>
/// Represents a sequence of geographic points that define a polyline on the Earth's surface.
/// </summary>
/// <remarks>A GeoPolyline is an ordered collection of GeoPoint instances, where each point represents a vertex of
/// the polyline. The polyline can be used to model paths, routes, or boundaries in geographic applications. The class
/// inherits from List&lt;GeoPoint&gt;, allowing standard list operations on the points.
/// </remarks>
public class GeoPolyline : List<GeoPoint>
{
    /// <summary>
    /// Gets the collection of geographic points that define this shape.
    /// </summary>
    public IList<GeoPoint> Points => this;
}
