using System.Text.Json;

namespace Tudormobile.GeoJSON;

/// <summary>
/// Base class for typed coordinate representations.
/// </summary>
public abstract record class GeoJSONCoordinates
{
    internal abstract void WriteCoordinatesTo(Utf8JsonWriter writer);
}
