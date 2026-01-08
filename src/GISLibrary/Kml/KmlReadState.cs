namespace Tudormobile.Kml;

/// <summary>
/// Specifies the current state of a <see cref="KmlReader"/>.
/// </summary>
public enum KmlReadState
{
    /// <summary>
    /// The reader has been created but not yet started reading.
    /// </summary>
    Initial,

    /// <summary>
    /// The reader is positioned at a document element.
    /// </summary>
    Document,

    /// <summary>
    /// The reader is positioned at a folder element.
    /// </summary>
    Folder,

    /// <summary>
    /// The reader is positioned at a placemark element.
    /// </summary>
    Placemark,

    /// <summary>
    /// The reader has reached the end of the file.
    /// </summary>
    EndOfFile,

    /// <summary>
    /// The reader has been closed.
    /// </summary>
    Closed,

    /// <summary>
    /// An error occurred during reading.
    /// </summary>
    Error
}
