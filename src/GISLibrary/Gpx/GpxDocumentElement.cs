using System.Xml.Linq;

namespace Tudormobile.Gpx;

public abstract partial class GpxDocumentBase
{
    /// <summary>
    /// Base class for GPX/TCX elements that wrap XML elements.
    /// </summary>
    public class GpxDocumentElement
    {
        /// <summary>
        /// The underlying XML element wrapped by this document element instance.
        /// </summary>
        protected XElement _element;

        internal GpxDocumentElement(XElement element) { _element = element ?? throw new ArgumentNullException(nameof(element)); }

        /// <summary>
        /// Gets the underlying <see cref="XElement"/> wrapped by this instance.
        /// </summary>
        /// <returns>The underlying <see cref="XElement"/>.</returns>
        public XElement AsElement() => _element;

        /// <summary>
        /// Explicitly converts an <see cref="GpxDocumentElement"/> to its underlying <see cref="XElement"/>.
        /// </summary>
        /// <param name="xmlElement">The <see cref="GpxDocumentElement"/> to convert.</param>
        /// <returns>The underlying <see cref="XElement"/>.</returns>
        public static explicit operator XElement(GpxDocumentElement xmlElement) => xmlElement._element;
    }
}
