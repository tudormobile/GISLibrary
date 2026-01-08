using System.Text;
using System.Xml.Linq;

namespace Tudormobile.Kml;

/// <summary>
/// Represents a KML document containing folders and placemarks.
/// </summary>
public class KmlDocument : KmlDocumentItemBase
{
    internal KmlDocument(IKmlItem item) : base(item) { }

    /// <summary>
    /// Gets the collection of folders contained in the document.
    /// </summary>
    /// <value>A list of <see cref="KmlFolder"/> objects.</value>
    public IList<KmlFolder> Folders { get; } = [];

    /// <summary>
    /// Gets the collection of folders contained in the document.
    /// </summary>
    /// <value>A list of <see cref="KmlFolder"/> objects.</value>
    public IList<KmlPlacemark> Placemarks { get; } = [];

    /// <summary>
    /// Gets the collection of folders contained in the document.
    /// </summary>
    /// <value>A list of <see cref="KmlFolder"/> objects.</value>
    public IEnumerable<KmlPlacemark> AllPlacemarks => Placemarks.Concat(Folders.SelectMany(x => x.Placemarks));

    /// <summary>
    /// Loads a KML document from a stream synchronously.
    /// </summary>
    /// <param name="stream">The stream containing KML data.</param>
    /// <returns>A <see cref="KmlDocument"/> instance.</returns>
    public static KmlDocument Load(Stream stream) => LoadAsync(stream).GetAwaiter().GetResult();

    /// <summary>
    /// Loads a KML document from the specified file path synchronously.
    /// </summary>
    /// <param name="path">The path to the KML file.</param>
    /// <returns>A <see cref="KmlDocument"/> instance.</returns>
    public static KmlDocument Load(string path)
    {
        using var stream = File.OpenRead(path);
        return Load(stream);
    }

