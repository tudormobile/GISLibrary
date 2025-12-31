using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON MultiLineString coordinate object.
/// </summary>
public record GeoJSONMultiLineString : GeoJSONCoordinates
{
    /// <summary>
    /// Gets or sets the collection of line strings.
    /// </summary>
    public List<GeoJSONLineString> LineStrings { get; set; } = [];

    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var lineString in LineStrings)
        {
            lineString.WriteCoordinatesTo(writer);
        }
        writer.WriteEndArray();
    }
}
