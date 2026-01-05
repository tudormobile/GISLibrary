namespace Tudormobile.Kml;

/// <summary>
/// Specifies the type of a KML geometry.
/// </summary>
public enum KmlGeometryType
{
    /// <summary>
    /// Represents an unsupported operation, feature, or value.
    /// </summary>
    /// <remarks>This type or member is typically used to indicate that a particular functionality is not
    /// available or not implemented in the current context. Attempting to use unsupported members may result in
    /// exceptions or undefined behavior.</remarks>
    Unsupported,

    /// <summary>
    /// Represents a point geometry.
    /// </summary>
    Point,

    /// <summary>
    /// Represents a line string geometry.
    /// </summary>
    LineString,

    /// <summary>
    /// Represents a polygon geometry.
    /// </summary>
    Polygon
}


