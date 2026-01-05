using System.IO.Compression;

namespace Tudormobile.Kml;

/// <summary>
/// Represents a KMZ document, which is a compressed KML file in ZIP format.
/// </summary>
public class KmzDocument
{
    /// <summary>
    /// Gets or initializes the KML document contained within the KMZ archive.
    /// </summary>
    /// <value>The <see cref="KmlDocument"/> extracted from the KMZ file.</value>
    public KmlDocument Document { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KmzDocument"/> class.
    /// </summary>
    /// <param name="path">The file path of the KMZ document.</param>
    /// <param name="document">The KML document contained within the KMZ archive.</param>
    public KmzDocument(string path, KmlDocument document) { Document = document; }

    /// <summary>
    /// Loads a KMZ document from the specified file path synchronously.
    /// </summary>
    /// <param name="path">The path to the KMZ file to load.</param>
    /// <returns>A <see cref="KmzDocument"/> instance containing the loaded KML document.</returns>
    public static KmzDocument Load(string path) => LoadAsync(path).GetAwaiter().GetResult();

    /// <summary>
    /// Saves the KMZ document to the specified file path synchronously.
    /// </summary>
    /// <param name="path">The path where the KMZ file will be saved.</param>
    public void Save(string path) => SaveAsync(path).GetAwaiter().GetResult();

    /// <summary>
    /// Asynchronously loads a KMZ document from the specified file path.
    /// </summary>
    /// <param name="path">The path to the KMZ file to load.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded <see cref="KmzDocument"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when no KML file is found in the KMZ archive.</exception>
    public static async Task<KmzDocument> LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        using var zipArchive = ZipFile.OpenRead(path);
        var entry = zipArchive.Entries.FirstOrDefault(e => e.FullName.EndsWith(".kml"))
            ?? throw new FileNotFoundException("No KML file found in the KMZ archive.");
        using var entryStream = entry.Open();
        return await KmlDocument.LoadAsync(entryStream, cancellationToken)
            .ContinueWith(t => new KmzDocument(path, t.Result), cancellationToken);
    }

    /// <summary>
    /// Asynchronously saves the KMZ document to the specified file path.
    /// </summary>
    /// <param name="path">The path where the KMZ file will be saved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        using var zipArchive = await ZipFile.OpenAsync(path, ZipArchiveMode.Create).ConfigureAwait(false);
        var entry = zipArchive.CreateEntry("doc.kml");
        using var entryStream = entry.Open();
        await Document.WriteToAsync(entryStream, cancellationToken).ConfigureAwait(false);
    }
}
