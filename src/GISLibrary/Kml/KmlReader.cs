using System.Runtime.CompilerServices;
using System.Xml;

namespace Tudormobile.Kml;

/// <summary>
/// Provides functionality for reading KML (Keyhole Markup Language) data.
/// </summary>
/// <remarks>
/// Use this class to parse and process KML files or streams. KML is an XML-based format commonly used for 
/// geographic data visualization in applications such as Google Earth. This class is intended for scenarios where 
/// high-performance, forward-only reader is required for larger data sets.
/// <para>
/// Only limited KML features are supported to allow extracting basic geographic information.
/// </para>
/// </remarks>
public class KmlReader : IDisposable
{
    private const string KLM_Document_Namespace = "http://www.opengis.net/kml/2.2";
    private const string Folder_Element_Name = "Folder";
    private const string Name_Element_Name = "name";
    private const string Description_Element_Name = "description";
    private const string Point_Element_Name = "Point";
    private const string Polygon_Element_Name = "Polygon";
    private const string LineString_Element_Name = "LineString";
    private const string Coordinates_Element_Name = "coordinates";
    private const string Placemark_Element_Name = "Placemark";
    private const string Id_Attribute_Name = "id";
    private const string Document_Element_Name = "Document";
    private XmlReader _xmlReader;
    private KmlReader(XmlReader xmlReader) { _xmlReader = xmlReader; }

