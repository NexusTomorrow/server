using System.Buffers;
using System.Buffers.Binary;

namespace NexusTomorrow.Transport;

/// <summary>
/// What <see cref="FrameReader.TryRead"/> found at the start of a buffer.
/// </summary>
public enum FrameStatus
{
    /// <summary>
    /// The buffer holds less than one frame; read more.
    /// </summary>
    Incomplete,

    /// <summary>
    /// One frame was taken from the buffer.
    /// </summary>
    Complete,

    /// <summary>
    /// The frame's length is shorter than its header or longer than the cap; the connection is hostile or broken.
    /// </summary>
    Malformed,
}

/// <summary>
/// Takes frames from received bytes: a 32-bit little-endian length that counts itself, a 16-bit little-endian opcode, then the body.
/// </summary>
public static class FrameReader
{
    /// <summary>
    /// The bytes before a frame's body: the 32-bit length and the 16-bit opcode.
    /// </summary>
    public const int HeaderBytes = 6;

    private const int LengthBytes = 4;

    /// <summary>
    /// Take one frame from the start of <paramref name="buffer"/>, advancing it past the frame. The length is judged as soon as its four
    /// bytes arrive, so an oversized frame is refused before its body is buffered.
    /// </summary>
    public static FrameStatus TryRead(ref ReadOnlySequence<byte> buffer, int maxFrameBytes, out ushort opcode, out ReadOnlySequence<byte> body)
    {
        opcode = 0;
        body = default;
        if (buffer.Length < LengthBytes)
            return FrameStatus.Incomplete;

        Span<byte> header = stackalloc byte[HeaderBytes];
        buffer.Slice(0, LengthBytes).CopyTo(header);
        uint length = BinaryPrimitives.ReadUInt32LittleEndian(header);
        if (length < HeaderBytes || length > (uint)maxFrameBytes)
            return FrameStatus.Malformed;
        if (buffer.Length < length)
            return FrameStatus.Incomplete;

        buffer.Slice(LengthBytes, HeaderBytes - LengthBytes).CopyTo(header[LengthBytes..]);
        opcode = BinaryPrimitives.ReadUInt16LittleEndian(header[LengthBytes..]);
        body = buffer.Slice(HeaderBytes, length - HeaderBytes);
        buffer = buffer.Slice(length);
        return FrameStatus.Complete;
    }
}
