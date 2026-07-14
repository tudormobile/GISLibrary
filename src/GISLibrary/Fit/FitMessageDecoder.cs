namespace Tudormobile.GIS.Fit;

/// <summary>
/// Decodes the raw, undecoded bytes of a <see cref="FitRecord"/> Data Message into typed values,
/// using its associated Definition Message field metadata.
/// </summary>
/// <remarks>
/// This decoder intentionally supports only the small subset of the FIT global message profile
/// relevant to this library's purpose: extracting a lat/lon/altitude/time track from a FIT file.
/// Messages other than the "record" (20) global message are not semantically decoded.
/// </remarks>
public static class FitMessageDecoder
{
    /// <summary>
    /// The FIT global message number for the "record" message (per-sample GPS/sensor data).
    /// </summary>
    public const ushort RecordGlobalMessageNumber = 20;

    private const byte FieldPositionLat = 0;
    private const byte FieldPositionLong = 1;
    private const byte FieldAltitude = 2;
    private const byte FieldTimestamp = 253;
    private const byte FieldEnhancedAltitude = 78;

    /// <summary>
    /// The FIT epoch (UTC), used to convert "timestamp" field values (seconds since this epoch) to <see cref="DateTime"/>.
    /// </summary>
    private static readonly DateTime FitEpoch = new(1989, 12, 31, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Scale factor used to convert FIT "semicircle" units to decimal degrees (2^31 / 180).
    /// </summary>
    private const double SemicirclesPerDegree = 11930464.7111;

    /// <summary>
    /// Determines whether the given <see cref="FitRecord"/> is a Data Message for the FIT "record" (20) global message.
    /// </summary>
    public static bool IsRecordMessage(this FitRecord record) =>
        record.MessageType == FitMessageType.Data && record.GlobalMessageNumber == RecordGlobalMessageNumber;

    /// <summary>
    /// Decodes a "record" (20) Data Message into a <see cref="FitTrackpoint"/>, extracting the
    /// timestamp, latitude, longitude, and altitude fields when present.
    /// </summary>
    /// <param name="record">A Data Message <see cref="FitRecord"/> whose <see cref="FitRecord.GlobalMessageNumber"/>
    /// is the "record" global message number (20).</param>
    /// <returns>The decoded <see cref="FitTrackpoint"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="record"/> is not a Data Message,
    /// or does not carry <see cref="FitRecord.FieldDefinitions"/> needed to decode it.</exception>
    public static FitTrackpoint DecodeTrackpoint(this FitRecord record)
    {
        if (record.MessageType != FitMessageType.Data)
        {
            throw new ArgumentException("Only Data Messages can be decoded.", nameof(record));
        }
        if (record.FieldDefinitions == null)
        {
            throw new ArgumentException("Record is missing field definitions required to decode it.", nameof(record));
        }

        DateTime? timestamp = null;
        double? latitude = null;
        double? longitude = null;
        double? altitude = null;

        var data = record.Data.Span;
        var offset = 0;

        foreach (var field in record.FieldDefinitions)
        {
            if (offset + field.Size > data.Length)
            {
                break;
            }

            var slice = data.Slice(offset, field.Size);
            offset += field.Size;

            switch (field.FieldDefinitionNumber)
            {
                case FieldTimestamp:
                    if (((FitBaseType)field.BaseType).Decode(slice, record.LittleEndian) is long secondsSinceEpoch)
                    {
                        timestamp = FitEpoch.AddSeconds(secondsSinceEpoch);
                    }
                    break;

                case FieldPositionLat:
                    if (((FitBaseType)field.BaseType).Decode(slice, record.LittleEndian) is long latSemicircles)
                    {
                        latitude = latSemicircles / SemicirclesPerDegree;
                    }
                    break;

                case FieldPositionLong:
                    if (((FitBaseType)field.BaseType).Decode(slice, record.LittleEndian) is long lonSemicircles)
                    {
                        longitude = lonSemicircles / SemicirclesPerDegree;
                    }
                    break;

                case FieldAltitude:
                    if (((FitBaseType)field.BaseType).Decode(slice, record.LittleEndian) is long altRaw)
                    {
                        // altitude = (raw / 5) - 500, per the FIT global profile ("altitude" field, units: m).
                        altitude ??= (altRaw / 5.0) - 500.0;
                    }
                    break;

                case FieldEnhancedAltitude:
                    if (((FitBaseType)field.BaseType).Decode(slice, record.LittleEndian) is long enhancedAltRaw)
                    {
                        // enhanced_altitude uses the same scale/offset as altitude, but with greater range.
                        altitude = (enhancedAltRaw / 5.0) - 500.0;
                    }
                    break;
            }
        }

        FitPosition? position = latitude.HasValue && longitude.HasValue
            ? new FitPosition(latitude.Value, longitude.Value, altitude)
            : null;

        return new FitTrackpoint(timestamp, position);
    }
}
