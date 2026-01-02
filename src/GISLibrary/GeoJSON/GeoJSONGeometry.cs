using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a geometry element in GeoJSON and provides typed access to its coordinates.
/// </summary>
public class GeoJSONGeometry
{
    private readonly JsonElement _geometryElement;
    private GeoJSONDocument.GeoJSONGeometryType _geometryType;
    private GeoJSONCoordinates? _coordinates;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJSONGeometry"/> class from a JSON element.
    /// </summary>
    /// <param name="geometryElement">The JSON element representing the geometry.</param>
    /// <exception cref="ArgumentException">Thrown when the provided element is not a known Geometry.</exception>
    public GeoJSONGeometry(JsonElement geometryElement)
    {
        if (!geometryElement.TryGetProperty(GeoJSONDocument.TYPE_PROPERTY, out var typeProperty)
            || !Enum.TryParse<GeoJSONDocument.GeoJSONGeometryType>(typeProperty.GetString(), out var geometryType)
            )
        {
            throw new ArgumentException("The provided JSON element is not a known Geometry.");
        }
        _geometryType = geometryType;
        _geometryElement = geometryElement;
    }

    /// <summary>
    /// Gets the geometry type as a string.
    /// </summary>
    public string Type => _geometryType.ToString();

    /// <summary>
    /// Gets the coordinates for the geometry as a typed object.
    /// </summary>
    public GeoJSONCoordinates Coordinates => _coordinates ??= CreateCoordinates(_geometryType, GetCoordinatesElement());

    internal GeoJSONDocument.GeoJSONGeometryType TestGeometryType
    {
        get => _geometryType;
        set => _geometryType = value;
    }

    private JsonElement GetCoordinatesElement()
        => _geometryType == GeoJSONDocument.GeoJSONGeometryType.GeometryCollection
            ? _geometryElement.GetProperty("geometries")
            : _geometryElement.GetProperty("coordinates");
    private static GeoJSONCoordinates CreateCoordinates(GeoJSONDocument.GeoJSONGeometryType geometryType, JsonElement coordinatesElement)
    {
        return geometryType switch
        {
            GeoJSONDocument.GeoJSONGeometryType.Point => new GeoJSONPoint(PositionFromCoordinates(coordinatesElement)),
            GeoJSONDocument.GeoJSONGeometryType.MultiPoint => new GeoJSONMultiPoint
            {
                Points = [.. coordinatesElement.EnumerateArray().Select(coord => new GeoJSONPoint(PositionFromCoordinates(coord)))]
            },
            GeoJSONDocument.GeoJSONGeometryType.LineString => new GeoJSONLineString
            {
                Positions = [.. coordinatesElement.EnumerateArray().Select(coord => PositionFromCoordinates(coord))]
            },
            GeoJSONDocument.GeoJSONGeometryType.MultiLineString => new GeoJSONMultiLineString
            {
                LineStrings = [.. coordinatesElement.EnumerateArray().Select(line => new GeoJSONLineString
                    {
                        Positions = [.. line.EnumerateArray().Select(coord => PositionFromCoordinates(coord))]
                    })]
            },
            GeoJSONDocument.GeoJSONGeometryType.Polygon => new GeoJSONPolygon
            {
                Rings = [.. coordinatesElement.EnumerateArray().Select(ring => new GeoJSONLineString
                    {
                        Positions = [.. ring.EnumerateArray().Select(coord => PositionFromCoordinates(coord))]
                    })]
            },
            GeoJSONDocument.GeoJSONGeometryType.MultiPolygon => new GeoJSONMultiPolygon
            {
                Polygons = [.. coordinatesElement.EnumerateArray().Select(polygon => new GeoJSONPolygon
                    {
                        Rings = [.. polygon.EnumerateArray().Select(ring => new GeoJSONLineString
                        {
                            Positions = [.. ring.EnumerateArray().Select(coord => PositionFromCoordinates(coord))]
                        })]
                    })]
            },
            GeoJSONDocument.GeoJSONGeometryType.GeometryCollection => new GeoJSONGeometryCollection
            {
                Geometries = [.. coordinatesElement.EnumerateArray().Select(geom => new GeoJSONGeometry(geom).Coordinates)]
            },
            _ => throw new NotSupportedException($"Geometry type '{geometryType}' is not supported."),
        };
    }

    private static GeoJSONPosition PositionFromCoordinates(JsonElement coordinatesElement)
    => new()
    {
        Longitude = coordinatesElement[0].GetDouble(),
        Latitude = coordinatesElement[1].GetDouble(),
        Altitude = coordinatesElement.GetArrayLength() == 2
            ? null
            : coordinatesElement.GetArrayLength() == 3
                ? coordinatesElement[2].GetDouble()
                : throw new IndexOutOfRangeException("Invalid number of elements in Point coordinates array.")
    };
}
