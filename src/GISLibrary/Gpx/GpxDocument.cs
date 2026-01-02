using System.Xml.Linq;

namespace Tudormobile.Gpx;

/// <summary>
/// Represents a GPX (GPS Exchange Format) document with support for versions 1.0 and 1.1.
/// </summary>
/// <remarks>
/// Currently, only reading/parsing of GPX documents is supported. Writing or modifying GPX documents is not implemented.
/// </remarks>
public partial class GpxDocument : GpxDocumentBase
{
    private readonly string _version;
    private string? _name;
    private string? _description;
    private string? _author;
    private string? _email;
    private string? _url;
    private string? _urlName;
    private DateTime? _time;
    private string? _keywords;
    private GpxBounds? _bounds;

    /// <summary>
    /// Initializes a new instance of the <see cref="GpxDocument"/> class.
    /// </summary>
    /// <param name="document">The XML document containing GPX data.</param>
    /// <exception cref="ArgumentNullException">Thrown when the document is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the document is invalid or missing required elements.</exception>
    /// <exception cref="NotSupportedException">Thrown when the GPX version is not 1.0 or 1.1.</exception>
    public GpxDocument(XDocument document) : base(document, "gpx")
    {
        if (_root.Attribute("version") == null)
        {
            throw new ArgumentException("The provided GPX document does not have a version attribute.");
        }
        _version = (_root.Attribute("version")?.Value) ?? throw new ArgumentException("The provided GPX document does not have a version attribute value.");
        if (_version != "1.0" && _version != "1.1")
        {
            throw new NotSupportedException($"GPX version {_version} is not supported. Only versions 1.0 and 1.1 are supported.");
        }
    }

    /// <summary>
    /// Gets the GPX version of the document (e.g., "1.0" or "1.1").
    /// </summary>
    public string Version => _version;

    /// <summary>
    /// Gets a value indicating whether the document is GPX version 1.1.
    /// </summary>
    public bool IsVersion11 => _version == "1.1";

    /// <summary>
    /// Gets the name of the GPX file or track.
    /// </summary>
    public string Name => _name ??= GetStringValueAnyVersion("name") ?? string.Empty;

    /// <summary>
    /// Gets the description of the GPX file or track.
    /// </summary>
    public string Description => _description ??= GetStringValueAnyVersion("desc") ?? string.Empty;

    /// <summary>
    /// Gets the author of the GPX file.
    /// </summary>
    public string Author => _author ??= GetStringValueAnyVersion("author") ?? string.Empty;

    /// <summary>
    /// Gets the email address associated with the GPX file.
    /// </summary>
    public string Email => _email ??= GetStringValueAnyVersion("email") ?? string.Empty;

    /// <summary>
    /// Gets the URL associated with the GPX file.
    /// </summary>
    public string Url => _url ??= GetStringValueAnyVersion("url") ?? string.Empty;

    /// <summary>
    /// Gets the name of the URL associated with the GPX file.
    /// </summary>
    public string UrlName => _urlName ??= GetStringValueAnyVersion("urlname") ?? string.Empty;

    /// <summary>
    /// Gets the timestamp of when the GPX file was created.
    /// </summary>
    public DateTime Time => _time ??= GetDateTimeValueAnyVersion("time");

    /// <summary>
    /// Gets the keywords associated with the GPX file.
    /// </summary>
    public string Keywords => _keywords ??= GetStringValueAnyVersion("keywords") ?? string.Empty;

    /// <summary>
    /// Gets the list of waypoints in the GPX document.
    /// </summary>
    public List<GpxWaypoint> Waypoints => [.. _root.Elements(_defaultNamespace + "wpt").Select(x => new GpxWaypoint(x))];

    /// <summary>
    /// Gets the list of routes in the GPX document.
    /// </summary>
    public List<GpxRoute> Routes => [.. _root.Elements(_defaultNamespace + "rte").Select(rte => new GpxRoute(rte))];

    /// <summary>
    /// Gets the list of tracks in the GPX document.
    /// </summary>
    public List<GpxTrack> Tracks => [.. _root.Elements(_defaultNamespace + "trk").Select(trk => new GpxTrack(trk))];

    /// <summary>
    /// Gets the geographic bounds of the GPX data.
    /// </summary>
    public GpxBounds Bounds => _bounds ??= new(IsVersion11
        ? (_root.Element(_defaultNamespace + "metadata")?.Element(_defaultNamespace + "bounds"))
        : _root.Element(_defaultNamespace + "bounds")
    );

    private string? GetStringValueAnyVersion(string elementName)
    {
        var element = IsVersion11
                ? (_root.Element(_defaultNamespace + "metadata")?.Element(_defaultNamespace + elementName))
                : _root.Element(_defaultNamespace + elementName);
        return element?.Value;
    }

    private DateTime GetDateTimeValueAnyVersion(string name)
    {
        var timeString = GetStringValueAnyVersion(name);
        if (timeString != null && DateTime.TryParse(timeString, out DateTime result))
        {
            return result;
        }
        return DateTime.MinValue;
    }
}
