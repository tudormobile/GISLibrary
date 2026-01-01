namespace Tudormobile.GeoJSON;

/// <summary>
/// Represents a GeoJSON file on disk and provides simple file operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeoJSONFile"/> class for the specified file path.
/// </remarks>
/// <param name="path">The path to the GeoJSON file.</param>
public class GeoJSONFile(string path)
{

    /// <summary>
    /// Determines whether the file exists.
    /// </summary>
    /// <returns><c>true</c> if the file exists; otherwise <c>false</c>.</returns>
    public bool Exists() => File.Exists(path);

    /// <summary>
    /// Gets the last write time of the file.
    /// </summary>
    /// <returns>The <see cref="DateTime"/> the file was last modified.</returns>
    public DateTime GetLastModifiedTime() => File.Exists(path) ? File.GetLastWriteTime(path) : DateTime.MinValue;

    /// <summary>
    /// Gets the size of the file in bytes.
    /// </summary>
    /// <returns>The file size in bytes.</returns>
    public long GetFileSize() => File.Exists(path) ? new FileInfo(path).Length : 0L;

    /// <summary>
    /// Reads the GeoJSON document from the file asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the <see cref="GeoJSONDocument"/>.</returns>
    public async Task<GeoJSONDocument> ReadDocumentAsync(CancellationToken cancellationToken = default)
        => await GeoJSONDocument.LoadFromFileAsync(path, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Writes the provided GeoJSON document to the file asynchronously.
    /// </summary>
    /// <param name="document">The document to write.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteDocumentAsync(GeoJSONDocument document, CancellationToken cancellationToken = default)
        => await document.SaveToFileAsync(path, cancellationToken).ConfigureAwait(false);
}
