namespace Tudormobile.Kml;

/// <summary>
/// Represents a KML placemark item with associated geometry.
/// </summary>
/// <param name="Id">The unique identifier of the placemark.</param>
/// <param name="Name">The name of the placemark.</param>
/// <param name="Description">The description of the placemark.</param>
/// <param name="Geometry">The geometry associated with the placemark.</param>
public record class KmlPlacemarkItem(string Id, string Name, string Description, KmlGeometry Geometry)
    : KmlItem(Id, Name, Description)
{
    /// <summary>
    /// Gets the type of this KML item.
    /// </summary>
    /// <value>Always returns <see cref="KmlItemType.Placemark"/>.</value>
    public override KmlItemType ItemType => KmlItemType.Placemark;
}


