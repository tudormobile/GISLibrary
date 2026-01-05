namespace Tudormobile.Kml;

/// <summary>
/// Base class for KML geometry types.
/// </summary>
public abstract record class KmlGeometry
{
    /// <summary>
    /// Gets the type of the geometry.
    /// </summary>
    /// <value>A <see cref="KmlGeometryType"/> value indicating the geometry's type.</value>
    public abstract KmlGeometryType GeometryType { get; }
}

/// <summary>
/// Represents a KML geometry type that is not supported by the current implementation.
/// </summary>
public record class KmlUnsupportedGeometry() : KmlGeometry
{
    /// <summary>
    /// Gets the type of this geometry.
    /// </summary>
    /// <value>Always returns <see cref="KmlGeometryType.Unsupported"/>.</value>
    public override KmlGeometryType GeometryType => KmlGeometryType.Unsupported;
}


