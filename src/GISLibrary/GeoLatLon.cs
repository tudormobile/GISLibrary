namespace Tudormobile.GIS;

/// <summary>
/// Represents a geographic location specified by latitude and longitude coordinates.
/// </summary>
public struct GeoLatLon
{
    /// <summary>
    /// Gets or sets the latitude component of the geographic coordinate.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the geographic longitude coordinate, in degrees.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Initializes a new instance of the GeoLatLon class with the specified latitude and longitude coordinates.
    /// </summary>
    /// <param name="latitude">The latitude component of the geographic coordinate, in decimal degrees. Valid values are between -90.0 and
    /// 90.0.</param>
    /// <param name="longitude">The longitude component of the geographic coordinate, in decimal degrees. Valid values are between -180.0 and
    /// 180.0.</param>
    public GeoLatLon(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

