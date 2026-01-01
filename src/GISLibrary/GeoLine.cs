namespace Tudormobile.GIS;
/// <summary>
/// Represents a line segment defined by two geographic points.
/// </summary>
/// <remarks>The GeoLine structure provides constructors and implicit conversion operators for creating line
/// segments from coordinate pairs or arrays. It is commonly used to represent straight paths or boundaries between two
/// locations in geographic coordinate systems.</remarks>
public struct GeoLine
{
    /// <summary>
    /// Represents the starting geographic point of the route or segment.
    /// </summary>
    public GeoPoint StartPoint;

    /// <summary>
    /// Represents the geographic end point of the segment or route.
    /// </summary>
    public GeoPoint EndPoint;

    /// <summary>
    /// Initializes a new instance of the GeoLine class that represents a line segment defined by the specified start
    /// and end geographic points.
    /// </summary>
    /// <param name="startPoint">The geographic point at which the line segment starts. Cannot be null.</param>
    /// <param name="endPoint">The geographic point at which the line segment ends. Cannot be null.</param>
    public GeoLine(GeoPoint startPoint, GeoPoint endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    /// <summary>
    /// Initializes a new instance of the GeoLine class that represents a line segment defined by two geographic points.
    /// </summary>
    /// <param name="x1">The X-coordinate of the start point of the line segment.</param>
    /// <param name="y1">The Y-coordinate of the start point of the line segment.</param>
    /// <param name="x2">The X-coordinate of the end point of the line segment.</param>
    /// <param name="y2">The Y-coordinate of the end point of the line segment.</param>
    public GeoLine(double x1, double y1, double x2, double y2)
    {
        StartPoint = new GeoPoint(x1, y1);
        EndPoint = new GeoPoint(x2, y2);
    }

    /// <summary>
    /// Converts a tuple containing two points into a GeoLine instance representing the line segment between those
    /// points.
    /// </summary>
    /// <param name="t">A tuple containing four double values: (x1, y1, x2, y2), where (x1, y1) and (x2, y2) specify the coordinates of
    /// the start and end points of the line segment.</param>
    public static implicit operator GeoLine((double x1, double y1, double x2, double y2) t)
        => new(t.x1, t.y1, t.x2, t.y2);

    /// <summary>
    /// Converts a tuple of two points to a GeoLine instance representing the line segment between them.
    /// </summary>
    /// <remarks>This operator enables implicit conversion from a tuple of two (x, y) points to a GeoLine,
    /// allowing for concise creation of line segments from coordinate pairs.</remarks>
    /// <param name="t">A tuple containing two points, each represented as a tuple of x and y coordinates, that define the start and end
    /// points of the line segment.</param>
    public static implicit operator GeoLine(((double x, double y) a, (double x, double y) b) t)
        => new(t.a.x, t.a.y, t.b.x, t.b.y);
}
