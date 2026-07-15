using System.IO.Compression;

namespace Tudormobile.GIS.Fit;

/// <summary>
/// Represents a FIT (or compressed FIT) file on disk.
/// </summary>
/// <param name="path">Path to the file.</param>
public class FitFile(string path)
{
    /// <summary>
    /// Gets the file system path associated with this instance.
    /// </summary>
    public string Path
        => !string.IsNullOrWhiteSpace(path)
            ? path
            : throw new ArgumentException("Path cannot be null, empty, or whitespace.", nameof(path));

    /// <summary>
    /// Determines whether the file exists.
    /// </summary>
    /// <returns><c>true</c> if the file exists; otherwise <c>false</c>.</returns>
    public bool Exists() => File.Exists(Path);

    /// <summary>
    /// Gets the last write time of the file.
    /// </summary>
    /// <returns>The <see cref="DateTime"/> the file was last modified.</returns>
    public DateTime GetLastModifiedTime() => Exists() ? File.GetLastWriteTime(Path) : DateTime.MinValue;

    /// <summary>
    /// Gets the size of the file in bytes.
    /// </summary>
    /// <returns>The file size in bytes.</returns>
    public long GetFileSize() => Exists() ? new FileInfo(Path).Length : 0L;

    /// <summary>
    /// Gzip magic number: the first two bytes of a gzip-compressed stream.
    /// </summary>
    private const byte GZipMagicByte0 = 0x1F;
    private const byte GZipMagicByte1 = 0x8B;

    /// <summary>
    /// Determines whether the given stream begins with the gzip magic number.
    /// The stream position is restored afterwards, so the stream must support seeking.
    /// </summary>
    private static bool IsGZipCompressed(Stream stream)
    {
        if (!stream.CanSeek || stream.Length < 2)
        {
            return false;
        }

        var position = stream.Position;
        var first = stream.ReadByte();
        var second = stream.ReadByte();
        stream.Position = position;

        return first == GZipMagicByte0 && second == GZipMagicByte1;
    }

    /// <summary>
    /// Creates a reader for this file, transparently decompressing it if it is gzip-compressed.
    /// </summary>
    public FitStreamReader CreateReader()
    {
        Stream stream = File.OpenRead(path);
        if (IsGZipCompressed(stream))
        {
            stream = new GZipStream(stream, CompressionMode.Decompress);
        }
        return new FitStreamReader(stream);
    }

}
