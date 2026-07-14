namespace Tudormobile.GIS.Fit;

/// <summary>
/// Represents a single decoded FIT protocol record, which is either a Definition Message
/// (describing the layout of a local message type) or a Data Message (containing raw field bytes).
/// </summary>
/// <remarks>
/// Field-level semantic decoding (e.g. mapping raw bytes to named, typed values per the FIT global
/// profile) is intentionally not performed here; <see cref="Data"/> exposes the raw, undecoded bytes
/// for Data Messages so that callers can interpret them using the associated Definition Message.
/// </remarks>
public sealed class FitRecord
{
    /// <summary>
    /// Whether this record is a Definition Message or a Data Message.
    /// </summary>
    public required FitMessageType MessageType { get; init; }

    /// <summary>
    /// The local message type (0-15, or 0-3 for compressed timestamp headers) that this record
    /// is associated with. For Data Messages, this identifies the Definition Message that describes
    /// this record's layout.
    /// </summary>
    public required byte LocalMessageType { get; init; }

    /// <summary>
    /// Whether this record used a compressed timestamp header. Only Data Messages can use this form.
    /// </summary>
    public bool IsCompressedTimestamp { get; init; }

    /// <summary>
    /// The 5-bit time offset (in seconds) encoded in a compressed timestamp header, if
    /// <see cref="IsCompressedTimestamp"/> is <c>true</c>; otherwise <c>null</c>.
    /// </summary>
    public byte? TimeOffset { get; init; }

    /// <summary>
    /// The FIT global message number (per the FIT global profile) for a Definition Message.
    /// Only populated when <see cref="MessageType"/> is <see cref="FitMessageType.Definition"/>.
    /// </summary>
    public ushort? GlobalMessageNumber { get; init; }

    /// <summary>
    /// Whether the fields described by a Definition Message are encoded using little-endian byte order.
    /// Only meaningful when <see cref="MessageType"/> is <see cref="FitMessageType.Definition"/>.
    /// </summary>
    public bool LittleEndian { get; init; } = true;

    /// <summary>
    /// The field definitions declared by a Definition Message.
    /// Only populated when <see cref="MessageType"/> is <see cref="FitMessageType.Definition"/>.
    /// </summary>
    public IReadOnlyList<FitFieldDefinition>? FieldDefinitions { get; init; }

    /// <summary>
    /// The developer field definitions declared by a Definition Message, if any.
    /// Only populated when <see cref="MessageType"/> is <see cref="FitMessageType.Definition"/>
    /// and the message declared developer data fields.
    /// </summary>
    public IReadOnlyList<FitDeveloperFieldDefinition>? DeveloperFieldDefinitions { get; init; }

    /// <summary>
    /// The raw, undecoded field bytes for a Data Message, laid out according to the corresponding
    /// Definition Message. Empty for Definition Messages.
    /// </summary>
    public ReadOnlyMemory<byte> Data { get; init; }
}
