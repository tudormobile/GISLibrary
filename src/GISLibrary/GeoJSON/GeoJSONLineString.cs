using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON LineString coordinate object.
/// </summary>
public record GeoJSONLineString : GeoJSONCoordinates
{
    /// <summary>
    /// Initializes a new instance of the GeoJSONLineString class representing a geographic line string in GeoJSON
    /// format.
    /// </summary>
    /// <remarks>Use this constructor to create a GeoJSONLineString object before setting its coordinates or
    /// properties. This class is typically used to model paths or routes as defined by the GeoJSON
    /// specification.</remarks>
    public GeoJSONLineString() { }

    /// <summary>
    /// Initializes a new instance of the GeoJSONLineString class with the specified positions.
    /// </summary>
    /// <param name="positions">An enumerable collection of GeoJSONPosition objects that define the points of the line string. Must contain at
    /// least two positions.</param>
    /// <exception cref="ArgumentException">Thrown when the positions collection contains fewer than two elements.</exception>
    public GeoJSONLineString(IEnumerable<GeoJSONPosition> positions)
    {
        var posList = positions.ToList();
        if (posList.Count < 2)
        {
            throw new ArgumentException("A GeoJSON LineString must have at least two positions.");
        }
        Positions = posList;
    }

    /// <summary>
    /// Initializes a new instance of the GeoJSONLineString class from a collection of GeoJSONPoint objects.
    /// </summary>
    /// <remarks>Each point in the collection is converted to its corresponding geographic position. The
    /// collection must contain at least two points to form a valid line string.</remarks>
    /// <param name="points">The collection of GeoJSONPoint instances that define the vertices of the line string. The order of points
    /// determines the path of the line.</param>
    public GeoJSONLineString(IEnumerable<GeoJSONPoint> points)
        : this(points.Select(p => p.Position)) { }

    /// <summary>
    /// Gets or sets the positions that make up the line string.
    /// </summary>
    public List<GeoJSONPosition> Positions { get; set; } = [];

    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var position in Positions)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(position.Longitude);
            writer.WriteNumberValue(position.Latitude);
            if (position.Altitude.HasValue)
            {
                writer.WriteNumberValue(position.Altitude.Value);
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}
