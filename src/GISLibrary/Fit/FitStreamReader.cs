using System.Text;

namespace Tudormobile.GIS.Fit;

/// <summary>
/// Reads FIT protocol data (header and records) from a stream.
/// </summary>
/// <remarks>
/// Instances of this type are <b>not thread-safe</b>. It maintains sequential read state
/// (stream position, cached header, and eventually record cursor) and must be used by a
/// single thread/consumer at a time. Concurrent calls from multiple threads can interleave
/// reads against the underlying <see cref="Stream"/> and produce corrupted or inconsistent results.
/// </remarks>
public class FitStreamReader : BinaryReader
{
    private readonly string DEFAULT_NAME = "No Name";
    private FitHeader? _fitHeader;
    private string? _friendlyName;

    /// <summary>
    /// Cached FIT header, populated on the first call to <see cref="ReadHeader"/> or <see cref="ReadHeaderAsync"/>.
    /// </summary>
    public FitHeader Header => _fitHeader ?? ReadHeader();

    /// <summary>
    /// The 'FirendlyName' field from the (optional) User Profile record
    /// </summary>
    public string FriendlyName => _friendlyName ?? DEFAULT_NAME;

    /// <summary>
    /// Initializes a new instance of the <see cref="FitStreamReader"/> class based on the specified stream.
    /// </summary>
    /// <param name="input">The input stream containing raw (uncompressed) FIT data.</param>
    public FitStreamReader(Stream input) : base(input) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FitStreamReader"/> class based on the specified stream and character encoding.
    /// </summary>
    /// <param name="input">The input stream containing raw (uncompressed) FIT data.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public FitStreamReader(Stream input, Encoding encoding) : base(input, encoding) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FitStreamReader"/> class based on the specified stream and character encoding,
    /// and optionally leaves the stream open.
    /// </summary>
    /// <param name="input">The input stream containing raw (uncompressed) FIT data.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="leaveOpen"><c>true</c> to leave the stream open after the <see cref="FitStreamReader"/> is disposed; otherwise, <c>false</c>.</param>
    public FitStreamReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

    /// <summary>
    /// Gzip magic number: the first two bytes of a gzip-compressed stream.
    /// </summary>
    private const byte GZipMagicByte0 = 0x1F;
    private const byte GZipMagicByte1 = 0x8B;

    /// <summary>
    /// Creates a <see cref="FitStreamReader"/> from a stream that may or may not be gzip-compressed,
    /// without requiring the stream to support seeking. This allows use with non-seekable sources
    /// such as network/request bodies (e.g. an ASP.NET Core <c>HttpContext.Request.Body</c> stream).
    /// </summary>
    /// <param name="input">The source stream, which may be positioned at the start of raw FIT data or gzip-compressed FIT data.</param>
    /// <param name="leaveOpen"><c>true</c> to leave <paramref name="input"/> open after the reader is disposed.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="FitStreamReader"/> that transparently decompresses the data if needed.</returns>
    public static async Task<FitStreamReader> CreateAsync(Stream input, bool leaveOpen = false, CancellationToken cancellationToken = default)
    {
        var prefix = new byte[2];
        var bytesRead = await input.ReadAtLeastAsync(prefix, prefix.Length, throwOnEndOfStream: false, cancellationToken).ConfigureAwait(false);

        // Re-present the peeked bytes ahead of the remaining stream, since we may not be able to seek back.
        Stream stream = new PrefixedStream(prefix.AsMemory(0, bytesRead), input, leaveOpen);

        if (bytesRead == 2 && prefix[0] == GZipMagicByte0 && prefix[1] == GZipMagicByte1)
        {
            stream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
        }

        return new FitStreamReader(stream);
    }

    /// <summary>
    /// A stream that yields a small in-memory prefix before delegating remaining reads to an inner stream.
    /// Used to "un-peek" bytes read to sniff a stream's format when the underlying stream may not support seeking.
    /// </summary>
    private sealed class PrefixedStream(ReadOnlyMemory<byte> prefix, Stream inner, bool leaveOpen) : Stream
    {
        /// <summary>
        /// Remaining, not-yet-consumed bytes from the original peeked prefix.
        /// </summary>
        private ReadOnlyMemory<byte> _prefix = prefix;

        /// <inheritdoc/>
        public override bool CanRead => true;
        /// <inheritdoc/>
        public override bool CanSeek => false;
        /// <inheritdoc/>
        public override bool CanWrite => false;
        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException();
        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
            => Read(buffer.AsSpan(offset, count));

