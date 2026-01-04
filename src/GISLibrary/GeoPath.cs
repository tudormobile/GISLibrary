namespace Tudormobile.GIS;

/// <summary>
/// Represents a sequence of geographic positions that defines a path on the Earth's surface.
/// </summary>
public class GeoPath : IReadOnlyCollection<GeoPosition>
{
    private readonly List<GeoPosition> _positions = [];

    /// <summary>
    /// Gets a value indicating whether the collection contains no elements.
    /// </summary>
    public bool IsEmpty => _positions.Count == 0;

    /// <summary>
    /// Gets a value indicating whether the collection contains any positions.
    /// </summary>
    public bool HasValues => _positions.Count > 0;

    /// <summary>
    /// Gets the number of positions in the path.
    /// </summary>
    public int Count => _positions.Count;

    /// <summary>
    /// Adds a position to the end of the path.
    /// </summary>
    public GeoPath Add(GeoPosition position) { _positions.Add(position); return this; }

    /// <summary>
    /// Adds multiple positions to the end of the path.
    /// </summary>
    public GeoPath AddRange(IEnumerable<GeoPosition> positions) { _positions.AddRange(positions); return this; }

    /// <summary>
    /// Adds a position to the end of the path.
    /// </summary>
    public GeoPath Add(GeoLocation location, double altitude = 0) { _positions.Add(new GeoPosition(location.Latitude, location.Longitude, altitude)); return this; }

    /// <summary>
    /// Adds multiple positions to the end of the path.
    /// </summary>
    public GeoPath AddRange(IEnumerable<GeoLocation> locations, double altitude = 0) { _positions.AddRange(locations.Select(l => new GeoPosition(l.Latitude, l.Longitude, altitude))); return this; }

    /// <summary>
    /// Removes all positions from the path.
    /// </summary>
    public GeoPath Clear() { _positions.Clear(); return this; }

    /// <summary>
    /// Gets the position at the specified index.
    /// </summary>
    public GeoPosition this[int index] => _positions[index];

    /// <inheritdoc/>
    public IEnumerator<GeoPosition> GetEnumerator() => _positions.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
