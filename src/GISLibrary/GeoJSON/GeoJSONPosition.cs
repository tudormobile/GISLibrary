namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a geographic position (longitude, latitude and optional altitude).
/// </summary>
public struct GeoJSONPosition
{
    /// <summary>
    /// Initializes a new instance of the GeoJSONPosition class with the specified latitude, longitude, and optional
    /// altitude.
    /// </summary>
    /// <remarks>This constructor creates a position compatible with the GeoJSON specification, which
    /// represents geographic coordinates as an array of numbers in the order [longitude, latitude, altitude]. The
    /// altitude parameter is optional and may be omitted if not applicable.</remarks>
    /// <param name="latitude">The latitude component of the position, in decimal degrees. Valid values are between -90 and 90.</param>
    /// <param name="longitude">The longitude component of the position, in decimal degrees. Valid values are between -180 and 180.</param>
    /// <param name="altitude">The optional altitude component of the position, in meters above mean sea level. If null, the altitude is
    /// unspecified.</param>
    public GeoJSONPosition(double latitude, double longitude, double? altitude = null)
    {
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
    }

    public GeoJSONPosition(IEnumerable<double> values)
    {
        var count = values.Count();
        if (count < 2)
        {
            throw new ArgumentException("A GeoJSON position must have at least two values: longitude and latitude.");
        }
        if (count > 3)
        {
            throw new ArgumentException("A GeoJSON position can have at most three values: longitude, latitude, and altitude.");
        }
        Longitude = values.First();
        Latitude = values.Skip(1).First();
        Altitude = count == 3 ? values.Last() : null;
    }

    /// <summary>
    /// Longitude of the position.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Latitude of the position.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Optional altitude of the position.
    /// </summary>
    public double? Altitude { get; set; }
}
