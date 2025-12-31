using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON MultiPolygon coordinate object.
/// </summary>
public record GeoJSONMultiPolygon : GeoJSONCoordinates
{
    /// <summary>
    /// Gets or sets the polygons in the multi-polygon.
    /// </summary>
    public List<GeoJSONPolygon> Polygons { get; set; } = [];

    internal override void WriteCoordinatesTo(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var polygon in Polygons)
        {
            polygon.WriteCoordinatesTo(writer);
        }
        writer.WriteEndArray();
    }
}
