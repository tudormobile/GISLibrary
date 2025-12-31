namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON MultiPoint coordinate object.
/// </summary>
public record GeoJSONMultiPoint : GeoJSONCoordinates
{
    public GeoJSONMultiPoint()
    {

    }
    public GeoJSONMultiPoint(IEnumerable<GeoJSONPoint> points)
    {
        Points = points.ToList();
    }

    public GeoJSONMultiPoint(IEnumerable<GeoJSONPosition> positions)
    {
        Points = positions.Select(p => new GeoJSONPoint(p)).ToList();
    }

    /// <summary>
    /// Gets or sets the list of points.
    /// </summary>
    public List<GeoJSONPoint> Points { get; set; } = [];
}
