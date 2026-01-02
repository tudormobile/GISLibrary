using System.Collections.Immutable;
using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON FeatureCollection element and provides access to its features.
/// </summary>
public class GeoJSONFeatureCollection
{
    private readonly JsonElement _rootElement;
    private List<GeoJSONFeature>? _features;
    private ImmutableArray<double>? _boundingBox;

    internal GeoJSONFeatureCollection() { _features = []; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJSONFeatureCollection"/> class from a JSON element.
    /// </summary>
    /// <param name="rootElement">The JSON element representing the FeatureCollection.</param>
    /// <exception cref="ArgumentException">Thrown when the provided element is not a FeatureCollection.</exception>
    public GeoJSONFeatureCollection(JsonElement rootElement)
    {
        if (rootElement.GetProperty(GeoJSONDocument.TYPE_PROPERTY).GetString() != GeoJSONDocument.FEATURE_COLLECTION_TYPE)
        {
            throw new ArgumentException("The provided JSON element is not a FeatureCollection.");
        }
        _rootElement = rootElement;
    }

    /// <summary>
    /// Gets the list of features in the collection.
    /// </summary>
    public IList<GeoJSONFeature> Features => _features ??= GetFeatures();

    /// <summary>
    /// Gets the bounding box coordinates for the associated object, if available.
    /// </summary>
    public ImmutableArray<double>? BoundingBox
    {
        get => _rootElement.ValueKind != JsonValueKind.Undefined && _rootElement.TryGetProperty(GeoJSONDocument.BBOX_PROPERTY, out var bboxElement)
        ? GeoJSONBoundingBox.Parse(bboxElement)
        : _boundingBox;
        internal set => _boundingBox = value;
    }

    private List<GeoJSONFeature> GetFeatures()
    {
        if (_rootElement.TryGetProperty(GeoJSONDocument.FEATURES_PROPERTY, out var featuresElement))
        {
            return [.. featuresElement.EnumerateArray().Select(x => new GeoJSONFeature(x))];
        }
        return [];
    }
}
