namespace Tudormobile.Kml;

/// <summary>
/// Base record class for KML items.
/// </summary>
/// <param name="Id">The unique identifier of the item.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">The description of the item.</param>
public abstract record class KmlItem(string Id, string Name, string Description) : IKmlItem
{
    /// <summary>
    /// Gets the type of the KML item.
    /// </summary>
    /// <value>A <see cref="KmlItemType"/> value indicating the item's type.</value>
    public abstract KmlItemType ItemType { get; }
}


