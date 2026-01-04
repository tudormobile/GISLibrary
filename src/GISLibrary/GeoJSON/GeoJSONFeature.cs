using System.Collections.Immutable;
using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a single GeoJSON Feature.
/// </summary>
public class GeoJSONFeature
{
    private readonly JsonElement _featureElement;
    private readonly IGeoJSONFeatureBuilder? _builder;

    private IDictionary<string, JsonElement>? _properties;
    private IDictionary<string, object>? _foreignMembers;

    internal GeoJSONFeature(IGeoJSONFeatureBuilder builder) { _builder = builder; }
    internal IGeoJSONFeatureBuilder? Builder => _builder;

    /// <summary>
    /// Gets the bounding box that encloses the geometry represented by this feature, if available.
    /// </summary>
    /// <remarks>The bounding box is defined as an array of double values representing the minimum and maximum
    /// coordinates for each dimension, following the GeoJSON specification. If the feature does not specify a bounding
    /// box, this property may return null.</remarks>
    public ImmutableArray<double>? BoundingBox
    {
        get
        {
            if (_featureElement.ValueKind != JsonValueKind.Undefined &&
                _featureElement.TryGetProperty(GeoJSONDocument.BBOX_PROPERTY, out var bboxElement))
            {
                return GeoJSONBoundingBox.Parse(bboxElement);
            }
            return _builder?.BoundingBox;
        }
    }

    /// <summary>
    /// Gets the collection of JSON objects associated with their string keys.
    /// </summary>
    /// <remarks>The returned dictionary provides access to the underlying JSON elements by key. Modifications
    /// to the dictionary affect the set of available objects. The property never returns null.</remarks>
    public IDictionary<string, object> Objects => _foreignMembers ??= CreateObjectDictionary();

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJSONFeature"/> class from a JSON element.
    /// </summary>
    /// <param name="featureElement">The JSON element representing the feature.</param>
    /// <exception cref="ArgumentException">Thrown when the provided element is not a Feature.</exception>
    public GeoJSONFeature(JsonElement featureElement)
    {
        if (featureElement.GetProperty(GeoJSONDocument.TYPE_PROPERTY).GetString() != GeoJSONDocument.FEATURE_TYPE)
        {
            throw new ArgumentException("The provided JSON element is not a Feature.");
        }
        _featureElement = featureElement;
    }

    /// <summary>
    /// Gets the geometry element for the feature.
    /// </summary>
    public JsonElement Geometry => _featureElement.GetProperty(GeoJSONDocument.GEOMETRY_PROPERTY);

    /// <summary>
    /// Gets the properties dictionary for the feature.
    /// </summary>
    public IDictionary<string, JsonElement> Properties => _properties
        ??= _featureElement.GetProperty(GeoJSONDocument.PROPERTIES_PROPERTY).EnumerateObject()
            .ToDictionary(x => x.Name, x => x.Value);

    private Dictionary<string, object> CreateObjectDictionary()
    {
        var dict = new Dictionary<string, object>();
        foreach (var property in _featureElement.EnumerateObject())
        {
            if (property.Name != GeoJSONDocument.TYPE_PROPERTY
                && property.Name != GeoJSONDocument.PROPERTIES_PROPERTY
                && property.Name != GeoJSONDocument.GEOMETRY_PROPERTY
                && property.Name != GeoJSONDocument.BBOX_PROPERTY)
            {
                dict[property.Name] = property.Value;
            }
        }
        return dict;
    }
}
