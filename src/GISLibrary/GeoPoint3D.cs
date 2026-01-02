namespace Tudormobile.GIS;

/// <summary>
/// 'Point' geometry object
/// </summary>
public readonly struct GeoPoint3D(double x, double y, double z) : IEquatable<GeoPoint3D>
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
    /// Gets or sets the Z-coordinate value.
    /// </summary>
    public double Z => z;

    /// <summary>
    /// Determines whether the current GeoPoint3D instance is equal to the specified GeoPoint3D.
    /// </summary>
    /// <param name="other">The GeoPoint3D to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> have the same X and Y coordinates;
    /// otherwise, <see langword="false"/>.</returns>
    public bool Equals(GeoPoint3D other) => X == other.X && Y == other.Y && Z == other.Z;

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoPoint3D instance.
    /// </summary>
    /// <remarks>This method overrides Object.Equals and provides value-based equality comparison for GeoPoint3D
    /// instances.</remarks>
    /// <param name="obj">The object to compare with the current GeoPoint3D. Can be null.</param>
    /// <returns>true if the specified object is a GeoPoint3D and has the same values as the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is GeoPoint3D p && Equals(p);

    /// <summary>
    /// Serves as the default hash function for the current object.
    /// </summary>
    /// <remarks>Use this method when inserting instances of this type into hash-based collections such as
    /// HashSet or Dictionary. The hash code is based on the values of the X and Y properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <summary>
    /// Determines whether two specified GeoPoint3D instances have the same value.
    /// </summary>
    /// <remarks>This operator performs a value comparison. It returns true if both GeoPoint3D instances
    /// represent the same coordinates.</remarks>
    /// <param name="left">The first GeoPoint3D to compare.</param>
    /// <param name="right">The second GeoPoint3D to compare.</param>
    /// <returns>true if the values of left and right are equal; otherwise, false.</returns>
    public static bool operator ==(GeoPoint3D left, GeoPoint3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoPoint3D instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==). Two GeoPoint3D
    /// instances are considered not equal if any of their corresponding values differ.</remarks>
    /// <param name="left">The first GeoPoint3D to compare.</param>
    /// <param name="right">The second GeoPoint3D to compare.</param>
    /// <returns>true if the specified GeoPoint3D instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoPoint3D left, GeoPoint3D right) => !left.Equals(right);

    /// <summary>
    /// Returns a string that represents the current GeoPoint3D in the format "GeoPoint3D(X, Y)".
    /// </summary>
    /// <returns>A string representation of the GeoPoint3D, including its X and Y coordinates.</returns>
    public override string ToString() => $"({X}, {Y}, {Z})";
}
