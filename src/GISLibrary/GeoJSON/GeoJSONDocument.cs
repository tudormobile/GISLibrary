using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a parsed GeoJSON document.
/// </summary>
public class GeoJSONDocument
{
    internal static readonly string TYPE_PROPERTY = "type";
    internal static readonly string FEATURES_PROPERTY = "features";
    internal static readonly string GEOMETRY_PROPERTY = "geometry";
    internal static readonly string PROPERTIES_PROPERTY = "properties";
    internal static readonly string FEATURE_COLLECTION_TYPE = "FeatureCollection";
    internal static readonly string FEATURE_TYPE = "Feature";
    internal static readonly string BBOX_PROPERTY = "bbox";

    /// <summary>
    /// Geometry types defined by the GeoJSON specification used by this library.
    /// </summary>
    internal enum GeoJSONGeometryType
    {
        Point,
        MultiPoint,
        LineString,
        MultiLineString,
        Polygon,
        MultiPolygon,
        GeometryCollection
    }

    private readonly JsonDocument? _jsonDocument;
    private GeoJSONFeatureCollection? _featureCollection;
    private IDictionary<string, JsonElement>? _properties;
    private IDictionary<string, object>? _propertyObjects;
    private List<(string, object)>? _objects;
    private IDictionary<string, object>? _arbitraryObjects;

    /// <summary>
    /// Creates a new instance of the IGeoJSONDocumentBuilder for constructing GeoJSON documents.
    /// </summary>
    /// <returns>A new GeoJSONDocumentBuilder instance that can be used to build GeoJSON documents.</returns>
    public static IGeoJSONDocumentBuilder Create() => new GeoJSONDocumentBuilder();

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJSONDocument"/> class from a <see cref="JsonDocument"/>.
    /// </summary>
    /// <param name="jsonDocument">The parsed JSON document representing GeoJSON.</param>
    internal GeoJSONDocument(JsonDocument jsonDocument)
    {
        _jsonDocument = jsonDocument;
    }
    internal void AddObject(string name, object value) => (_objects ??= []).Add((name, value));
    internal void AddProperty(string name, object value) => (_propertyObjects ??= new Dictionary<string, object>())[name] = value;

    internal GeoJSONDocument()
    {
        _jsonDocument = null;
        _featureCollection = new GeoJSONFeatureCollection();
    }

    /// <summary>
    /// Gets the collection of JSON objects associated with their string keys.
    /// </summary>
    /// <remarks>The returned dictionary provides access to the underlying JSON elements by key. Modifications
    /// to the dictionary affect the set of available objects. The property never returns null.</remarks>
    public IDictionary<string, object> Objects => _arbitraryObjects ??= CreateObjectDictionary();

    /// <summary>
    /// Gets the properties dictionary for the document.
    /// </summary>
    public IDictionary<string, JsonElement> Properties => _properties ??= CreatePropertiesDictionary();

    private Dictionary<string, JsonElement> CreatePropertiesDictionary()
    {
        if (_jsonDocument is not null
         && _jsonDocument.RootElement.TryGetProperty(GeoJSONDocument.PROPERTIES_PROPERTY, out var propertiesElement))
        {
            return propertiesElement.EnumerateObject().ToDictionary(x => x.Name, x => x.Value);
        }
        return _propertyObjects?.ToDictionary(x => x.Key, x => JsonSerializer.SerializeToElement(x.Value)) ?? [];
    }

    private Dictionary<string, object> CreateObjectDictionary()
    {
        if (_jsonDocument is not null)
        {
            var dict = new Dictionary<string, object>();
            foreach (var property in _jsonDocument.RootElement.EnumerateObject())
            {
                if (property.Name != TYPE_PROPERTY && property.Name != FEATURES_PROPERTY && property.Name != PROPERTIES_PROPERTY)
                {
                    dict[property.Name] = property.Value;
                }
            }
            return dict;
        }
        return _objects?.ToDictionary(x => x.Item1, x => x.Item2) ?? [];
    }


    /// <summary>
    /// Loads a GeoJSON document from a file asynchronously.
    /// </summary>
    /// <param name="path">The path to the GeoJSON file.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous load operation. The task result contains the <see cref="GeoJSONDocument"/>.</returns>
    public static async Task<GeoJSONDocument> LoadFromFileAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return await ParseAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Parses a GeoJSON document from a UTF-8 JSON stream asynchronously.
    /// </summary>
    /// <param name="utf8Json">The stream containing UTF-8 JSON data.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous parse operation. The task result contains the <see cref="GeoJSONDocument"/>.</returns>
    public static async Task<GeoJSONDocument> ParseAsync(Stream utf8Json, CancellationToken cancellationToken = default)
    {
        var result = await JsonDocument.ParseAsync(utf8Json, cancellationToken: cancellationToken).ConfigureAwait(false);
        return new GeoJSONDocument(result);
    }

