namespace Tudormobile.GeoJSON;

internal class GeoJSONFeatureBuilder : GeoJSONBuilder, IGeoJSONFeatureBuilder
{
    private GeoJSONCoordinates? _geometry;
    private List<GeoJSONCoordinates>? _geometries;

    public GeoJSONCoordinates? Geometry => _geometry;

    public IEnumerable<GeoJSONCoordinates>? Geometries => _geometries;

    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.Properties => base._properties;

    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.Objects => base._objects;

    /// <inheritdoc/>
    public GeoJSONFeature Build() => new GeoJSONFeature(this);

    /// <inheritdoc/>
    public IGeoJSONFeatureBuilder SetGeometry(GeoJSONCoordinates geometry)
    {
        _geometry = geometry;
        return this;
    }
    /// <inheritdoc/>
    public IGeoJSONFeatureBuilder SetGeometry(IEnumerable<GeoJSONCoordinates> geometries)
    {
        _geometries = [.. geometries];
        return this;
    }
    /// <inheritdoc/>
    IGeoJSONFeatureBuilder IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.AddObject(string name, object value)
    {
        base.AddObject(name, value);
        return this;
    }
    /// <inheritdoc/>
    IGeoJSONFeatureBuilder IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.AddProperty(string name, object value)
    {
        base.AddProperty(name, value);
        return this;
    }
}

public interface IBuilder<T>
{
    T Build();
}

public interface IGeoJSONObjectBuilder<T>
{
    T AddObject(string name, object value);
    T AddProperty(string name, object value);
    List<(string, object)> Properties { get; }
    List<(string, object)> Objects { get; }

}

public interface IGeoJSONFeatureBuilder
    : IBuilder<GeoJSONFeature>,
      IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>
{
    IGeoJSONFeatureBuilder SetGeometry(GeoJSONCoordinates geometry);
    IGeoJSONFeatureBuilder SetGeometry(IEnumerable<GeoJSONCoordinates> geometries);
    GeoJSONCoordinates? Geometry { get; }
    IEnumerable<GeoJSONCoordinates>? Geometries { get; }
}
