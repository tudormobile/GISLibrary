using System.Buffers.Binary;
using System.Text;
using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitHeaderTests
{
    /// <summary>
    /// Builds a representative FIT header byte sequence, as produced by modern FIT encoders
    /// (typically a 14-byte header including the optional 2-byte header CRC).
    /// </summary>
    /// <param name="headerSize">Declared header size byte.</param>
    /// <param name="protocolVersion">Protocol version byte.</param>
    /// <param name="profileVersion">Profile version (little-endian ushort).</param>
    /// <param name="dataSize">Data size (little-endian uint).</param>
    /// <param name="dataType">4-character data type marker.</param>
    /// <param name="includeHeaderCrc">Whether to append a 2-byte header CRC (making a 14-byte header).</param>
    private static byte[] BuildHeaderBytes(
        byte headerSize = 14,
        byte protocolVersion = 16,
        ushort profileVersion = 2158,
        uint dataSize = 21000,
        string dataType = ".FIT",
        bool includeHeaderCrc = true)
    {
        var length = includeHeaderCrc ? 14 : 12;
        var bytes = new byte[length];
        bytes[0] = headerSize;
        bytes[1] = protocolVersion;
        BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(2, 2), profileVersion);
        BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(4, 4), dataSize);
        Encoding.ASCII.GetBytes(dataType).CopyTo(bytes, 8);

        if (includeHeaderCrc)
        {
            // Header CRC value is not validated by FitHeader, so any placeholder is acceptable here.
            BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(12, 2), 0x0000);
        }

        return bytes;
    }

    [TestMethod]
    public void Constructor_WithRepresentativeModernHeader_ParsesAllFields()
    {
        var bytes = BuildHeaderBytes(headerSize: 14, protocolVersion: 16, profileVersion: 2158, dataSize: 21000, dataType: ".FIT");

        var header = new FitHeader(bytes);

        Assert.AreEqual(14, header.HeaderSize);
        Assert.AreEqual(16, header.ProtocolVersion);
        Assert.AreEqual((ushort)2158, header.ProfileVersion);
        Assert.AreEqual(21000u, header.DataSize);
        Assert.AreEqual(".FIT", header.DataType);
    }

    [TestMethod]
    public void Constructor_With12ByteHeaderWithoutCrc_ParsesSuccessfully()
    {
        var bytes = BuildHeaderBytes(headerSize: 12, includeHeaderCrc: false);

        var header = new FitHeader(bytes);

        Assert.AreEqual(12, header.HeaderSize);
        Assert.AreEqual(".FIT", header.DataType);
    }

    [TestMethod]
    public void Constructor_WithTooFewBytes_ThrowsFormatException()
    {
        var bytes = BuildHeaderBytes()[..(FitHeader.MinimumSize - 1)];

        Assert.ThrowsExactly<FormatException>(() => new FitHeader(bytes));
    }

    [TestMethod]
    public void Constructor_WithHeaderSizeSmallerThanMinimum_ThrowsFormatException()
    {
        var bytes = BuildHeaderBytes(headerSize: 10, includeHeaderCrc: false);

        Assert.ThrowsExactly<FormatException>(() => new FitHeader(bytes));
    }

    [TestMethod]
    public void Constructor_WithInvalidDataType_ThrowsFormatException()
    {
        var bytes = BuildHeaderBytes();
        Encoding.ASCII.GetBytes("BAD!").CopyTo(bytes, 8);

        Assert.ThrowsExactly<FormatException>(() => new FitHeader(bytes));
    }

    [TestMethod]
    public void Constructor_WithDataSize_BuildsStandard14ByteHeader()
    {
        var header = new FitHeader(21000u);

        Assert.AreEqual(14, header.HeaderSize);
        Assert.AreEqual(16, header.ProtocolVersion);
        Assert.AreEqual((ushort)2022, header.ProfileVersion);
        Assert.AreEqual(21000u, header.DataSize);
        Assert.AreEqual(".FIT", header.DataType);
    }
}
