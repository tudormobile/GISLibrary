using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Base class for GPX entities that contain common metadata properties.
    /// </summary>
    public class GpxEntity : GpxDocumentElement
    {
        internal GpxEntity(XElement element) : base(element) { }

        /// <summary>
        /// Gets the name of the GPX entity.
        /// </summary>
        public string Name => _element.Element(_element.GetDefaultNamespace() + "name")?.Value ?? string.Empty;

        /// <summary>
        /// Gets the comment associated with the GPX entity.
        /// </summary>
        public string Comment => _element.Element(_element.GetDefaultNamespace() + "cmt")?.Value ?? string.Empty;

        /// <summary>
        /// Gets the description of the GPX entity.
        /// </summary>
        public string Description => _element.Element(_element.GetDefaultNamespace() + "desc")?.Value ?? string.Empty;

        /// <summary>
        /// Gets the source of the GPX data.
        /// </summary>
        public string Source => _element.Element(_element.GetDefaultNamespace() + "src")?.Value ?? string.Empty;

        /// <summary>
        /// Gets the symbol name for the GPX entity.
        /// </summary>
        public string SymbolName => _element.Element(_element.GetDefaultNamespace() + "sym")?.Value ?? string.Empty;

        /// <summary>
        /// Gets the classification type of the GPX entity.
        /// </summary>
        public string ClassificationType => _element.Element(_element.GetDefaultNamespace() + "type")?.Value ?? string.Empty;
    }
}
