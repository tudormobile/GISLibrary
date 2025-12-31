using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON MultiPoint coordinate object.
/// </summary>
public record GeoJSONMultiPoint : GeoJSONCoordinates
{
    /// <summary>
    /// Initializes a new instance of the GeoJSONMultiPoint class representing a collection of geographic points in
    /// GeoJSON format.
    /// </summary>
    /// <remarks>Use this constructor to create an empty GeoJSONMultiPoint object before adding points or
    /// configuring its properties. This class is typically used when working with spatial data that requires
    /// representing multiple point locations according to the GeoJSON specification.</remarks>
    public GeoJSONMultiPoint() { }

    /// <summary>
    /// Initializes a new instance of the GeoJSONMultiPoint class containing the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of GeoJSONPoint objects to include in the multi-point geometry. Cannot be null.</param>
    public GeoJSONMultiPoint(IEnumerable<GeoJSONPoint> points) { Points = [.. points]; }

    /// <summary>
    /// Initializes a new instance of the GeoJSONMultiPoint class containing multiple geographic points specified by
    /// their positions.
    /// </summary>
    /// <remarks>Each position in the collection is converted into a GeoJSONPoint and included in the
    /// multi-point geometry. The order of points in the resulting geometry matches the order of the provided
    /// positions.</remarks>
    /// <param name="positions">An enumerable collection of GeoJSONPosition objects representing the coordinates of each point in the
    /// multi-point geometry. Cannot be null or contain null elements.</param>
    public GeoJSONMultiPoint(IEnumerable<GeoJSONPosition> positions) { Points = [.. positions.Select(p => new GeoJSONPoint(p))]; }

    /// <summary>
    /// Gets or sets the list of points.
    /// </summary>
    public List<GeoJSONPoint> Points { get; set; } = [];

    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var point in Points)
        {
            point.WriteCoordinatesTo(writer);
        }
        writer.WriteEndArray();
    }
}
