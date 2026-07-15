using System.Buffers.Binary;
using System.Text;

namespace Tudormobile.GIS.Fit;

/// <summary>
/// Represents the 12 (or 14) byte header at the start of a FIT file.
/// </summary>
public class FitHeader
{
    /// <summary>
    /// The minimum number of bytes required to parse a <see cref="FitHeader"/>.
    /// </summary>
    public const int MinimumSize = 12;

    /// <summary>
    /// The data type marker used to identify a valid FIT file (".FIT").
    /// </summary>
    public const string FitDataType = ".FIT";

    /// <summary>
    /// Standard header size, in bytes, used when constructing a new header.
    /// </summary>
    private const byte DefaultHeaderSize = 14;

    /// <summary>
    /// Default FIT protocol version (1.0) used when constructing a new header.
    /// </summary>
    private const byte DefaultProtocolVersion = 16;

    /// <summary>
    /// Default FIT profile version (20.22) used when constructing a new header.
    /// </summary>
    private const ushort DefaultProfileVersion = 2022;

    /// <summary>
    /// Size of this header, in bytes, as declared by the file itself (typically 12 or 14).
    /// </summary>
    public byte HeaderSize { get; }

    /// <summary>
    /// FIT protocol version.
    /// </summary>
    public byte ProtocolVersion { get; }

    /// <summary>
    /// FIT profile version.
    /// </summary>
    public ushort ProfileVersion { get; }

    /// <summary>
    /// Size, in bytes, of the data records that follow the header (excludes header and trailing CRC).
    /// </summary>
    public uint DataSize { get; }

    /// <summary>
    /// The data type marker. Always ".FIT" for valid FIT files.
    /// </summary>
    public string DataType { get; }

    /// <summary>
    /// Parses and validates a FIT header from the supplied bytes.
    /// </summary>
    /// <param name="data">At least <see cref="MinimumSize"/> bytes read from the start of a FIT file.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="data"/> does not contain a valid FIT header.</exception>
    public FitHeader(ReadOnlySpan<byte> data)
    {
        if (data.Length < MinimumSize)
        {
            throw new FormatException($"FIT header requires at least {MinimumSize} bytes.");
        }

        HeaderSize = data[0];
        if (HeaderSize < MinimumSize)
        {
            throw new FormatException($"Invalid FIT header size: {HeaderSize}.");
        }

        ProtocolVersion = data[1];
        ProfileVersion = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(2, 2));
        DataSize = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(4, 4));
        DataType = Encoding.ASCII.GetString(data.Slice(8, 4));

        if (DataType != FitDataType)
        {
            throw new FormatException($"Invalid FIT data type marker: '{DataType}'.");
        }
    }

    /// <summary>
    /// Builds a standard 14-byte FIT header with the given data size.
    /// </summary>
    /// <param name="dataSize">Size, in bytes, of the data records that follow the header.</param>
    /// <remarks>
    /// Uses <see cref="HeaderSize"/> = 14, <see cref="ProtocolVersion"/> = 16 (protocol version 1.0),
    /// and <see cref="ProfileVersion"/> = 2022 (profile version 20.22).
    /// </remarks>
    public FitHeader(uint dataSize)
    {
        HeaderSize = DefaultHeaderSize;
        ProtocolVersion = DefaultProtocolVersion;
        ProfileVersion = DefaultProfileVersion;
        DataSize = dataSize;
        DataType = FitDataType;
    }

}
