using System.Xml.Linq;

namespace Tudormobile.Gpx;

/// <summary>
/// Provides a base class for XML document wrappers, offering common save operations for derived XML document types.
/// </summary>
public abstract partial class GpxDocumentBase
{
    /// <summary>
    /// The underlying <see cref="XDocument"/> instance representing the XML document.
    /// </summary>
    protected readonly XDocument _document;

    /// <summary>
    /// Represents the root XML element associated with the current instance.
    /// </summary>
    /// <remarks>This field provides direct access to the underlying root <see cref="XElement"/> for derived
    /// classes. It is intended for use within subclasses that require manipulation or inspection of the XML
    /// structure.</remarks>
    protected readonly XElement _root;

    /// <summary>
    /// Represents the default XML namespace used by the containing class or its derived types.
    /// </summary>
    /// <remarks>This field is intended for use by derived classes to provide a consistent namespace context
    /// for XML operations. The value should be assigned during construction and remain unchanged throughout the
    /// lifetime of the object.</remarks>
    protected readonly XNamespace _defaultNamespace;

    /// <summary>
    /// Initializes a new instance of the <see cref="GpxDocumentBase"/> class with the specified <see cref="XDocument"/>.
    /// </summary>
    /// <param name="document">The <see cref="XDocument"/> to wrap. Cannot be null.</param>
    /// <param name="rootName">The name of the root element.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="document"/> is null.</exception>
    protected GpxDocumentBase(XDocument document, string rootName)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document), "The provided XDocument is null.");
        _root = _document.Root ?? throw new ArgumentException("The provided XDocument does not have a root element.");
        if (_document.Root.Name.LocalName != rootName)
        {
            throw new ArgumentException("The provided XDocument does not have the expected root element.");
        }
        // Use the actual namespace of the root element instead of GetDefaultNamespace()
        _defaultNamespace = _root.Name.Namespace;
    }

    /// <summary>
    /// Saves the current document to the specified file using the provided save options.
    /// </summary>
    /// <param name="fileName">The path and name of the file to which the document will be saved. Cannot be null or empty.</param>
    /// <param name="options">The options that control how the document is saved. If not specified, <see cref="SaveOptions.None"/> is used.</param>
    public void Save(string fileName, SaveOptions options = SaveOptions.None) => _document.Save(fileName, options);

    /// <summary>
    /// Saves the current document to the specified stream using the provided save options.
    /// </summary>
    /// <param name="stream">The stream to which the document will be written. The stream must be writable and remain open for the duration
    /// of the operation.</param>
    /// <param name="options">The options that control how the document is saved. If not specified, <see cref="SaveOptions.None"/> is used.</param>
    public void Save(Stream stream, SaveOptions options = SaveOptions.None) => _document.Save(stream, options);

    /// <summary>
    /// Asynchronously saves the document to the specified stream using the provided save options.
    /// </summary>
    /// <param name="stream">The stream to which the document will be saved. Must be writable and remain open for the duration of the
    /// operation.</param>
    /// <param name="options">Options that control how the document is saved. The default is <see cref="SaveOptions.None"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(Stream stream, SaveOptions options = SaveOptions.None, CancellationToken cancellationToken = default) => await _document.SaveAsync(stream, options, cancellationToken);

}
