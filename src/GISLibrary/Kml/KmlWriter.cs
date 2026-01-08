using System.Xml;

namespace Tudormobile.Kml;

/// <summary>
/// Provides functionality for writing KML (Keyhole Markup Language) documents.
/// </summary>
public class KmlWriter : IDisposable
{
    private readonly XmlWriter _xmlWriter;
    private bool _hasDocumentContainer = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="KmlWriter"/> class.
    /// </summary>
    /// <param name="xmlWriter">The underlying XML writer.</param>
    private KmlWriter(XmlWriter xmlWriter) { _xmlWriter = xmlWriter; }

    /// <summary>
    /// Releases all resources used by the current instance of the class.
    /// </summary>
    public void Dispose()
    {
        _xmlWriter.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates a new instance of <see cref="KmlWriter"/> that writes to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write the KML document to.</param>
    /// <returns>A new <see cref="KmlWriter"/> instance.</returns>
    public static KmlWriter Create(Stream stream) => new(XmlWriter.Create(stream, new XmlWriterSettings()
    {
        Indent = true,
        Async = true,
    }));

    /// <summary>
    /// Asynchronously writes the start of a KML document with optional document container information.
    /// </summary>
    /// <param name="id">The identifier for the KML document.</param>
    /// <param name="name">The name of the KML document.</param>
    /// <param name="description">The description of the KML document.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <remarks>
    /// If any of (id | name | description) are provided, a document element is created in addition to the root tag
    /// containing these values. In this case, the WriteEndKmlAsync() method will also close the document element 
    /// along with the kml root.
    /// </remarks>
    public async Task WriteStartKmlAsync(string? id = null, string? name = null, string? description = null)
    {
        _hasDocumentContainer = !string.IsNullOrWhiteSpace(string.Concat(id, name, description));
        await _xmlWriter.WriteStartDocumentAsync().ConfigureAwait(false);
        await _xmlWriter.WriteStartElementAsync(null, KmlReader.Root_Element_Name, KmlReader.KML_Document_Namespace).ConfigureAwait(false);
        if (_hasDocumentContainer)
        {
            await _xmlWriter.WriteStartElementAsync(null, KmlReader.Document_Element_Name, null).ConfigureAwait(false);
            await WriteIdNameDescriptionAsync(id ?? "", name ?? "", description ?? "").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously writes the id attribute, name element, and description element for a KML entity.
    /// </summary>
    /// <param name="id">The identifier to write as an attribute.</param>
    /// <param name="name">The name to write as an element.</param>
    /// <param name="description">The description to write as an element. If the description contains XML special characters, it will be wrapped in a CDATA section.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteIdNameDescriptionAsync(string id, string name, string description)
    {
        if (!string.IsNullOrWhiteSpace(id))
        {
            await _xmlWriter.WriteAttributeStringAsync(null, KmlReader.Id_Attribute_Name, null, id).ConfigureAwait(false);
        }
        if (!string.IsNullOrWhiteSpace(name))
        {
            await _xmlWriter.WriteElementStringAsync(null, KmlReader.Name_Element_Name, null, name).ConfigureAwait(false);
        }
        if (!string.IsNullOrWhiteSpace(description))
        {
            if (description.ContainsAny(['<', '>', '&', '\'', '"']))
            {
                await _xmlWriter.WriteStartElementAsync(null, KmlReader.Description_Element_Name, null).ConfigureAwait(false);
                await _xmlWriter.WriteCDataAsync(description).ConfigureAwait(false);
                await _xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            }
            else
            {
                await _xmlWriter.WriteElementStringAsync(null, KmlReader.Description_Element_Name, null, description).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Asynchronously writes the end of a KML document and flushes the writer.
    /// </summary>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <remarks>
    /// This method also flushes the output stream to finish off the document.
    /// </remarks>
    public async Task WriteEndKmlAsync()
    {
        if (_hasDocumentContainer)
        {
            await _xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
        }
        await _xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
        await _xmlWriter.WriteEndDocumentAsync().ConfigureAwait(false);
        await _xmlWriter.FlushAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously flushes any buffered data to the underlying storage or stream.
    /// </summary>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    /// <remarks>Explicitly flush the output stream.
    /// <para>
    /// </para>
    /// Calling this method is not normally required. This method is automatically called when using
    /// the WriteEndKmlAsync() method to finish off the document.
    /// </remarks>
    public async Task FlushAsync() => await _xmlWriter.FlushAsync().ConfigureAwait(false);

    /// <summary>
    /// Asynchronously writes a collection of KML folders to the document.
    /// </summary>
    /// <param name="folders">The collection of folders to write.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteFoldersAsync(IList<KmlFolder> folders, CancellationToken cancellationToken = default)
    {
        foreach (var folder in folders)
        {
            await _xmlWriter.WriteStartElementAsync(null, KmlReader.Folder_Element_Name, null).ConfigureAwait(false);
            await WriteIdNameDescriptionAsync(folder.Id, folder.Name, folder.Description).ConfigureAwait(false);
            await WritePlacemarksAsync(folder.Placemarks, cancellationToken).ConfigureAwait(false);
            await _xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    /// <summary>
    /// Asynchronously writes a collection of KML placemarks to the document.
    /// </summary>
    /// <param name="placemarks">The collection of placemarks to write.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WritePlacemarksAsync(IList<KmlPlacemark> placemarks, CancellationToken cancellationToken)
    {
        foreach (var placemark in placemarks)
        {
            await _xmlWriter.WriteStartElementAsync(null, KmlReader.Placemark_Element_Name, null).ConfigureAwait(false);
            await WriteIdNameDescriptionAsync(placemark.Id, placemark.Name, placemark.Description).ConfigureAwait(false);
            await _xmlWriter.WriteEndElementAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
