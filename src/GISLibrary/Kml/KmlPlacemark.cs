namespace Tudormobile.Kml;

/// <summary>
/// Represents a placemark in a KML document, which contains geographic data and optional metadata.
/// </summary>
public class KmlPlacemark : KmlDocumentItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KmlPlacemark"/> class.
    /// </summary>
    /// <param name="item">The underlying KML item.</param>
    public KmlPlacemark(IKmlItem item) : base(item) { }

    /// <summary>
    /// Gets the geometry associated with this placemark.
    /// </summary>
    /// <value>A <see cref="KmlGeometry"/> object representing the geographic shape or location.</value>
    public KmlGeometry Geometry => ((KmlPlacemarkItem)base._item).Geometry;
}