    /// <summary>
    /// Releases all resources used by the current instance of the class.
    /// </summary>
    public void Dispose()
    {
        _xmlReader.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates a new instance of the KmlReader class for reading KML data from the specified file path.
    /// </summary>
    /// <param name="path">The path to the KML file to read. The path must refer to an existing file.</param>
    /// <param name="allowAsync">True to allow async operations.</param>
    /// <returns>A KmlReader instance that reads KML data from the specified file.</returns>
    public static KmlReader Create(string path, bool allowAsync = false) => new(XmlReader.Create(path, new XmlReaderSettings() { Async = allowAsync }));

    /// <summary>
    /// Creates a new instance of the KmlReader class for reading KML data from the specified stream.
    /// </summary>
    /// <param name="stream">The stream containing KML data to read.</param>
    /// <param name="allowAsync">True to allow async operations.</param>
    /// <returns>A KmlReader instance that reads KML data from the specified stream.</returns>
    public static KmlReader Create(Stream stream, bool allowAsync = false) => new(XmlReader.Create(stream, new XmlReaderSettings() { Async = allowAsync }));

    /// <summary>
    /// Reads the start of a KML document and returns document information.
    /// </summary>
    /// <returns>A <see cref="KmlDocumentItem"/> containing the document's metadata.</returns>
    public KmlDocumentItem ReadDocumentStart()
    {
        // Read the id attribute BEFORE consuming the start element
        string id = _xmlReader.GetAttribute(Id_Attribute_Name) ?? string.Empty;
        _xmlReader.ReadStartElement(Document_Element_Name, KLM_Document_Namespace);
        string name = string.Empty;
        string description = string.Empty;
        while (_xmlReader.IsStartElement())
        {
            switch (_xmlReader.LocalName)
            {
                case Name_Element_Name:
                    name = _xmlReader.ReadElementContentAsString();
                    break;
                case Description_Element_Name:
                    description = _xmlReader.ReadElementContentAsString();
                    break;
                default:
                    _xmlReader.Skip();
                    break;
            }
        }
        ReadState = KmlReadState.Document;
        return new KmlDocumentItem(id, name, description);
    }

    /// <summary>
    /// Reads the start of a KML folder and returns folder information.
    /// </summary>
    /// <returns>A <see cref="KmlFolderItem"/> containing the folder's metadata.</returns>
    public KmlFolderItem ReadFolderStart()
    {
        // Read the id attribute BEFORE consuming the start element
        string id = _xmlReader.GetAttribute(Id_Attribute_Name) ?? string.Empty;
        _xmlReader.ReadStartElement(Folder_Element_Name, KLM_Document_Namespace);
        string name = string.Empty;
        string description = string.Empty;
        while (_xmlReader.IsStartElement())
        {
            switch (_xmlReader.LocalName)
            {
                case Name_Element_Name:
                    name = _xmlReader.ReadElementContentAsString();
                    break;
                case Description_Element_Name:
                    description = _xmlReader.ReadElementContentAsString();
                    break;
                default:
                    _xmlReader.Skip();
                    break;
            }
        }
        ReadState = KmlReadState.Folder;
        return new KmlFolderItem(id, name, description);
    }

    /// <summary>
    /// Reads a KML placemark and returns its information.
    /// </summary>
    /// <returns>A <see cref="KmlPlacemarkItem"/> containing the placemark's data and geometry.</returns>
    public KmlPlacemarkItem ReadPlacemark()
    {
        // Read the id attribute BEFORE consuming the start element
        string id = _xmlReader.GetAttribute(Id_Attribute_Name) ?? string.Empty;
        _xmlReader.ReadStartElement(Placemark_Element_Name, KLM_Document_Namespace);
        string name = string.Empty;
        string description = string.Empty;
        KmlGeometry? geometry = null;
        while (_xmlReader.IsStartElement())
        {
            switch (_xmlReader.LocalName)
            {
                case Name_Element_Name:
                    name = _xmlReader.ReadElementContentAsString();
                    break;
                case Description_Element_Name:
                    description = _xmlReader.ReadElementContentAsString();
                    break;
                case Point_Element_Name:
                    geometry = ReadPointGeometry();
                    break;
                case Polygon_Element_Name:
                    geometry = ReadPolygonGeometry();
                    break;
                case LineString_Element_Name:
                    geometry = ReadLineStringGeometry();
                    break;
                default:
                    geometry = new KmlUnsupportedGeometry();
                    _xmlReader.Skip();
                    break;
            }
        }
        _xmlReader.ReadEndElement(); // Placemark
        ReadState = KmlReadState.Placemark;
        return geometry != null
            ? new KmlPlacemarkItem(id, name, description, geometry)
            : throw new InvalidOperationException("Placemark geometry could not be read.");
    }

    private KmlGeometry? ReadPointGeometry()
    {
        KmlPoint? point = null;
        _xmlReader.ReadStartElement(Point_Element_Name, KLM_Document_Namespace);
        while (_xmlReader.IsStartElement())
        {
            if (_xmlReader.LocalName == Coordinates_Element_Name)
            {
                string coordText = _xmlReader.ReadElementContentAsString();
                var coords = coordText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (coords.Length >= 2 &&
                    double.TryParse(coords[1], out double latitude) &&
                    double.TryParse(coords[0], out double longitude))
                {
                    double altitude = 0;
                    if (coords.Length >= 3)
                    {
                        double.TryParse(coords[2], out altitude);
                    }
                    point = new KmlPoint(latitude, longitude, altitude);
                }
            }
            else
            {
                _xmlReader.Skip();
            }
        }
        _xmlReader.ReadEndElement(); // Point
        return point;
    }

    private KmlGeometry? ReadPolygonGeometry()
    {
        KmlPolygon? polygon = null;
        return polygon;
    }

    private KmlGeometry? ReadLineStringGeometry()
    {
        KmlLineString? lineString = null;
        return lineString;
    }

    /// <summary>
    /// Determines whether the current position is at the start of a folder element.
    /// /// </summary>
    /// <returns><see langword="true"/> if the reader is positioned at a folder element; otherwise, <see langword="false"/>.</returns>
    public bool IsFolder() => _xmlReader.IsStartElement(Folder_Element_Name, KLM_Document_Namespace);

    /// <summary>
    /// Determines whether the current element is a Document element in the KLM document namespace.
    /// </summary>
    /// <returns><see langword="true"/> if the current element is a Document element in the expected namespace; otherwise, <see
    /// langword="false"/>.</returns>
    public bool IsDocument() => _xmlReader.IsStartElement(Document_Element_Name, KLM_Document_Namespace);

    /// <summary>
    /// Determines whether the current position is at the start of a placemark element.
    /// </summary>
    /// <returns><see langword="true"/> if the reader is positioned at a placemark element; otherwise, <see langword="false"/>.</returns>
    public bool IsPlacemark() => _xmlReader.IsStartElement(Placemark_Element_Name, KLM_Document_Namespace);

    /// <summary>
    /// Moves the reader to the next placemark element in the document.
    /// </summary>
    /// <returns><see langword="true"/> if a placemark was found; otherwise, <see langword="false"/>.</returns>
    public bool MoveToPlacemark()
    {
        bool result = _xmlReader.ReadToFollowing(Placemark_Element_Name, KLM_Document_Namespace);
        ReadState = result ? KmlReadState.Placemark : KmlReadState.EndOfFile;
        return result;
    }

    /// <summary>
    /// Moves the reader to the next folder element in the document.
    /// </summary>
    /// <returns><see langword="true"/> if a folder was found; otherwise, <see langword="false"/>.</returns>
    public bool MoveToFolder()
    {
        bool result = _xmlReader.ReadToFollowing(Folder_Element_Name, KLM_Document_Namespace);
        ReadState = result ? KmlReadState.Folder : KmlReadState.EndOfFile;
        return result;
    }

    /// <summary>
    /// Attempts to move the current position to the next document in the collection.
    /// </summary>
    /// <returns>true if the position was successfully moved to the next document; otherwise, false.</returns>
    public bool MoveToDocument()
    {
        bool result = _xmlReader.ReadToFollowing(Document_Element_Name, KLM_Document_Namespace);
        ReadState = result ? KmlReadState.Document : KmlReadState.EndOfFile;
        return result;
    }

    /// <summary>
    /// Gets the current state of the reader.
    /// </summary>
    /// <value>A <see cref="Kml.KmlReadState"/> value indicating the reader's current position in the document.</value>
    public KmlReadState ReadState { get; private set; }

    /// <summary>
    /// Asynchronously reads all placemarks from the KML document.
    /// </summary>
    /// <returns>An asynchronous enumerable sequence of <see cref="KmlPlacemark"/> objects.</returns>
    /// <remarks>
    /// You must have created this <see cref="KmlReader"/> instance with async support enabled.
    /// </remarks>
    public async IAsyncEnumerable<KmlPlacemark> ReadPlacemarksAsync([EnumeratorCancellation] CancellationToken cancellation = default)
    {
        while (await _xmlReader.ReadAsync().ConfigureAwait(false))
        {
            if (_xmlReader.IsStartElement(Placemark_Element_Name, KLM_Document_Namespace))
            {
                var placemarkItem = ReadPlacemark();
                yield return new KmlPlacemark(placemarkItem);
            }
        }
    }
}
