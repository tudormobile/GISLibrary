using System.Xml.Linq;

namespace Tudormobile.Tcx;

/// <summary>
/// Represents a Training Center XML (TCX) document, providing access to workout and fitness data in the TCX format.
/// </summary>
/// <remarks>Use this class to read, manipulate, or generate TCX files, which are commonly used for exchanging
/// fitness activity data between devices and applications. Inherits functionality from the XmlDocumentBase class.</remarks>
public partial class TcxDocument : Gpx.GpxDocumentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TcxDocument"/> class.
    /// </summary>
    /// <param name="document">The XML document containing TCX data.</param>
    /// <exception cref="ArgumentNullException">Thrown when the document is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the document is invalid or missing required elements.</exception>
    /// <exception cref="NotSupportedException">Thrown when the TCX version is not supported.</exception>
    public TcxDocument(XDocument document) : base(document, "TrainingCenterDatabase") { }

    /// <summary>
    /// Gets the collection of activities contained in the TCX document.
    /// </summary>
    public IEnumerable<TcxActivity> Activities => _root.Element(_root.GetDefaultNamespace() + "Activities")?.Elements(_root.GetDefaultNamespace() + "Activity").Select(x => new TcxActivity(x)) ?? [];
}