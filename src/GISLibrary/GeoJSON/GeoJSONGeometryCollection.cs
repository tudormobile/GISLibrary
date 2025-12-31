namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON GeometryCollection coordinate object.
/// </summary>
public record GeoJSONGeometryCollection : GeoJSONCoordinates
{
    private List<GeoJSONCoordinates> _geometries = [];
    /// <summary>
    /// Gets or sets the geometries contained in the collection.
    /// </summary>
    public List<GeoJSONCoordinates> Geometries
    {
        get { return _geometries; }
        internal set { _geometries = value; }
    }

    /// <summary>
    /// Adds a geometry to the collection.
    /// </summary>
    /// <remarks>This method enables method chaining by returning the same instance after adding the
    /// geometry.</remarks>
    /// <param name="geometryCoordinates">The geometry coordinates to add to the collection. Cannot be null.</param>
    /// <returns>The current <see cref="GeoJSONGeometryCollection"/> instance with the added geometry.</returns>
    public GeoJSONGeometryCollection AddGeometry(GeoJSONCoordinates geometryCoordinates)
    {
        _geometries.Add(geometryCoordinates);
        return this;
    }
}