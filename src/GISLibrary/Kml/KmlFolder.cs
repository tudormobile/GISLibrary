namespace Tudormobile.Kml;

/// <summary>
/// Represents a folder within a KML document that can contain placemarks.
/// </summary>
public class KmlFolder : KmlDocumentItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KmlFolder"/> class.
    /// </summary>
    /// <param name="item">The underlying KML item.</param>
    public KmlFolder(KmlFolderItem item) : base(item) { }

    /// <summary>
    /// Gets or initializes the collection of placemarks contained in this folder.
    /// </summary>
    /// <value>A list of <see cref="KmlPlacemark"/> objects.</value>
    public List<KmlPlacemark> Placemarks { get; } = [];
}
