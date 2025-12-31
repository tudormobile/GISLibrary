using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON Point coordinate object.
/// </summary>
public record GeoJSONPoint : GeoJSONCoordinates
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJSONPoint"/> class.
    /// </summary>
    public GeoJSONPoint()
    {
        Position = default;
    }

    /// <summary>
    /// Initializes a new instance of the GeoJSONPoint class with the specified latitude, longitude, and optional
    /// altitude.
    /// </summary>
    /// <param name="latitude">The latitude component of the point, in decimal degrees. Valid values are between -90 and 90.</param>
    /// <param name="longitude">The longitude component of the point, in decimal degrees. Valid values are between -180 and 180.</param>
    /// <param name="altitude">The optional altitude component of the point, in meters above mean sea level. If null, the altitude is not
    /// specified.</param>
    public GeoJSONPoint(double latitude, double longitude, double? altitude = null)
        : this(new GeoJSONPosition(latitude, longitude, altitude)) { }

    /// <summary>
    /// Initializes a new instance of the GeoJSONPoint class using the specified coordinate values.
    /// </summary>
    /// <remarks>The order of values in the collection must follow the GeoJSON specification: longitude,
    /// latitude, and optionally altitude. Additional elements beyond the third are ignored.</remarks>
    /// <param name="values">An enumerable collection of double values representing the coordinates of the point. The collection must contain
    /// at least two elements: longitude and latitude. An optional third element specifies altitude.</param>
    public GeoJSONPoint(IEnumerable<double> values)
        : this(new GeoJSONPosition(values)) { }

    /// <summary>
    /// Initializes a new instance of the GeoJSONPoint class with the specified geographic position.
    /// </summary>
    /// <param name="position">The geographic position that defines the coordinates of the point. Cannot be null.</param>
    public GeoJSONPoint(GeoJSONPosition position) { Position = position; }

    /// <summary>
    /// Gets or sets the point position.
    /// </summary>
    public GeoJSONPosition Position { get; set; }

    /// <summary>
    /// Implicitly converts a <see cref="GeoJSONPoint"/> to a <see cref="GeoJSONPosition"/>.
    /// If the source point is <c>null</c>, the default <see cref="GeoJSONPosition"/> is returned.
    /// </summary>
    /// <param name="point">The source point to convert.</param>
    public static implicit operator GeoJSONPosition(GeoJSONPoint? point) => point?.Position ?? default;

    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(Position.Longitude);
        writer.WriteNumberValue(Position.Latitude);
        if (Position.Altitude.HasValue)
        {
            writer.WriteNumberValue(Position.Altitude.Value);
        }
        writer.WriteEndArray();
    }
}
