namespace Tudormobile.GIS.Fit;

/// <summary>
/// FIT protocol base type identifiers, as declared in Definition Message field entries.
/// The numeric values match the raw base type byte defined by the FIT protocol
/// (bit 7 marks the type as endian-sensitive; bits 0-4 identify the type).
/// </summary>
public enum FitBaseType : byte
{
    /// <summary>1-byte enumerated value.</summary>
    Enum = 0x00,
    /// <summary>1-byte signed integer.</summary>
    SInt8 = 0x01,
    /// <summary>1-byte unsigned integer.</summary>
    UInt8 = 0x02,
    /// <summary>2-byte signed integer.</summary>
    SInt16 = 0x83,
    /// <summary>2-byte unsigned integer.</summary>
    UInt16 = 0x84,
    /// <summary>4-byte signed integer.</summary>
    SInt32 = 0x85,
    /// <summary>4-byte unsigned integer.</summary>
    UInt32 = 0x86,
    /// <summary>Null-terminated ASCII string.</summary>
    String = 0x07,
    /// <summary>4-byte IEEE-754 floating point value.</summary>
    Float32 = 0x88,
    /// <summary>8-byte IEEE-754 floating point value.</summary>
    Float64 = 0x89,
    /// <summary>1-byte unsigned integer, where 0 represents an invalid/unset value.</summary>
    UInt8z = 0x0A,
    /// <summary>2-byte unsigned integer, where 0 represents an invalid/unset value.</summary>
    UInt16z = 0x8B,
    /// <summary>4-byte unsigned integer, where 0 represents an invalid/unset value.</summary>
    UInt32z = 0x8C,
    /// <summary>1-byte raw binary data.</summary>
    Byte = 0x0D,
    /// <summary>8-byte signed integer.</summary>
    SInt64 = 0x8E,
    /// <summary>8-byte unsigned integer.</summary>
    UInt64 = 0x8F,
    /// <summary>8-byte unsigned integer, where 0 represents an invalid/unset value.</summary>
    UInt64z = 0x90,
}
