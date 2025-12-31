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
}
