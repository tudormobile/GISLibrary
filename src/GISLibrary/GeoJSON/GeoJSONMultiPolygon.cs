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
}
