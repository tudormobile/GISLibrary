namespace Tudormobile.GIS;

/// <summary>
/// Represents a geographic location specified by latitude and longitude coordinates.
/// <param name="latitude">The latitude component of the geographic coordinate, in decimal degrees. Valid values are between -90.0 and
/// 90.0.</param>
/// <param name="longitude">The longitude component of the geographic coordinate, in decimal degrees. Valid values are between -180.0 and
/// 180.0.</param>
/// </summary>
public readonly struct GeoLatLon(double latitude, double longitude) : IEquatable<GeoLatLon>
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
    /// Determines whether the current GeoLatLon instance is equal to the specified GeoLatLon.
    /// </summary>
    /// <param name="other">The GeoLatLon to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance and <paramref name="other"/> have the same X and Y coordinates;
    /// otherwise, <see langword="false"/>.</returns>
    public bool Equals(GeoLatLon other) => Latitude == other.Latitude && Longitude == other.Longitude;

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoLatLon instance.
    /// </summary>
    /// <remarks>This method overrides Object.Equals and provides value-based equality comparison for GeoLatLon
    /// instances.</remarks>
    /// <param name="obj">The object to compare with the current GeoLatLon. Can be null.</param>
    /// <returns>true if the specified object is a GeoLatLon and has the same values as the current instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is GeoLatLon p && Equals(p);

    /// <summary>
    /// Serves as the default hash function for the current object.
    /// </summary>
    /// <remarks>Use this method when inserting instances of this type into hash-based collections such as
    /// HashSet or Dictionary. The hash code is based on the values of the X and Y properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Determines whether two specified GeoLatLon instances have the same value.
    /// </summary>
    /// <remarks>This operator performs a value comparison. It returns true if both GeoLatLon instances
    /// represent the same coordinates.</remarks>
    /// <param name="left">The first GeoLatLon to compare.</param>
    /// <param name="right">The second GeoLatLon to compare.</param>
    /// <returns>true if the values of left and right are equal; otherwise, false.</returns>
    public static bool operator ==(GeoLatLon left, GeoLatLon right) => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoLatLon instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==). Two GeoLatLon
    /// instances are considered not equal if any of their corresponding values differ.</remarks>
    /// <param name="left">The first GeoLatLon to compare.</param>
    /// <param name="right">The second GeoLatLon to compare.</param>
    /// <returns>true if the specified GeoLatLon instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoLatLon left, GeoLatLon right) => !left.Equals(right);
    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Latitude:F6}, {Longitude:F6}";
}