    /// <summary>
    /// Asynchronously saves the current object to a file in JSON format at the specified path.
    /// </summary>
    /// <remarks>The resulting file will contain a formatted (indented) JSON representation of the object. The
    /// method overwrites any existing file at the specified path.</remarks>
    /// <param name="path">The file system path where the JSON data will be written. If the file exists, it will be overwritten.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveToFileAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(path);
        using var writer = new Utf8JsonWriter(stream, options: new JsonWriterOptions() { Indented = true });
        await WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously writes the current object to the specified stream in JSON format.
    /// </summary>
    /// <remarks>The output JSON is formatted with indentation for readability. The caller is responsible for
    /// managing the lifetime of the provided stream.</remarks>
    /// <param name="stream">The stream to which the object will be serialized. Must be writable.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var writer = new Utf8JsonWriter(stream, options: new JsonWriterOptions() { Indented = true });
        await WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the GeoJSON document to a file asynchronously.
    /// </summary>
    /// <param name="utf8JsonWriter">Writer to use.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task WriteToAsync(Utf8JsonWriter utf8JsonWriter, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            if (_jsonDocument == null)
            {
                WriteTo(utf8JsonWriter);
            }
            else
            {
                _jsonDocument.WriteTo(utf8JsonWriter);
            }
            utf8JsonWriter.Flush();
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the feature collection contained in the document.
    /// </summary>
    public GeoJSONFeatureCollection FeatureCollection => _featureCollection ??= new GeoJSONFeatureCollection(_jsonDocument!.RootElement);

    private void WriteTo(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        // Write type
        writer.WriteString(TYPE_PROPERTY, FEATURE_COLLECTION_TYPE);
        // Write custom objects
        if (_objects != null)
        {
            foreach (var (name, value) in _objects)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
        }
        // Write features
        writer.WritePropertyName(FEATURES_PROPERTY);
        writer.WriteStartArray();
        foreach (var feature in FeatureCollection.Features.Where(f => f is not null))
        {
            WriteFeatureTo(writer, feature.Builder!.Build());
        }
        writer.WriteEndArray();
        // Write properties
        if (_propertyObjects != null)
        {
            writer.WritePropertyName(PROPERTIES_PROPERTY);
            writer.WriteStartObject();
            foreach (var (name, value) in _propertyObjects)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    private static void WriteFeatureTo(Utf8JsonWriter writer, GeoJSONFeature feature)
    {
        writer.WriteStartObject();
        writer.WriteString(TYPE_PROPERTY, FEATURE_TYPE);

        // Custom objects first
        if (feature.Builder!.Objects != null)
        {
            foreach (var (name, value) in feature.Builder!.Objects)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
        }
        // Then the geometry
        if (feature.Builder.Geometry != null || feature.Builder.Geometries != null)
        {
            writer.WritePropertyName(GEOMETRY_PROPERTY);
            writer.WriteStartObject();

            if (feature.Builder.Geometry != null)
            {
                WriteGeometryTo(writer, feature.Builder.Geometry);
            }
            else if (feature.Builder.Geometries != null)
            {
                writer.WriteString(TYPE_PROPERTY, "GeometryCollection");
                writer.WritePropertyName("geometries");
                writer.WriteStartArray();
                foreach (var geometry in feature.Builder.Geometries)
                {
                    writer.WriteStartObject();
                    WriteGeometryTo(writer, geometry);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        // Finally, the standard feature properties
        if (feature.Builder.Properties != null)
        {
            writer.WritePropertyName(PROPERTIES_PROPERTY);
            writer.WriteStartObject();
            foreach (var (name, value) in feature.Builder.Properties)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
            writer.WriteEndObject();
        }
        writer.WriteEndObject();
    }

    private static void WriteGeometryTo(Utf8JsonWriter writer, GeoJSONCoordinates? geometry)
    {
        writer.WriteString(TYPE_PROPERTY, TypeFromCoordinates(geometry));
        if (geometry is not null)
        {
            writer.WritePropertyName("coordinates");
            geometry!.WriteCoordinatesTo(writer);   // Non-null as per TypeFromCoordinates
        }
    }

    private static string TypeFromCoordinates(GeoJSONCoordinates? geometry)
    {
        return geometry switch
        {
            GeoJSONPoint => "Point",
            GeoJSONMultiPoint => "MultiPoint",
            GeoJSONLineString => "LineString",
            GeoJSONMultiLineString => "MultiLineString",
            GeoJSONPolygon => "Polygon",
            GeoJSONMultiPolygon => "MultiPolygon",
            _ => throw new NotImplementedException(),
        };
    }
}