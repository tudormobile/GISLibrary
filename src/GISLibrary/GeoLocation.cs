namespace Tudormobile.GIS;

/// <summary>
/// Represents a geographic location specified by latitude and longitude coordinates.
/// <param name="latitude">The latitude component of the geographic coordinate, in decimal degrees. Valid values are between -90.0 and
/// 90.0.</param>
/// <param name="longitude">The longitude component of the geographic coordinate, in decimal degrees. Valid values are between -180.0 and
/// 180.0.</param>
/// </summary>
public readonly struct GeoLocation(double latitude, double longitude) : IEquatable<GeoLocation>
{
    /// <summary>
    /// Gets or sets the latitude component of the geographic coordinate.
    /// </summary>
    public double Latitude => latitude;

    /// <summary>
    /// Gets or sets the geographic longitude coordinate, in degrees.
    /// </summary>
    public double Longitude => longitude;

    /// <summary>
    /// Determines whether the current position instance is equal to the specified position.
    /// </summary>
    /// <param name="other">The position to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> have the same coordinates;
    /// otherwise, <see langword="false"/>.</returns>
    public bool Equals(GeoLocation other) =>
        Math.Abs(Latitude - other.Latitude) < double.Epsilon &&
        Math.Abs(Longitude - other.Longitude) < double.Epsilon;

    /// <summary>
    /// Determines whether the specified object is equal to the current position instance.
    /// </summary>
    /// <remarks>This method overrides Object.Equals and provides value-based equality comparison for position
    /// instances.</remarks>
    /// <param name="obj">The object to compare with the current position. Can be null.</param>
    /// <returns>true if the specified object is a position and has the same values as the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is GeoLocation p && Equals(p);

    /// <summary>
    /// Serves as the default hash function for the current object.
    /// </summary>
    /// <remarks>Use this method when inserting instances of this type into hash-based collections such as
    /// HashSet or Dictionary. The hash code is based on the values of the Latitude and Longitude properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Determines whether two specified position instances have the same value.
    /// </summary>
    /// <remarks>This operator performs a value comparison. It returns true if both position instances
    /// represent the same coordinates.</remarks>
    /// <param name="left">The first position to compare.</param>
    /// <param name="right">The second position to compare.</param>
    /// <returns>true if the values of left and right are equal; otherwise, false.</returns>
    public static bool operator ==(GeoLocation left, GeoLocation right) => left.Equals(right);

    /// <summary>
    /// Determines whether two position instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==). Two position
    /// instances are considered not equal if any of their corresponding values differ.</remarks>
    /// <param name="left">The first position to compare.</param>
    /// <param name="right">The second position to compare.</param>
    /// <returns>true if the specified position instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoLocation left, GeoLocation right) => !left.Equals(right);
    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Latitude:F6}, {Longitude:F6}";
}

