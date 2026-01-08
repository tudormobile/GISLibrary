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
    internal const string KML_Document_Namespace = "http://www.opengis.net/kml/2.2";
    internal const string Root_Element_Name = "kml";
    internal const string Document_Element_Name = "Document";
    internal const string Folder_Element_Name = "Folder";
    internal const string Placemark_Element_Name = "Placemark";
    internal const string Id_Attribute_Name = "id";
    internal const string Name_Element_Name = "name";
    internal const string Description_Element_Name = "description";
    internal const string Point_Element_Name = "Point";
    internal const string Polygon_Element_Name = "Polygon";
    internal const string LineString_Element_Name = "LineString";
    internal const string Coordinates_Element_Name = "coordinates";
    internal const string OuterBoundaryIs_Element_Name = "outerBoundaryIs";
    internal const string InnerBoundaryIs_Element_Name = "innerBoundaryIs";
    internal const string LinearRing_Element_Name = "LinearRing";
    private readonly XmlReader _xmlReader;
    private KmlReader(XmlReader xmlReader) { _xmlReader = xmlReader; }
    private static readonly char[] separator = [' ', '\n', '\r', '\t'];

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
        _xmlReader.ReadStartElement(Document_Element_Name, KML_Document_Namespace);
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
        _xmlReader.ReadStartElement(Folder_Element_Name, KML_Document_Namespace);
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
        _xmlReader.ReadStartElement(Placemark_Element_Name, KML_Document_Namespace);
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

    /// <summary>
    /// Determines whether the current position is at the start of a folder element.
    /// /// </summary>
    /// <returns><see langword="true"/> if the reader is positioned at a folder element; otherwise, <see langword="false"/>.</returns>
    public bool IsFolder() => _xmlReader.IsStartElement(Folder_Element_Name, KML_Document_Namespace);

    /// <summary>
    /// Determines whether the current element is a Document element in the KLM document namespace.
    /// </summary>
    /// <returns><see langword="true"/> if the current element is a Document element in the expected namespace; otherwise, <see
    /// langword="false"/>.</returns>
    public bool IsDocument() => _xmlReader.IsStartElement(Document_Element_Name, KML_Document_Namespace);

    /// <summary>
    /// Determines whether the current position is at the start of a placemark element.
    /// </summary>
    /// <returns><see langword="true"/> if the reader is positioned at a placemark element; otherwise, <see langword="false"/>.</returns>
    public bool IsPlacemark() => _xmlReader.IsStartElement(Placemark_Element_Name, KML_Document_Namespace);

    /// <summary>
    /// Moves the reader to the next placemark element in the document.
    /// </summary>
    /// <returns><see langword="true"/> if a placemark was found; otherwise, <see langword="false"/>.</returns>
    public bool MoveToPlacemark()
    {
        bool result = _xmlReader.ReadToFollowing(Placemark_Element_Name, KML_Document_Namespace);
        ReadState = result ? KmlReadState.Placemark : KmlReadState.EndOfFile;
        return result;
    }

    /// <summary>
    /// Moves the reader to the next folder element in the document.
    /// </summary>
    /// <returns><see langword="true"/> if a folder was found; otherwise, <see langword="false"/>.</returns>
    public bool MoveToFolder()
    {
        bool result = _xmlReader.ReadToFollowing(Folder_Element_Name, KML_Document_Namespace);
        ReadState = result ? KmlReadState.Folder : KmlReadState.EndOfFile;
        return result;
    }

    /// <summary>
    /// Attempts to move the current position to the next document in the collection.
    /// </summary>
    /// <returns>true if the position was successfully moved to the next document; otherwise, false.</returns>
    public bool MoveToDocument()
    {
        bool result = _xmlReader.ReadToFollowing(Document_Element_Name, KML_Document_Namespace);
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
    public async IAsyncEnumerable<KmlPlacemark> ReadPlacemarksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await _xmlReader.ReadAsync().ConfigureAwait(false) && !cancellationToken.IsCancellationRequested)
        {
            if (_xmlReader.IsStartElement(Placemark_Element_Name, KML_Document_Namespace))
            {
                var placemarkItem = ReadPlacemark();
                yield return new KmlPlacemark(placemarkItem);
            }
        }
    }

    internal static List<(double Latitude, double Longitude, double Altitude)> ParseCoordinatesList(string coordsText)
    {
        var coords = new List<(double Latitude, double Longitude, double Altitude)>();
        var coordPairs = coordsText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        foreach (var coordPair in coordPairs)
        {
            var coordsParts = coordPair.Split(',');
            if (coordsParts.Length < 2) continue;
            _ = double.TryParse(coordsParts[0], out double longitude);
            _ = double.TryParse(coordsParts[1], out double latitude);
            double altitude = 0;
            if (coordsParts.Length >= 3)
            {
                _ = double.TryParse(coordsParts[2], out altitude);
            }
            coords.Add((latitude, longitude, altitude));
        }
        return coords;
    }

    internal static KmlPoint ParseCoordinates(string coordsText)
    {
        var coordsParts = coordsText.Split(',');
        if (coordsParts.Length < 2) return new KmlPoint(0, 0, 0);
        if (!double.TryParse(coordsParts[0], out double longitude)) longitude = 0;
        if (!double.TryParse(coordsParts[1], out double latitude)) latitude = 0;
        double altitude = 0;
        if (coordsParts.Length >= 3)
        {
            _ = double.TryParse(coordsParts[2], out altitude);
        }
        return new KmlPoint(latitude, longitude, altitude);
    }

    private KmlPoint? ReadPointGeometry()
    {
        KmlPoint? point = null;
        _xmlReader.ReadStartElement(Point_Element_Name, KML_Document_Namespace);
        while (_xmlReader.IsStartElement())
        {
            if (_xmlReader.LocalName == Coordinates_Element_Name)
            {
                string coordText = _xmlReader.ReadElementContentAsString();
                point = ParseCoordinates(coordText);
            }
            else
            {
                _xmlReader.Skip();
            }
        }
        _xmlReader.ReadEndElement(); // Point
        return point;
    }

    private KmlPolygon? ReadPolygonGeometry()
    {
        List<(double, double, double)>? outerBoundary = null;
        List<(double, double, double)>? innerBoundary = null;
        _xmlReader.ReadStartElement(Polygon_Element_Name, KML_Document_Namespace);
        while (_xmlReader.IsStartElement())
        {
            if (_xmlReader.LocalName == OuterBoundaryIs_Element_Name)
            {
                outerBoundary = ReadLinearRing(OuterBoundaryIs_Element_Name);
                _xmlReader.ReadEndElement(); // OuterBoundaryIs
            }
            else if (_xmlReader.LocalName == InnerBoundaryIs_Element_Name)
            {
                innerBoundary = ReadLinearRing(InnerBoundaryIs_Element_Name);
            }
            else
            {
                _xmlReader.Skip();
            }
        }
        _xmlReader.ReadEndElement(); // Polygon
        return new KmlPolygon(outerBoundary!, [innerBoundary!]);
    }

    private List<(double, double, double)> ReadLinearRing(string elementName)
    {
        List<(double, double, double)> points = [];
        _xmlReader.ReadStartElement(elementName, KML_Document_Namespace);
        // Read LinearRing
        _xmlReader.ReadStartElement(LinearRing_Element_Name, KML_Document_Namespace);
        while (_xmlReader.IsStartElement())
        {
            if (_xmlReader.LocalName == Coordinates_Element_Name)
            {
                string coordText = _xmlReader.ReadElementContentAsString();
                points = KmlReader.ParseCoordinatesList(coordText);
            }
        }
        _xmlReader.ReadEndElement(); // LinearRing
        return points;
    }

    private KmlLineString? ReadLineStringGeometry()
    {
        KmlLineString? lineString = null;
        _xmlReader.ReadStartElement(LineString_Element_Name, KML_Document_Namespace);
        while (_xmlReader.IsStartElement())
        {
            if (_xmlReader.LocalName == Coordinates_Element_Name)
            {
                string coordText = _xmlReader.ReadElementContentAsString();
                var coords = KmlReader.ParseCoordinatesList(coordText);
                lineString = new KmlLineString(coords);
            }
            else
            {
                _xmlReader.Skip();
            }
        }
        _xmlReader.ReadEndElement(); // LineString
        return lineString;
    }
}
