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

    /// <summary>
    /// Initializes a new instance of the GeoJSONPosition class using the specified coordinate values.
    /// </summary>
    /// <remarks>The first value in the collection is interpreted as longitude, the second as latitude, and
    /// the third (if present) as altitude. This constructor enforces the GeoJSON specification for position
    /// arrays.</remarks>
    /// <param name="values">An enumerable collection of double values representing the position coordinates. The collection must contain two
    /// or three elements: longitude, latitude, and optionally altitude.</param>
    /// <exception cref="ArgumentException">Thrown if the collection contains fewer than two or more than three values.</exception>
    public GeoJSONPosition(IEnumerable<double> values)
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException("A GeoJSON position must have at least two values: longitude and latitude.");
        }
        Longitude = enumerator.Current;
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException("A GeoJSON position must have at least two values: longitude and latitude.");
        }
        Latitude = enumerator.Current;
        if (enumerator.MoveNext())
        {
            Altitude = enumerator.Current;
            if (enumerator.MoveNext())
            {
                throw new ArgumentException("A GeoJSON position can have at most three values: longitude, latitude, and altitude.");
            }
        }
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
