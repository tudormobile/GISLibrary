using System.Collections.Immutable;
using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a bounding box as defined by the GeoJSON specification.
/// </summary>
/// <remarks>A GeoJSON bounding box defines the spatial extent of a geometry, feature, or feature collection. It
/// is typically represented as an array of numbers specifying the minimum and maximum coordinates along each dimension
/// (e.g., [west, south, east, north] for 2D geometries). This class provides functionality for parsing and handling
/// bounding box data in GeoJSON structures.</remarks>
public class GeoJSONBoundingBox
{
    internal static ImmutableArray<double>? Parse(string json) => Parse(JsonElement.Parse(json));

    internal static ImmutableArray<double>? Parse(JsonElement bboxElement)
    {
        if (bboxElement.ValueKind == JsonValueKind.Array)
        {
            return bboxElement.EnumerateArray()
                .Where(item => item.ValueKind == JsonValueKind.Number && item.TryGetDouble(out _))
                .Select(item => item.GetDouble())
                .ToImmutableArray();
        }
        return null;
    }
}
