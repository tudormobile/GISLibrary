namespace Tudormobile.GIS;
/// <summary>
/// Represents a line segment defined by two geographic points.
/// </summary>
/// <param name="startPoint">The geographic point at which the line segment starts. Cannot be null.</param>
/// <param name="endPoint">The geographic point at which the line segment ends. Cannot be null.</param>
/// <remarks>The GeoLine structure provides constructors and implicit conversion operators for creating line
/// segments from coordinate pairs or arrays. It is commonly used to represent straight paths or boundaries between two
/// locations in geographic coordinate systems.</remarks>
public readonly struct GeoLine(GeoPoint startPoint, GeoPoint endPoint) : IEquatable<GeoLine>
{
    /// <summary>
    /// Represents the starting geographic point of the route or segment.
    /// </summary>
    public GeoPoint StartPoint => startPoint;

    /// <summary>
    /// Represents the geographic end point of the segment or route.
    /// </summary>
    public GeoPoint EndPoint => endPoint;

    /// <summary>
    /// Initializes a new instance of the GeoLine class that represents a line segment defined by two geographic points.
    /// </summary>
    /// <param name="x1">The X-coordinate of the start point of the line segment.</param>
    /// <param name="y1">The Y-coordinate of the start point of the line segment.</param>
    /// <param name="x2">The X-coordinate of the end point of the line segment.</param>
    /// <param name="y2">The Y-coordinate of the end point of the line segment.</param>
    public GeoLine(double x1, double y1, double x2, double y2)
        : this(new GeoPoint(x1, y1), new GeoPoint(x2, y2)) { }

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

    /// <summary>
    /// Determines whether the specified GeoLine is equal to the current GeoLine.
    /// </summary>
    /// <param name="other">The GeoLine to compare with the current GeoLine.</param>
    /// <returns>true if the specified GeoLine is equal to the current GeoLine; otherwise, false.</returns>
    public bool Equals(GeoLine other)
        => StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoLine.
    /// </summary>
    /// <param name="obj">The object to compare with the current GeoLine.</param>
    /// <returns>true if the specified object is a GeoLine and is equal to the current GeoLine; otherwise, false.</returns>
    public override bool Equals(object? obj)
        => obj is GeoLine other && Equals(other);

    /// <summary>
    /// Returns the hash code for this GeoLine.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
        => HashCode.Combine(StartPoint, EndPoint);

    /// <summary>
    /// Determines whether two GeoLine instances are equal.
    /// </summary>
    /// <param name="left">The first GeoLine to compare.</param>
    /// <param name="right">The second GeoLine to compare.</param>
    /// <returns>true if the two GeoLine instances are equal; otherwise, false.</returns>
    public static bool operator ==(GeoLine left, GeoLine right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoLine instances are not equal.
    /// </summary>
    /// <param name="left">The first GeoLine to compare.</param>
    /// <param name="right">The second GeoLine to compare.</param>
    /// <returns>true if the two GeoLine instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoLine left, GeoLine right)
        => !left.Equals(right);

    /// <summary>
    /// Returns a string that represents the current object, including the start and end points.
    /// </summary>
    /// <returns>A string in the format "(StartPoint, EndPoint)" that represents the current instance.</returns>
    public override string ToString() => $"({StartPoint}, {EndPoint})";
}
