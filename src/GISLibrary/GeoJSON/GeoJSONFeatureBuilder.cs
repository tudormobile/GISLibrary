using System.Collections.Immutable;
using Tudormobile.GIS;

namespace Tudormobile.GeoJSON;

/// <inheritdoc/>
internal class GeoJSONFeatureBuilder : GeoJSONBuilder, IGeoJSONFeatureBuilder
{
    private GeoJSONCoordinates? _geometry;
    private List<GeoJSONCoordinates>? _geometries;
    private ImmutableArray<double>? _boundingBox;

    /// <inheritdoc/>
    public GeoJSONCoordinates? Geometry => _geometry;

    /// <inheritdoc/>
    public IEnumerable<GeoJSONCoordinates>? Geometries => _geometries;

    /// <inheritdoc/>
    public ImmutableArray<double>? BoundingBox => _boundingBox;

    /// <inheritdoc/>
    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.Properties => base._properties;

    /// <inheritdoc/>
    List<(string, object)> IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.Objects => base._objects;

    /// <inheritdoc/>
    public GeoJSONFeature Build() => new(this);

    /// <inheritdoc/>
    public IGeoJSONFeatureBuilder SetGeometry(GeoJSONCoordinates geometry)
    {
        _geometry = geometry;
        return this;
    }

    /// <inheritdoc/>
    public IGeoJSONFeatureBuilder SetGeometry(IEnumerable<GeoJSONCoordinates> geometries)
    {
        _geometries = [.. geometries];
        return this;
    }

    /// <inheritdoc/>
    public IGeoJSONFeatureBuilder SetBoundingBox(IEnumerable<double> values)
    {
        _boundingBox = values.ToImmutableArray();
        return this;
    }

    /// <inheritdoc/>
    IGeoJSONFeatureBuilder IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.AddObject(string name, object value)
    {
        base.AddObject(name, value);
        return this;
    }

    /// <inheritdoc/>
    IGeoJSONFeatureBuilder IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>.AddProperty(string name, object value)
    {
        base.AddProperty(name, value);
        return this;
    }
}

/// <summary>
/// Defines a builder interface for constructing GeoJSON objects and their associated properties in a type-safe manner.
/// </summary>
/// <remarks>Implementations of this interface allow incremental construction of GeoJSON objects by adding named
/// objects and properties. This interface is intended for use in scenarios where GeoJSON structures need to be built
/// programmatically, such as serialization or dynamic object creation.</remarks>
/// <typeparam name="T">The type returned by builder methods, typically the implementing builder type to support fluent chaining.</typeparam>
public interface IGeoJSONObjectBuilder<T>
{
    /// <summary>
    /// Adds a new object with the specified name and value to the collection and returns the resulting object.
    /// </summary>
    /// <param name="name">The name to associate with the object. Cannot be null or empty.</param>
    /// <param name="value">The value of the object to add. May be null depending on the collection's requirements.</param>
    /// <returns>The object that was added to the collection.</returns>
    T AddObject(string name, object value);

    /// <summary>
    /// Adds a property with the specified name and value to the current object and returns the updated instance.
    /// </summary>
    /// <param name="name">The name of the property to add. Cannot be null or empty.</param>
    /// <param name="value">The value to assign to the property. Can be null.</param>
    /// <returns>The updated instance of type T with the new property added.</returns>
    T AddProperty(string name, object value);

    /// <summary>
    /// Gets a list of key-value pairs representing the properties associated with the current instance.
    /// </summary>
    /// <remarks>Each tuple in the list contains a property name and its corresponding value. The list is
    /// read-only and reflects the current state of the instance's properties.</remarks>
    List<(string, object)> Properties { get; }

    /// <summary>
    /// Gets the collection of object pairs, where each pair consists of a string key and its associated object value.
    /// </summary>
    /// <remarks>The order of items in the list is preserved. The property is read-only; to modify the
    /// collection, use the appropriate methods provided by the containing class.</remarks>
    List<(string, object)> Objects { get; }
}

/// <summary>
/// Defines methods and properties for building a GeoJSON feature, allowing configuration of geometry and related data.
/// </summary>
/// <remarks>Implementations of this interface enable the construction of GeoJSON features by specifying geometry
/// as either a single coordinate set or a collection of coordinate sets. This interface is typically used in scenarios
/// where GeoJSON features need to be programmatically assembled for serialization or transmission. Members inherited
/// from IBuilder and IGeoJSONObjectBuilder provide additional configuration options for the resulting feature
/// object.</remarks>
public interface IGeoJSONFeatureBuilder
    : IBuilder<GeoJSONFeature>,
      IGeoJSONObjectBuilder<IGeoJSONFeatureBuilder>
{
    /// <summary>
    /// Sets the geometry for the GeoJSON feature being built.
    /// </summary>
    /// <param name="geometry">The coordinates representing the geometry to associate with the feature. Cannot be null.</param>
    /// <returns>The current instance of <see cref="IGeoJSONFeatureBuilder"/> to allow method chaining.</returns>
    IGeoJSONFeatureBuilder SetGeometry(GeoJSONCoordinates geometry);

    /// <summary>
    /// Sets the geometry for the feature using the specified collection of coordinates.
    /// </summary>
    /// <param name="geometries">An enumerable collection of coordinates that defines the geometry of the feature. Cannot be null or contain null
    /// elements.</param>
    /// <returns>The current instance of <see cref="IGeoJSONFeatureBuilder"/> to allow method chaining.</returns>
    IGeoJSONFeatureBuilder SetGeometry(IEnumerable<GeoJSONCoordinates> geometries);

    /// <summary>
    /// Gets the geometric coordinates representing the spatial location or shape of the object, if available.
    /// </summary>
    /// <remarks>The value is <see langword="null"/> if the object does not have associated geometry. The
    /// coordinates are provided in GeoJSON format, which is commonly used for representing geographic data in web
    /// applications and APIs.</remarks>
    GeoJSONCoordinates? Geometry { get; }

    /// <summary>
    /// Gets the collection of coordinate geometries represented by this instance.
    /// </summary>
    IEnumerable<GeoJSONCoordinates>? Geometries { get; }

    /// <summary>
    /// Sets a bounding box to the GeoJSON geometry.
    /// </summary>
    /// <remarks>The number and order of values in <paramref name="values"/> must match the dimensionality of
    /// the geometry being described. Supplying an incorrect number of coordinates may result in an invalid GeoJSON
    /// document.</remarks>
    /// <param name="values">A sequence of double values representing the bounding box coordinates. The values should be provided in the
    /// order required by the GeoJSON specification (e.g., [west, south, east, north] for 2D, or [west, south, minZ,
    /// east, north, maxZ] for 3D).</param>
    /// <returns>The current instance of the <see cref="IGeoJSONFeatureBuilder"/>, enabling method chaining.</returns>
    IGeoJSONFeatureBuilder SetBoundingBox(IEnumerable<double> values);

    /// <summary>
    /// Gets the bounding box that defines the spatial extent of the object, if available.
    /// </summary>
    /// <remarks>The bounding box is typically represented as an array of coordinates specifying the minimum
    /// and maximum extents along each dimension. If the bounding box is not defined, the property returns
    /// null.</remarks>
    ImmutableArray<double>? BoundingBox { get; }
}
