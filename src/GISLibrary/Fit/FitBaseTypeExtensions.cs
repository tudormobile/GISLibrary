namespace Tudormobile.GIS.Fit;

/// <summary>
/// Helpers for decoding raw FIT field bytes according to their <see cref="FitBaseType"/>.
/// </summary>
public static class FitBaseTypeExtensions
{
    /// <summary>
    /// Gets the size, in bytes, of a single element of the given base type (excluding <see cref="FitBaseType.String"/>,
    /// whose length is determined by the field's declared size instead).
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="baseType"/> is not a recognized FIT base type.</exception>
    public static int GetElementSize(this FitBaseType baseType) => baseType switch
    {
        FitBaseType.Enum or FitBaseType.SInt8 or FitBaseType.UInt8 or FitBaseType.UInt8z or FitBaseType.Byte or FitBaseType.String => 1,
        FitBaseType.SInt16 or FitBaseType.UInt16 or FitBaseType.UInt16z => 2,
        FitBaseType.SInt32 or FitBaseType.UInt32 or FitBaseType.UInt32z or FitBaseType.Float32 => 4,
        FitBaseType.SInt64 or FitBaseType.UInt64 or FitBaseType.UInt64z or FitBaseType.Float64 => 8,
        _ => throw new NotSupportedException($"Unrecognized FIT base type: 0x{(byte)baseType:X2}."),
    };

    /// <summary>
    /// The raw value that represents "invalid"/"not set" for the given base type, per the FIT protocol.
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="baseType"/> is not a recognized FIT base type.</exception>
    public static ulong GetInvalidValue(this FitBaseType baseType) => baseType switch
    {
        FitBaseType.Enum or FitBaseType.UInt8 or FitBaseType.UInt8z or FitBaseType.Byte => 0xFF,
        FitBaseType.SInt8 => 0x7F,
        FitBaseType.UInt16 or FitBaseType.UInt16z => 0xFFFF,
        FitBaseType.SInt16 => 0x7FFF,
        FitBaseType.UInt32 or FitBaseType.UInt32z => 0xFFFFFFFF,
        FitBaseType.SInt32 => 0x7FFFFFFF,
        FitBaseType.UInt64 or FitBaseType.UInt64z => 0xFFFFFFFFFFFFFFFF,
        FitBaseType.SInt64 => 0x7FFFFFFFFFFFFFFF,
        FitBaseType.String or FitBaseType.Float32 or FitBaseType.Float64 => 0,
        _ => throw new NotSupportedException($"Unrecognized FIT base type: 0x{(byte)baseType:X2}."),
    };

    /// <summary>
    /// Decodes a single field value from raw bytes according to the given base type and byte order.
    /// </summary>
    /// <param name="baseType">The FIT base type describing how to interpret the bytes.</param>
    /// <param name="data">The raw field bytes, exactly the declared size of the field.</param>
    /// <param name="littleEndian">Whether the bytes are encoded in little-endian byte order.</param>
    /// <returns>
    /// The decoded value as <see cref="long"/> (integer types), <see cref="ulong"/> (unsigned 64-bit),
    /// <see cref="float"/>/<see cref="double"/> (floating point), <see cref="string"/> (string type).
    /// For non-string types, returns <c>null</c> when the value represents the type's "invalid" marker.
    /// </returns>
    public static object? Decode(this FitBaseType baseType, ReadOnlySpan<byte> data, bool littleEndian)
    {
        if (baseType == FitBaseType.String)
        {
            var nullIndex = data.IndexOf((byte)0);
            var text = System.Text.Encoding.ASCII.GetString(nullIndex >= 0 ? data[..nullIndex] : data);
            return text;
        }

        var elementSize = baseType.GetElementSize();
        if (data.Length < elementSize)
        {
            return null;
        }

        ulong raw = elementSize switch
        {
            1 => data[0],
            2 => littleEndian
                ? System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(data)
                : System.Buffers.Binary.BinaryPrimitives.ReadUInt16BigEndian(data),
            4 => littleEndian
                ? System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(data)
                : System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(data),
            8 => littleEndian
                ? System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(data)
                : System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(data),
            _ => throw new NotSupportedException($"Unexpected element size {elementSize} for FIT base type {baseType}."),
        };

        if (raw == baseType.GetInvalidValue())
        {
            return null;
        }

        return baseType switch
        {
            FitBaseType.SInt8 => (object)(long)(sbyte)raw,
            FitBaseType.SInt16 => (object)(long)(short)raw,
            FitBaseType.SInt32 => (object)(long)(int)raw,
            FitBaseType.SInt64 => (object)(long)raw,
            FitBaseType.Float32 => (object)BitConverter.Int32BitsToSingle((int)raw),
            FitBaseType.Float64 => (object)BitConverter.Int64BitsToDouble((long)raw),
            FitBaseType.UInt64 or FitBaseType.UInt64z => (object)raw,
            _ => (object)(long)raw,
        };
    }
}
