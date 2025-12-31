namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON Polygon coordinate object.
/// </summary>
public record GeoJSONPolygon : GeoJSONCoordinates
{
    /// <summary>
    /// Gets or sets the linear rings that make up the polygon.
    /// </summary>
    public List<GeoJSONLineString> Rings { get; set; } = [];
}
