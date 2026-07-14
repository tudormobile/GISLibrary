using System.Buffers.Binary;
using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitBaseTypeTests
{
    [TestMethod]
    [DataRow(FitBaseType.Enum, 1)]
    [DataRow(FitBaseType.SInt8, 1)]
    [DataRow(FitBaseType.UInt8, 1)]
    [DataRow(FitBaseType.UInt8z, 1)]
    [DataRow(FitBaseType.Byte, 1)]
    [DataRow(FitBaseType.String, 1)]
    [DataRow(FitBaseType.SInt16, 2)]
    [DataRow(FitBaseType.UInt16, 2)]
    [DataRow(FitBaseType.UInt16z, 2)]
    [DataRow(FitBaseType.SInt32, 4)]
    [DataRow(FitBaseType.UInt32, 4)]
    [DataRow(FitBaseType.UInt32z, 4)]
    [DataRow(FitBaseType.Float32, 4)]
    [DataRow(FitBaseType.SInt64, 8)]
    [DataRow(FitBaseType.UInt64, 8)]
    [DataRow(FitBaseType.UInt64z, 8)]
    [DataRow(FitBaseType.Float64, 8)]
    public void GetElementSize_ReturnsExpectedSize(FitBaseType baseType, int expectedSize)
    {
        Assert.AreEqual(expectedSize, baseType.GetElementSize());
    }

    [TestMethod]
    public void GetElementSize_WithUnknownValue_ThrowsNotSupportedException()
    {
        var baseType = (FitBaseType)0xFF;

        Assert.ThrowsExactly<NotSupportedException>(() => baseType.GetElementSize());
    }

    [TestMethod]
    [DataRow(FitBaseType.Enum, 0xFFUL)]
    [DataRow(FitBaseType.UInt8, 0xFFUL)]
    [DataRow(FitBaseType.UInt8z, 0xFFUL)]
    [DataRow(FitBaseType.Byte, 0xFFUL)]
    [DataRow(FitBaseType.SInt8, 0x7FUL)]
    [DataRow(FitBaseType.UInt16, 0xFFFFUL)]
    [DataRow(FitBaseType.UInt16z, 0xFFFFUL)]
    [DataRow(FitBaseType.SInt16, 0x7FFFUL)]
    [DataRow(FitBaseType.UInt32, 0xFFFFFFFFUL)]
    [DataRow(FitBaseType.UInt32z, 0xFFFFFFFFUL)]
    [DataRow(FitBaseType.SInt32, 0x7FFFFFFFUL)]
    [DataRow(FitBaseType.UInt64, 0xFFFFFFFFFFFFFFFFUL)]
    [DataRow(FitBaseType.UInt64z, 0xFFFFFFFFFFFFFFFFUL)]
    [DataRow(FitBaseType.SInt64, 0x7FFFFFFFFFFFFFFFUL)]
    public void GetInvalidValue_ReturnsExpectedValue(FitBaseType baseType, ulong expected)
    {
        Assert.AreEqual(expected, baseType.GetInvalidValue());
    }

    [TestMethod]
    [DataRow(FitBaseType.String)]
    [DataRow(FitBaseType.Float32)]
    [DataRow(FitBaseType.Float64)]
    public void GetInvalidValue_WithNonSentinelType_ReturnsZero(FitBaseType baseType)
    {
        Assert.AreEqual(0UL, baseType.GetInvalidValue());
    }

    [TestMethod]
    public void GetInvalidValue_WithUnknownValue_ThrowsNotSupportedException()
    {
        var baseType = (FitBaseType)0xFF;

        Assert.ThrowsExactly<NotSupportedException>(() => baseType.GetInvalidValue());
    }

    [TestMethod]
    public void Decode_String_WithNullTerminator_ReturnsTruncatedText()
    {
        var data = new byte[] { (byte)'A', (byte)'B', 0, (byte)'C' };

        var result = FitBaseType.String.Decode(data, littleEndian: true);

        Assert.AreEqual("AB", result);
    }

    [TestMethod]
    public void Decode_String_WithoutNullTerminator_ReturnsFullText()
    {
        var data = new byte[] { (byte)'A', (byte)'B', (byte)'C' };

        var result = FitBaseType.String.Decode(data, littleEndian: true);

        Assert.AreEqual("ABC", result);
    }

    [TestMethod]
    public void Decode_WithInsufficientData_ReturnsNull()
    {
        var data = new byte[] { 0x01 };

        var result = FitBaseType.UInt32.Decode(data, littleEndian: true);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Decode_UInt8_WithInvalidSentinel_ReturnsNull()
    {
        var data = new byte[] { 0xFF };

        var result = FitBaseType.UInt8.Decode(data, littleEndian: true);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Decode_UInt8_WithValidValue_ReturnsLong()
    {
        var data = new byte[] { 0x2A };

        var result = FitBaseType.UInt8.Decode(data, littleEndian: true);

        Assert.AreEqual(42L, result);
    }

    [TestMethod]
    public void Decode_SInt8_WithNegativeValue_ReturnsLong()
    {
        var data = new byte[] { unchecked((byte)-5) };

        var result = FitBaseType.SInt8.Decode(data, littleEndian: true);

        Assert.AreEqual(-5L, result);
    }

    [TestMethod]
    public void Decode_UInt16_LittleEndian_ReturnsLong()
    {
        var data = new byte[2];
        BinaryPrimitives.WriteUInt16LittleEndian(data, 1234);

        var result = FitBaseType.UInt16.Decode(data, littleEndian: true);

        Assert.AreEqual(1234L, result);
    }

    [TestMethod]
    public void Decode_UInt16_BigEndian_ReturnsLong()
    {
        var data = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(data, 1234);

        var result = FitBaseType.UInt16.Decode(data, littleEndian: false);

        Assert.AreEqual(1234L, result);
    }

    [TestMethod]
    public void Decode_SInt16_WithNegativeValue_ReturnsLong()
    {
        var data = new byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(data, -1234);

        var result = FitBaseType.SInt16.Decode(data, littleEndian: true);

        Assert.AreEqual(-1234L, result);
    }

    [TestMethod]
    public void Decode_UInt32_LittleEndian_ReturnsLong()
    {
        var data = new byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(data, 123456789);

        var result = FitBaseType.UInt32.Decode(data, littleEndian: true);

        Assert.AreEqual(123456789L, result);
    }

    [TestMethod]
    public void Decode_UInt32_BigEndian_ReturnsLong()
    {
        var data = new byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(data, 123456789);

        var result = FitBaseType.UInt32.Decode(data, littleEndian: false);

        Assert.AreEqual(123456789L, result);
    }

    [TestMethod]
    public void Decode_SInt32_WithNegativeValue_ReturnsLong()
    {
        var data = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(data, -123456789);

        var result = FitBaseType.SInt32.Decode(data, littleEndian: true);

        Assert.AreEqual(-123456789L, result);
    }

    [TestMethod]
    public void Decode_SInt64_WithNegativeValue_ReturnsLong()
    {
        var data = new byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(data, -123456789012345);

        var result = FitBaseType.SInt64.Decode(data, littleEndian: true);

        Assert.AreEqual(-123456789012345L, result);
    }

    [TestMethod]
    public void Decode_UInt64_LittleEndian_ReturnsUlong()
    {
        var data = new byte[8];
        BinaryPrimitives.WriteUInt64LittleEndian(data, 123456789012345UL);

        var result = FitBaseType.UInt64.Decode(data, littleEndian: true);

        Assert.AreEqual(123456789012345UL, result);
    }

    [TestMethod]
    public void Decode_UInt64_BigEndian_ReturnsUlong()
    {
        var data = new byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(data, 123456789012345UL);

        var result = FitBaseType.UInt64.Decode(data, littleEndian: false);

        Assert.AreEqual(123456789012345UL, result);
    }

    [TestMethod]
    public void Decode_Float32_ReturnsFloat()
    {
        var data = new byte[4];
        BitConverter.TryWriteBytes(data, 3.14f);
        if (!BitConverter.IsLittleEndian)
        {
            data.AsSpan().Reverse();
        }

        var result = FitBaseType.Float32.Decode(data, littleEndian: true);

        Assert.AreEqual(3.14f, result);
    }

    [TestMethod]
    public void Decode_Float64_ReturnsDouble()
    {
        var data = new byte[8];
        BitConverter.TryWriteBytes(data, 3.14159);
        if (!BitConverter.IsLittleEndian)
        {
            data.AsSpan().Reverse();
        }

        var result = FitBaseType.Float64.Decode(data, littleEndian: true);

        Assert.AreEqual(3.14159, result);
    }

    [TestMethod]
    public void Decode_Byte_WithValue_ReturnsLong()
    {
        var data = new byte[] { 0x7B };

        var result = FitBaseType.Byte.Decode(data, littleEndian: true);

        Assert.AreEqual(123L, result);
    }

    [TestMethod]
    public void Decode_UInt32z_WithInvalidSentinel_ReturnsNull()
    {
        var data = new byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(data, 0xFFFFFFFF);

        var result = FitBaseType.UInt32z.Decode(data, littleEndian: true);

        Assert.IsNull(result);
    }
}
