using System.Collections.Immutable;
using Tudormobile.GIS;

namespace Tudormobile.GeoJSON;

/// <inheritdoc/>
internal class GeoJSONDocumentBuilder : GeoJSONBuilder, IGeoJSONDocumentBuilder
{
    private readonly List<Func<IGeoJSONFeatureBuilder, IGeoJSONFeatureBuilder>> _features = [];
    private ImmutableArray<double>? _boundingBox;

    /// <inheritdoc/>
    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.Properties => base._properties;

    /// <inheritdoc/>
    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>.Objects => base._objects;

    /// <inheritdoc/>
    public GeoJSONDocument Build()
    {
        var doc = new GeoJSONDocument();

        var featureBuilders = _features.Select(f => f(new GeoJSONFeatureBuilder()));
        // Add custom objects
        foreach (var (name, value) in ((IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>)this).Objects)
        {
            doc.AddObject(name, value);
        }
        // Add features to the document
        foreach (var featureBuilder in featureBuilders)
        {
            var feature = featureBuilder.Build();
            doc.FeatureCollection.Features.Add(feature.Builder!.Build());
        }
        doc.FeatureCollection.BoundingBox = _boundingBox;
        // add properties
        foreach (var (name, value) in ((IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>)this).Properties)
        {
            doc.AddProperty(name, value);
        }
        return doc;
    }

    /// <inheritdoc/>
    public IGeoJSONDocumentBuilder SetBoundingBox(IEnumerable<double> values)
    {
        _boundingBox = values.ToImmutableArray();
        return this;
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

/// <summary>
/// Defines a builder interface for constructing GeoJSON documents with features and geometry objects.
/// </summary>
/// <remarks>This interface extends both generic and GeoJSON-specific builder interfaces, enabling fluent
/// composition of GeoJSON documents. Implementations typically allow chaining of feature and geometry additions to
/// incrementally build a complete GeoJSON structure.</remarks>
public interface IGeoJSONDocumentBuilder
    : IBuilder<GeoJSONDocument>,
      IGeoJSONObjectBuilder<IGeoJSONDocumentBuilder>
{
    /// <summary>
    /// Adds a new feature to the GeoJSON document using the specified builder function.
    /// </summary>
    /// <remarks>The provided builder function is invoked to configure the feature before it is added to the
    /// document. This method supports fluent chaining for building complex GeoJSON documents.</remarks>
    /// <param name="value">A delegate that receives an <see cref="IGeoJSONFeatureBuilder"/> and returns a configured feature builder
    /// representing the feature to add. Cannot be null.</param>
    /// <returns>The current <see cref="IGeoJSONDocumentBuilder"/> instance, enabling method chaining.</returns>
    IGeoJSONDocumentBuilder AddFeature(Func<IGeoJSONFeatureBuilder, IGeoJSONFeatureBuilder> value);

    /// <summary>
    /// Sets a bounding box to the GeoJSON document using the specified coordinate values.
    /// </summary>
    /// <remarks>
    /// The number and order of values in <paramref name="values"/> must match the dimensionality of
    /// the geometry being described. Supplying an incorrect number of coordinates may result in an invalid GeoJSON
    /// document. A bounding box is normally set on geometry elements, but when set on the FeatureCollection level,
    /// it applies to all features within the collection.
    /// </remarks>
    /// <param name="values">A sequence of double values representing the bounding box coordinates. The values should be provided in the
    /// order required by the GeoJSON specification (e.g., [west, south, east, north] for 2D, or [west, south, minZ,
    /// east, north, maxZ] for 3D).</param>
    /// <returns>The current instance of the <see cref="IGeoJSONDocumentBuilder"/>, enabling method chaining.</returns>
    IGeoJSONDocumentBuilder SetBoundingBox(IEnumerable<double> values);

}

