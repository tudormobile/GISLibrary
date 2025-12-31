namespace Tudormobile.GeoJSON;

internal class GeoJSONBuilder
{
    protected readonly List<(string, object)> _properties = [];
    protected readonly List<(string, object)> _objects = [];

    public void AddObject(string name, object value)
    {
        _properties.Add((name, value));
    }
    /// <inheritdoc/>
    public void AddProperty(string name, object value)
    {
        _objects.Add((name, value));
    }

}
