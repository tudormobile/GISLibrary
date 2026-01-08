namespace Tudormobile.Kml;

/// <summary>
/// Represents a KML folder item.
/// </summary>
/// <param name="Id">The unique identifier of the folder.</param>
/// <param name="Name">The name of the folder.</param>
/// <param name="Description">The description of the folder.</param>
public record class KmlFolderItem(string Id, string Name, string Description)
    : KmlItem(Id, Name, Description)
{
    /// <summary>
    /// Gets the type of this KML item.
    /// </summary>
    /// <value>Always returns <see cref="KmlItemType.Folder"/>.</value>
    public override KmlItemType ItemType => KmlItemType.Folder;
}


