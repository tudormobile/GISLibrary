namespace Tudormobile.GIS;

/// <summary>
/// Represents a geographic location specified by latitude, longitude, and altitude coordinates.
/// </summary>
/// <param name="latitude">The latitude component of the geographic coordinate, in decimal degrees. Valid values are between -90.0 and
/// 90.0.</param>
/// <param name="longitude">The longitude component of the geographic coordinate, in decimal degrees. Valid values are between -180.0 and
/// 180.0.</param>
/// <param name="altitude">The altitude value.</param>
public readonly struct GeoLatLonAlt(double latitude, double longitude, double altitude) : IEquatable<GeoLatLonAlt>
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
    /// Gets or sets the altitude value.
    /// </summary>
    public double Altitude => altitude;

    /// <summary>
    /// Determines whether the current instance is equal to the specified GeoLatLonAlt value.
    /// </summary>
    /// <param name="other">The GeoLatLonAlt value to compare with the current instance.</param>
    /// <returns>true if the current instance and the specified value have the same latitude, longitude, and altitude; otherwise,
    /// false.</returns>
    public bool Equals(GeoLatLonAlt other)
    {
        return Latitude == other.Latitude &&
               Longitude == other.Longitude &&
               Altitude == other.Altitude;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current GeoLatLonAlt instance.
    /// </summary>
    /// <remarks>This method supports value equality comparison for GeoLatLonAlt instances. It returns <see
    /// langword="false"/> if the specified object is not a GeoLatLonAlt.</remarks>
    /// <param name="obj">The object to compare with the current GeoLatLonAlt instance.</param>
    /// <returns><see langword="true"/> if the specified object is a GeoLatLonAlt and is equal to the current instance;
    /// otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is GeoLatLonAlt other && Equals(other);

    /// <summary>
    /// Serves as the default hash function for the object.
    /// </summary>
    /// <remarks>Use the returned hash code when adding instances of this type to hash-based collections such
    /// as <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> or <see
    /// cref="System.Collections.Generic.HashSet{T}"/>. The hash code is based on the values of the Latitude, Longitude,
    /// and Altitude properties.</remarks>
    /// <returns>A 32-bit signed integer hash code that represents the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude, Altitude);

    /// <summary>
    /// Determines whether two GeoLatLonAlt instances are equal.
    /// </summary>
    /// <param name="left">The first GeoLatLonAlt instance to compare.</param>
    /// <param name="right">The second GeoLatLonAlt instance to compare.</param>
    /// <returns>true if the specified GeoLatLonAlt instances are equal; otherwise, false.</returns>
    public static bool operator ==(GeoLatLonAlt left, GeoLatLonAlt right) => left.Equals(right);

    /// <summary>
    /// Determines whether two GeoLatLonAlt instances are not equal.
    /// </summary>
    /// <remarks>This operator returns the opposite result of the equality operator (==).</remarks>
    /// <param name="left">The first GeoLatLonAlt instance to compare.</param>
    /// <param name="right">The second GeoLatLonAlt instance to compare.</param>
    /// <returns>true if the specified instances are not equal; otherwise, false.</returns>
    public static bool operator !=(GeoLatLonAlt left, GeoLatLonAlt right) => !left.Equals(right);

    /// <summary>
    /// Returns a string that represents the current geographic coordinates, including latitude, longitude, and
    /// altitude.
    /// </summary>
    /// <returns>A string in the format "{Latitude}, {Longitude}, {Altitude}", where latitude and longitude are formatted to six
    /// decimal places.</returns>
    public override string ToString() => $"{Latitude:F6}, {Longitude:F6}, {Altitude}";
}

