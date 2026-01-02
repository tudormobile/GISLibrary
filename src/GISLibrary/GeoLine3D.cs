namespace Tudormobile.GIS;

/// <summary>
/// Represents a three-dimensional geographic line defined by a start and end point.
/// </summary>
/// <remarks>Use this structure to model a straight line segment in 3D geographic space, such as a path or
/// boundary between two locations. The coordinates of the start and end points are typically expressed in the same
/// spatial reference system.</remarks>
public readonly struct GeoLine3D(GeoPoint3D startPoint, GeoPoint3D endPoint) : IEquatable<GeoLine3D>
{
    /// <summary>
    /// Represents the starting point in three-dimensional geographic coordinates.
    /// </summary>
    public GeoPoint3D StartPoint => startPoint;

    /// <summary>
    /// Represents the end point of the geometric segment in three-dimensional space.
    /// </summary>
    public GeoPoint3D EndPoint => endPoint;

    /// <summary>
    /// Determines whether the current instance is equal to the specified GeoLine3D value.
    /// </summary>
    /// <param name="other">The GeoLine3D value to compare with the current instance.</param>
    /// <returns>true if the current instance and the specified value have the same latitude, longitude, and altitude; otherwise,
    /// false.</returns>
    public bool Equals(GeoLine3D other) => StartPoint == other.StartPoint;

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoLine3D instance.
    /// </summary>
    /// <remarks>This method supports value equality comparison for GeoLine3D instances. It returns <see
    /// langword="false"/> if the specified object is not a GeoLine3D.</remarks>
    /// <param name="obj">The object to compare with the current GeoLine3D instance.</param>
    /// <returns><see langword="true"/> if the specified object is a GeoLine3D and is equal to the current instance;
    /// otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is GeoLine3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function for the object.
    /// </summary>
    /// <remarks>Use the returned hash code when adding instances of this type to hash-based collections such
    /// as <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> or <see
    /// cref="System.Collections.Generic.HashSet{T}"/>. The hash code is based on the values of the Latitude, Longitude,
    /// and Altitude properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(StartPoint, EndPoint);

    /// <summary>
    /// Determines whether two GeoLine3D instances are equal.
    /// </summary>
    /// <param name="left">The first GeoLine3D instance to compare.</param>
    /// <param name="right">The second GeoLine3D instance to compare.</param>
    /// <returns>true if the specified GeoLine3D instances are equal; otherwise, false.</returns>
    public static bool operator ==(GeoLine3D left, GeoLine3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoLine3D instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==).</remarks>
    /// <param name="left">The first GeoLine3D instance to compare.</param>
    /// <param name="right">The second GeoLine3D instance to compare.</param>
    /// <returns>true if the specified instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoLine3D left, GeoLine3D right) => !left.Equals(right);

    /// <summary>
    /// Returns a string that represents the current geographic coordinates.
    /// </summary>
    /// <returns>A string in the format "({StartPoint}, {EndPoint})".</returns>
    public override string ToString() => $"({StartPoint}, {EndPoint})";
}
