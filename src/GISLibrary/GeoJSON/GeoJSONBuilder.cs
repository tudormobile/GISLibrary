namespace Tudormobile.GeoJSON;

internal class GeoJSONBuilder
{
    protected readonly List<(string, object)> _properties = [];
    protected readonly List<(string, object)> _objects = [];

    /// <summary>
    /// Adds an object with the specified name to the collection.
    /// </summary>
    /// <param name="name">The name associated with the object to add. Cannot be null.</param>
    /// <param name="value">The object to add to the collection. Can be null.</param>
    public void AddObject(string name, object value)
    {
        _objects.Add((name, value));
    }

    /// <summary>
    /// Adds a property with the specified name and value to the collection.
    /// </summary>
    /// <param name="name">The name of the property to add. Cannot be null or empty.</param>
    /// <param name="value">The value to associate with the property. Can be null.</param>
    public void AddProperty(string name, object value)
    {
        _properties.Add((name, value));
    }
}