    /// <summary>
    /// Asynchronously loads a KML document from the specified file path.
    /// </summary>
    /// <param name="path">The path to the KML file.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see cref="KmlDocument"/>.</returns>
    public static async Task<KmlDocument> LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return await LoadAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Parses a KML document from a string.
    /// </summary>
    /// <param name="text">The string containing KML data.</param>
    /// <returns>A <see cref="KmlDocument"/> instance.</returns>
    public static KmlDocument Parse(string text)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return Load(stream);
    }

    /// <summary>
    /// Saves the KML document to the specified file path synchronously.
    /// </summary>
    /// <param name="path">The path where the KML file will be saved.</param>
    public void Save(string path)
    {
        using var stream = File.Create(path);
        WriteTo(stream);
    }

    /// <summary>
    /// Writes the KML document to the specified stream synchronously.
    /// </summary>
    /// <param name="stream">The stream to write the KML data to.</param>
    public void WriteTo(Stream stream)
    {
        WriteToAsync(stream).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Asynchronously saves the KML document to the specified file path.
    /// </summary>
    /// <param name="path">The path where the KML file will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(path);
        await WriteToAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously loads a KML document from a stream.
    /// </summary>
    /// <param name="stream">The stream containing KML data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see cref="KmlDocument"/>.</returns>
    public static async Task<KmlDocument> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var xDoc = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken).ConfigureAwait(false);
        var ns = xDoc.Root!.GetDefaultNamespace();
        if (xDoc.Root.Name != ns + KmlReader.Root_Element_Name) throw new FormatException("Root element is not <kml>");
        var documentElement = xDoc.Root.Element(ns + KmlReader.Document_Element_Name);
        var id = documentElement?.Attribute(KmlReader.Id_Attribute_Name)?.Value ?? string.Empty;
        var name = documentElement?.Element(ns + KmlReader.Name_Element_Name)?.Value ?? string.Empty;
        var description = documentElement?.Element(ns + KmlReader.Description_Element_Name)?.Value ?? string.Empty;

        var doc = new KmlDocument(new KmlDocumentItem(id, name, description));

        // Placemarks (root level or document level if there is a document)
        AddPlacemarksToList(doc.Placemarks, documentElement ?? xDoc.Root, ns);

        // Folders in the document (root level or document level if there is a document)
        AddFoldersToList(doc.Folders, documentElement ?? xDoc.Root, ns);
        return doc;
    }

    /// <summary>
    /// Asynchronously writes the KML document to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write the KML data to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var writer = KmlWriter.Create(stream);
        await writer.WriteStartKmlAsync(Id, Name, Description);
        await writer.WriteFoldersAsync(Folders, cancellationToken);
        await writer.WritePlacemarksAsync(Placemarks, cancellationToken);
        await writer.WriteEndKmlAsync();
    }

    private static void AddFoldersToList(IList<KmlFolder> folders, XElement element, XNamespace ns)
    {
        foreach (var folderElement in element.Elements(ns + KmlReader.Folder_Element_Name))
        {
            var id = folderElement.Attribute(KmlReader.Id_Attribute_Name)?.Value ?? string.Empty;
            var name = folderElement.Element(ns + KmlReader.Name_Element_Name)?.Value ?? string.Empty;
            var description = folderElement.Element(ns + KmlReader.Description_Element_Name)?.Value ?? string.Empty;
            var folder = new KmlFolder(new KmlFolderItem(id, name, description));
            // Placemarks in the folder
            AddPlacemarksToList(folder.Placemarks, folderElement, ns);
            // Nested folders?
            //AddFoldersToList(folder.Folders, folderElement, ns);
            folders.Add(folder);
        }
    }

    private static void AddPlacemarksToList(IList<KmlPlacemark> placemarks, XElement element, XNamespace ns)
    {
        foreach (var placemarkElement in element.Elements(ns + KmlReader.Placemark_Element_Name))
        {
            var id = placemarkElement.Attribute(KmlReader.Id_Attribute_Name)?.Value ?? string.Empty;
            var name = placemarkElement.Element(ns + KmlReader.Name_Element_Name)?.Value ?? string.Empty;
            var description = placemarkElement.Element(ns + KmlReader.Description_Element_Name)?.Value ?? string.Empty;
            var geometry = CreateGeometry(placemarkElement, ns);
            var placemark = new KmlPlacemark(new KmlPlacemarkItem(id, name, description, geometry));
            placemarks.Add(placemark);
        }
    }

    private static KmlGeometry CreateGeometry(XElement placemarkElement, XNamespace ns)
    {
        var result = new KmlUnsupportedGeometry();
        foreach (var element in placemarkElement.Elements())
        {
            switch (element.Name.LocalName)
            {
                case KmlReader.Point_Element_Name:
                    return CreatePoint(element, ns);
                case KmlReader.Polygon_Element_Name:
                    return CreatePolygonGeometry(element, ns);
                case KmlReader.LineString_Element_Name:
                    return CreateReadLineStringGeometry(element, ns);
                default:
                    continue;
            }
        }
        return result;
    }

    private static KmlLineString CreateReadLineStringGeometry(XElement element, XNamespace ns)
    {
        var coordinatesElement = element.Element(ns + KmlReader.Coordinates_Element_Name);
        if (coordinatesElement == null) return new KmlLineString([]);
        var coordsText = coordinatesElement.Value.Trim();
        var coords = KmlReader.ParseCoordinatesList(coordsText);
        return new KmlLineString(coords);
    }

    private static KmlPolygon CreatePolygonGeometry(XElement element, XNamespace ns)
    {
        var outerBoundaryElement = element.Element(ns + KmlReader.OuterBoundaryIs_Element_Name);
        if (outerBoundaryElement == null) return new KmlPolygon([], []);
        var linearRingElement = outerBoundaryElement.Element(ns + KmlReader.LinearRing_Element_Name);
        if (linearRingElement == null) return new KmlPolygon([], []);
        var coordinatesElement = linearRingElement.Element(ns + KmlReader.Coordinates_Element_Name);
        if (coordinatesElement == null) return new KmlPolygon([], []);
        var coordsText = coordinatesElement.Value.Trim();
        var outerCoords = KmlReader.ParseCoordinatesList(coordsText);
        return new KmlPolygon(outerCoords, []);
    }

    private static KmlPoint CreatePoint(XElement pointElement, XNamespace ns)
    {
        var coordinatesElement = pointElement.Element(ns + KmlReader.Coordinates_Element_Name);
        if (coordinatesElement == null) return new KmlPoint(0, 0, 0);
        var coordsText = coordinatesElement.Value.Trim();
        return KmlReader.ParseCoordinates(coordsText);
    }
}