        /// <summary>
        /// Reads bytes into <paramref name="buffer"/>, first draining any remaining prefix bytes before
        /// delegating to the inner stream.
        /// </summary>
        /// <param name="buffer">The destination buffer to fill.</param>
        /// <returns>The number of bytes read.</returns>
        public override int Read(Span<byte> buffer)
        {
            if (!_prefix.IsEmpty)
            {
                var toCopy = Math.Min(_prefix.Length, buffer.Length);
                _prefix.Span[..toCopy].CopyTo(buffer);
                _prefix = _prefix[toCopy..];
                return toCopy;
            }
            return inner.Read(buffer);
        }

        /// <summary>
        /// Asynchronously reads bytes into <paramref name="buffer"/>, first draining any remaining prefix bytes
        /// before delegating to the inner stream.
        /// </summary>
        /// <param name="buffer">The destination buffer to fill.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task whose result is the number of bytes read.</returns>
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (!_prefix.IsEmpty)
            {
                var toCopy = Math.Min(_prefix.Length, buffer.Length);
                _prefix[..toCopy].CopyTo(buffer);
                _prefix = _prefix[toCopy..];
                return toCopy;
            }
            return await inner.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

        /// <inheritdoc/>
        public override void Flush() => throw new NotSupportedException();
        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        /// <inheritdoc/>
        public override void SetLength(long value) => throw new NotSupportedException();
        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <summary>
        /// Releases the resources used by this stream, optionally disposing the inner stream
        /// unless the caller requested it be left open.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !leaveOpen)
            {
                inner.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Reads and returns the FIT file header, decoding it from the underlying stream on first call
    /// and returning the cached instance on subsequent calls.
    /// </summary>
    /// <returns>The parsed <see cref="FitHeader"/>.</returns>
    /// <exception cref="FormatException">Thrown when the header cannot be parsed as valid FIT data.</exception>
    /// <remarks>Not thread-safe. Must not be called concurrently with other reads on this instance.</remarks>
    public FitHeader ReadHeader()
    {
        if (_fitHeader == null)
        {
            var buffer = ReadBytes(FitHeader.MinimumSize);
            var header = new FitHeader(buffer);

            var remaining = header.HeaderSize - buffer.Length;
            if (remaining > 0)
            {
                // Advance past any additional header bytes (e.g. future extensions) via
                // a forward-only read, since the underlying stream may not support seeking.
                var extra = ReadBytes(remaining);
                if (extra.Length != remaining)
                {
                    throw new FormatException("Invalid FIT data format.");
                }
            }

            _fitHeader = header;
        }
        return _fitHeader ?? throw new FormatException("Invalid FIT data format.");
    }

    /// <summary>
    /// Asynchronously reads and returns the FIT file header, decoding it from the underlying stream on first call
    /// and returning the cached instance on subsequent calls.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task whose result is the parsed <see cref="FitHeader"/>.</returns>
    /// <exception cref="FormatException">Thrown when the header cannot be parsed as valid FIT data.</exception>
    /// <remarks>Not thread-safe. Must not be called concurrently with other reads on this instance.</remarks>
    public async Task<FitHeader> ReadHeaderAsync(CancellationToken cancellationToken = default)
    {
        if (_fitHeader == null)
        {
            var buffer = new byte[FitHeader.MinimumSize];
            await BaseStream.ReadExactlyAsync(buffer.AsMemory(0, buffer.Length), cancellationToken).ConfigureAwait(false);
            var header = new FitHeader(buffer);

            var remaining = header.HeaderSize - buffer.Length;
            if (remaining > 0)
            {
                // Advance past any additional header bytes (e.g. future extensions) via
                // a forward-only read, since the underlying stream may not support seeking.
                var extra = new byte[remaining];
                await BaseStream.ReadExactlyAsync(extra.AsMemory(0, remaining), cancellationToken).ConfigureAwait(false);
            }

            _fitHeader = header;
        }
        return _fitHeader ?? throw new FormatException("Invalid FIT data format.");
    }

    /// <summary>
    /// Bit mask isolating the message type bit (Definition vs. Data) of a normal record header.
    /// </summary>
    private const byte RecordHeaderDefinitionMask = 0x40;

    /// <summary>
    /// Bit mask isolating the compressed-timestamp-header bit of a record header.
    /// </summary>
    private const byte RecordHeaderCompressedTimestampMask = 0x80;

    /// <summary>
    /// Bit mask isolating the local message type bits of a normal record header.
    /// </summary>
    private const byte RecordHeaderLocalMessageTypeMask = 0x0F;

    /// <summary>
    /// Bit mask isolating the local message type bits of a compressed-timestamp record header.
    /// </summary>
    private const byte CompressedTimestampLocalMessageTypeMask = 0x60;

    /// <summary>
    /// Bit mask isolating the time-offset bits of a compressed-timestamp record header.
    /// </summary>
    private const byte CompressedTimestampTimeOffsetMask = 0x1F;

    /// <summary>
    /// Bit mask isolating the "developer data present" bit of a Definition Message's record header.
    /// </summary>
    private const byte RecordHeaderDeveloperDataMask = 0x20;

    /// <summary>
    /// The FIT global message number for the "user_profile" message.
    /// </summary>
    private const ushort UserProfileGlobalMessageNumber = 3;

    /// <summary>
    /// The "friendly_name" field number within the "user_profile" message.
    /// </summary>
    private const byte UserProfileFriendlyNameField = 0;

    /// <summary>
    /// Inspects a Data Message and, if it is a "user_profile" (3) message containing a
    /// "friendly_name" (0) field, caches the decoded value for exposure via <see cref="FriendlyName"/>.
    /// </summary>
    private void UpdateFriendlyName(FitRecord record)
    {
        if (record.GlobalMessageNumber != UserProfileGlobalMessageNumber || record.FieldDefinitions == null)
        {
            return;
        }

        var data = record.Data.Span;
        var offset = 0;
        foreach (var field in record.FieldDefinitions)
        {
            if (offset + field.Size > data.Length)
            {
                break;
            }

            if (field.FieldDefinitionNumber == UserProfileFriendlyNameField)
            {
                if (((FitBaseType)field.BaseType).Decode(data.Slice(offset, field.Size), record.LittleEndian) is string friendlyName
                    && !string.IsNullOrEmpty(friendlyName))
                {
                    _friendlyName = friendlyName;
                }
                break;
            }

            offset += field.Size;
        }
    }

    /// <summary>
    /// Asynchronously reads and yields the FIT records (Definition and Data messages) that follow the header,
    /// reading the header first via <see cref="ReadHeaderAsync"/> if it has not already been read.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>An asynchronous sequence of the <see cref="FitRecord"/> instances contained in the file's data section.</returns>
    /// <exception cref="FormatException">Thrown when the record stream cannot be parsed as valid FIT data.</exception>
    /// <remarks>
    /// The trailing 2-byte CRC that follows the data section is also consumed from the stream, so that
    /// a subsequent call could, in principle, continue reading a chained FIT file that follows it.
    /// Not thread-safe. Must not be called concurrently with other reads on this instance.
    /// </remarks>
    public async IAsyncEnumerable<FitRecord> ReadRecordsAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var header = _fitHeader ?? await ReadHeaderAsync(cancellationToken).ConfigureAwait(false);

        var definitions = new Dictionary<byte, (bool LittleEndian, int TotalSize, ushort GlobalMessageNumber, IReadOnlyList<FitFieldDefinition> Fields)>();
        var buffer = new byte[1];
        long remaining = header.DataSize;

        while (remaining > 0)
        {
            await ReadExactAsync(buffer.AsMemory(0, 1), cancellationToken).ConfigureAwait(false);
            remaining -= 1;
            var recordHeader = buffer[0];

            if ((recordHeader & RecordHeaderCompressedTimestampMask) != 0)
            {
                var localMessageType = (byte)((recordHeader & CompressedTimestampLocalMessageTypeMask) >> 5);
                var timeOffset = (byte)(recordHeader & CompressedTimestampTimeOffsetMask);

                if (!definitions.TryGetValue(localMessageType, out var definition))
                {
                    throw new FormatException($"FIT data message references undefined local message type {localMessageType}.");
                }

                var data = new byte[definition.TotalSize];
                await ReadExactAsync(data.AsMemory(0, data.Length), cancellationToken).ConfigureAwait(false);
                remaining -= data.Length;

                var compressedTimestampRecord = new FitRecord
                {
                    MessageType = FitMessageType.Data,
                    LocalMessageType = localMessageType,
                    IsCompressedTimestamp = true,
                    TimeOffset = timeOffset,
                    GlobalMessageNumber = definition.GlobalMessageNumber,
                    LittleEndian = definition.LittleEndian,
                    FieldDefinitions = definition.Fields,
                    Data = data,
                };
                UpdateFriendlyName(compressedTimestampRecord);
                yield return compressedTimestampRecord;
            }
            else
            {
                var localMessageType = (byte)(recordHeader & RecordHeaderLocalMessageTypeMask);
                var isDefinition = (recordHeader & RecordHeaderDefinitionMask) != 0;

                if (isDefinition)
                {
                    var defHeader = new byte[5];
                    await ReadExactAsync(defHeader.AsMemory(0, defHeader.Length), cancellationToken).ConfigureAwait(false);
                    remaining -= defHeader.Length;

                    var littleEndian = defHeader[1] == 0;
                    var globalMessageNumber = littleEndian
                        ? (ushort)(defHeader[2] | (defHeader[3] << 8))
                        : (ushort)((defHeader[2] << 8) | defHeader[3]);
                    var fieldCount = defHeader[4];

                    var fieldBytes = new byte[fieldCount * 3];
                    if (fieldBytes.Length > 0)
                    {
                        await ReadExactAsync(fieldBytes.AsMemory(0, fieldBytes.Length), cancellationToken).ConfigureAwait(false);
                        remaining -= fieldBytes.Length;
                    }

                    var fields = new FitFieldDefinition[fieldCount];
                    var totalSize = 0;
                    for (var i = 0; i < fieldCount; i++)
                    {
                        var fieldDefinitionNumber = fieldBytes[i * 3];
                        var size = fieldBytes[(i * 3) + 1];
                        var baseType = fieldBytes[(i * 3) + 2];
                        fields[i] = new FitFieldDefinition(fieldDefinitionNumber, size, baseType);
                        totalSize += size;
                    }

                    IReadOnlyList<FitDeveloperFieldDefinition>? developerFields = null;
                    var hasDeveloperData = (recordHeader & RecordHeaderDeveloperDataMask) != 0;

                    if (hasDeveloperData)
                    {
                        var devCountBuffer = new byte[1];
                        await ReadExactAsync(devCountBuffer.AsMemory(0, 1), cancellationToken).ConfigureAwait(false);
                        remaining -= 1;
                        var devFieldCount = devCountBuffer[0];

                        var devFieldBytes = new byte[devFieldCount * 3];
                        if (devFieldBytes.Length > 0)
                        {
                            await ReadExactAsync(devFieldBytes.AsMemory(0, devFieldBytes.Length), cancellationToken).ConfigureAwait(false);
                            remaining -= devFieldBytes.Length;
                        }

                        var devFields = new FitDeveloperFieldDefinition[devFieldCount];
                        for (var i = 0; i < devFieldCount; i++)
                        {
                            var fieldDefinitionNumber = devFieldBytes[i * 3];
                            var size = devFieldBytes[(i * 3) + 1];
                            var developerDataIndex = devFieldBytes[(i * 3) + 2];
                            devFields[i] = new FitDeveloperFieldDefinition(fieldDefinitionNumber, size, developerDataIndex);
                            totalSize += size;
                        }
                        developerFields = devFields;
                    }

                    definitions[localMessageType] = (littleEndian, totalSize, globalMessageNumber, fields);

                    yield return new FitRecord
                    {
                        MessageType = FitMessageType.Definition,
                        LocalMessageType = localMessageType,
                        GlobalMessageNumber = globalMessageNumber,
                        LittleEndian = littleEndian,
                        FieldDefinitions = fields,
                        DeveloperFieldDefinitions = developerFields,
                    };
                }
                else
                {
                    if (!definitions.TryGetValue(localMessageType, out var definition))
                    {
                        throw new FormatException($"FIT data message references undefined local message type {localMessageType}.");
                    }

                    var data = new byte[definition.TotalSize];
                    if (data.Length > 0)
                    {
                        await ReadExactAsync(data.AsMemory(0, data.Length), cancellationToken).ConfigureAwait(false);
                        remaining -= data.Length;
                    }

                    var dataRecord = new FitRecord
                    {
                        MessageType = FitMessageType.Data,
                        LocalMessageType = localMessageType,
                        GlobalMessageNumber = definition.GlobalMessageNumber,
                        LittleEndian = definition.LittleEndian,
                        FieldDefinitions = definition.Fields,
                        Data = data,
                    };
                    UpdateFriendlyName(dataRecord);
                    yield return dataRecord;
                }
            }
        }

        // Consume the trailing 2-byte CRC that follows the data section.
        var crc = new byte[2];
        await ReadExactAsync(crc.AsMemory(0, crc.Length), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads exactly <paramref name="buffer"/>.Length bytes from the underlying stream, throwing if the
    /// stream ends before the buffer is filled.
    /// </summary>
    private async Task ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    {
        try
        {
            await BaseStream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        catch (EndOfStreamException ex)
        {
            throw new FormatException("Invalid FIT data format.", ex);
        }
    }
}
