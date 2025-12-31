namespace Tudormobile.GeoJSON;

internal class GeoJSONDocumentBuilder : GeoJSONBuilder, IGeoJSONDocumentBuilder
{
    private readonly List<Func<IGeoJSONFeatureBuilder, IGeoJSONFeatureBuilder>> _features = [];

    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.Properties => base._properties;

    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.Objects => base._objects;

    /// <inheritdoc/>
    public GeoJSONDocument Build()
    {
        var doc = new GeoJSONDocument();

        var featureBuilders = _features.Select(f => f(new GeoJSONFeatureBuilder()));
        // Add custom objects
        foreach (var (name, value) in _objects)
        {
            doc.AddObject(name, value);
        }
        // Add features to the document
        foreach (var featureBuilder in featureBuilders)
        {
            var feature = featureBuilder.Build();
            doc.FeatureCollection.Features.Add(feature.Builder!.Build());
        }
        // add properties
        foreach (var (name, value) in _properties)
        {
            doc.AddProperty(name, value);
        }
        return doc;
    }

    /// <inheritdoc/>
    public IGeoJSONDocumentBuilder AddFeature(Func<IGeoJSONFeatureBuilder, IGeoJSONFeatureBuilder> value)
    {
        _features.Add(value);
        return this;
    }

    /// <inheritdoc/>
    IGeoJSONDocumentBuilder IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.AddObject(string name, object value)
    {
        base.AddObject(name, value);
        return this;
    }

    /// <inheritdoc/>
    IGeoJSONDocumentBuilder IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.AddProperty(string name, object value)
    {
        base.AddProperty(name, value);
        return this;
    }
}

public interface IGeoJSONDocumentBuilder
    : IBuilder<GeoJSONDocument>,
      IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>
{
    IGeoJSONDocumentBuilder AddFeature(Func<IGeoJSONFeatureBuilder, IGeoJSONFeatureBuilder> value);
}

