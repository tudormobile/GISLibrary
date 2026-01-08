namespace Tudormobile.Kml;

/// <summary>
/// Represents a KML document item.
/// </summary>
/// <param name="Id">The unique identifier of the document.</param>
/// <param name="Name">The name of the document.</param>
/// <param name="Description">The description of the document.</param>
public record class KmlDocumentItem(string Id, string Name, string Description)
    : KmlItem(Id, Name, Description)
{
    /// <summary>
    /// Gets the type of this KML item.
    /// </summary>
    /// <value>Always returns <see cref="KmlItemType.Document"/>.</value>
    public override KmlItemType ItemType => KmlItemType.Document;
}


