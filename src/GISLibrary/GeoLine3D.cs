namespace Tudormobile.GIS;

/// <summary>
/// Represents a three-dimensional geographic line defined by a start and end point.
/// </summary>
/// <remarks>Use this structure to model a straight line segment in 3D geographic space, such as a path or
/// boundary between two locations. The coordinates of the start and end points are typically expressed in the same
/// spatial reference system.</remarks>
public struct GeoLine3D
{
    /// <summary>
    /// Represents the starting point in three-dimensional geographic coordinates.
    /// </summary>
    public GeoPoint3D StartPoint;

    /// <summary>
    /// Represents the end point of the geometric segment in three-dimensional space.
    /// </summary>
    public GeoPoint3D EndPoint;
}
