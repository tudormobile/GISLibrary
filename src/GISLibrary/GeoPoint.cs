namespace Tudormobile.GIS;

/// <summary>
/// 'Point' geometry object
/// </summary>
public readonly struct GeoPoint(double x, double y) : IEquatable<GeoPoint>
{
    /// <summary>
    /// 'X' coordinate of the point.
    /// </summary>
    public double X => x;

    /// <summary>
    /// 'Y' coordinate of the point.
    /// </summary>
    public double Y => y;

    /// <summary>
    /// Determines whether the current GeoPoint instance is equal to the specified GeoPoint.
    /// </summary>
    /// <param name="other">The GeoPoint to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> have the same X and Y coordinates;
    /// otherwise, <see langword="false"/>.</returns>
    public bool Equals(GeoPoint other) => X == other.X && Y == other.Y;

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoPoint instance.
    /// </summary>
    /// <remarks>This method overrides Object.Equals and provides value-based equality comparison for GeoPoint
    /// instances.</remarks>
    /// <param name="obj">The object to compare with the current GeoPoint. Can be null.</param>
    /// <returns>true if the specified object is a GeoPoint and has the same values as the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is GeoPoint p && Equals(p);

    /// <summary>
    /// Serves as the default hash function for the current object.
    /// </summary>
    /// <remarks>Use this method when inserting instances of this type into hash-based collections such as
    /// HashSet or Dictionary. The hash code is based on the values of the X and Y properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two specified GeoPoint instances have the same value.
    /// </summary>
    /// <remarks>This operator performs a value comparison. It returns true if both GeoPoint instances
    /// represent the same coordinates.</remarks>
    /// <param name="left">The first GeoPoint to compare.</param>
    /// <param name="right">The second GeoPoint to compare.</param>
    /// <returns>true if the values of left and right are equal; otherwise, false.</returns>
    public static bool operator ==(GeoPoint left, GeoPoint right) => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoPoint instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==). Two GeoPoint
    /// instances are considered not equal if any of their corresponding values differ.</remarks>
    /// <param name="left">The first GeoPoint to compare.</param>
    /// <param name="right">The second GeoPoint to compare.</param>
    /// <returns>true if the specified GeoPoint instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoPoint left, GeoPoint right) => !left.Equals(right);

    /// <summary>
    /// Returns a string that represents the current GeoPoint in the format "GeoPoint(X, Y)".
    /// </summary>
    /// <returns>A string representation of the GeoPoint, including its X and Y coordinates.</returns>
    public override string ToString() => $"({X}, {Y})";
}
