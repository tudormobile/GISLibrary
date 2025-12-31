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
    private List<(string, object)>? _properties;
    private List<(string, object)>? _objects;


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
    internal void AddProperty(string name, object value) => (_properties ??= []).Add((name, value));

    internal GeoJSONDocument()
    {
        _jsonDocument = null;
        _featureCollection = new GeoJSONFeatureCollection();
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
        return await ParseAsync(stream, cancellationToken);
    }

    /// <summary>
    /// Parses a GeoJSON document from a UTF-8 JSON stream asynchronously.
    /// </summary>
    /// <param name="utf8Json">The stream containing UTF-8 JSON data.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous parse operation. The task result contains the <see cref="GeoJSONDocument"/>.</returns>
    public static async Task<GeoJSONDocument> ParseAsync(Stream utf8Json, CancellationToken cancellationToken = default)
    {
        var result = await JsonDocument.ParseAsync(utf8Json, cancellationToken: cancellationToken);
        utf8Json.Close();
        utf8Json.Dispose();
        return new GeoJSONDocument(result);
    }

    /// <summary>
    /// Saves the GeoJSON document to a file asynchronously.
    /// </summary>
    /// <param name="path">The path to save the file to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveToFileAsync(string path, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            using var stream = File.Create(path);
            using var writer = new Utf8JsonWriter(stream, options: new JsonWriterOptions() { Indented = true });
            if (_jsonDocument == null)
            {
                WriteTo(writer);
            }
            else
            {
                _jsonDocument.WriteTo(writer);
            }
            writer.Flush();
        }, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the feature collection contained in the document.
    /// </summary>
    public GeoJSONFeatureCollection FeatureCollection => _featureCollection ??= new GeoJSONFeatureCollection(_jsonDocument!.RootElement);

    private void WriteTo(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        // Write custom objects
        if (_objects != null)
        {
            foreach (var (name, value) in _objects)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
        }
        // Write type
        writer.WriteString(TYPE_PROPERTY, FEATURE_COLLECTION_TYPE);
        // Write features
        writer.WritePropertyName(FEATURES_PROPERTY);
        writer.WriteStartArray();
        foreach (var feature in FeatureCollection.Features)
        {
            WriteFeatureTo(writer, feature.Builder?.Build());
        }
        writer.WriteEndArray();
        // Write properties
        if (_properties != null)
        {
            writer.WritePropertyName(PROPERTIES_PROPERTY);
            writer.WriteStartObject();
            foreach (var (name, value) in _properties)
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, value);
            }
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    private void WriteFeatureTo(Utf8JsonWriter writer, GeoJSONFeature? feature)
    {
        if (feature is not null && feature.Builder is not null)
        {
            writer.WriteStartObject();
            writer.WriteString(TYPE_PROPERTY, FEATURE_TYPE);

            // Custom objects first
            if (feature.Builder.Objects != null)
            {
                foreach (var (name, value) in feature.Builder.Objects)
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
                        WriteGeometryTo(writer, feature.Builder.Geometry);
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
        }
    }

    private void WriteGeometryTo(Utf8JsonWriter writer, GeoJSONCoordinates? geometry)
    {
        writer.WriteStartObject();
        writer.WriteString(TYPE_PROPERTY, TypeFromCoordinates(geometry));
        if (geometry is not null)
        {
            writer.WritePropertyName("coordinates");
            writer.WriteStartArray(); // TODO: Handle geometries
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteNull("coordinates");
        }
    }

    private string TypeFromCoordinates(GeoJSONCoordinates? geometry)
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