using Tudormobile.GeoJSON;

namespace Tudormobile.GIS.GeoJSON;

/// <summary>
/// Provides extension methods for working with GeoJSON data types.
/// </summary>
public static class GeoJSONExtensions
{
    /// <summary>
    /// Converts the geometry of the specified GeoJSON feature to a GeoJSONGeometry instance, if possible.
    /// </summary>
    /// <param name="feature">The GeoJSONFeature whose geometry is to be converted. Cannot be null.</param>
    /// <returns>A GeoJSONGeometry instance representing the feature's geometry if it is a valid object; otherwise, null.</returns>
    public static GeoJSONGeometry? AsGeometry(this GeoJSONFeature feature)
    {
        return feature.Geometry.ValueKind == System.Text.Json.JsonValueKind.Object
            ? new GeoJSONGeometry(feature.Geometry)
            : null;
    }
}
