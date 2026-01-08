namespace Tudormobile.Kml;

/// <summary>
/// Base class for KML document items providing common properties.
/// </summary>
public class KmlDocumentItemBase : IKmlItem
{
    /// <summary>
    /// The underlying KML item.
    /// </summary>
    protected IKmlItem _item;

    /// <summary>
    /// Initializes a new instance of the <see cref="KmlDocumentItemBase"/> class.
    /// </summary>
    /// <param name="item">The underlying KML item.</param>
    public KmlDocumentItemBase(IKmlItem item) { _item = item; }

    /// <summary>
    /// Gets the unique identifier of the KML item.
    /// </summary>
    /// <value>A string representing the item's ID.</value>
    public string Id => _item.Id;

    /// <summary>
    /// Gets the name of the KML item.
    /// </summary>
    /// <value>A string representing the item's name.</value>
    public string Name => _item.Name;

    /// <summary>
    /// Gets the description of the KML item.
    /// </summary>
    /// <value>A string representing the item's description.</value>
    public string Description => _item.Description;
}
