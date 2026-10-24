using System.Buffers;
using System.Buffers.Binary;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Transport;

/// <summary>
/// Writes frames: bare messages, enveloped messages and transport pings, from a message or from a body already serialized into a buffer
/// rented by <see cref="RentBody"/>. The frame length is always written at its full 32-bit width.
/// </summary>
public static class FrameWriter
{
    /// <summary>
    /// Serialize <paramref name="message"/>'s body into a buffer rented from <see cref="ArrayPool{T}.Shared"/> and return the buffer;
    /// <paramref name="length"/> is the body's byte count. The message is not referenced after this returns, so its sender may change
    /// or reuse it. The caller owns the buffer and returns it to the pool exactly once, with <c>clearArray: true</c>.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured; the buffer is already returned.</exception>
    public static byte[] RentBody(IWireStruct message, out int length)
    {
        length = BodyBytes(message);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            WriteBody(buffer, message, length);
        }
        catch
        {
            // Cleared like every returned send buffer: a returned buffer holds no message bytes.
            ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
            throw;
        }

        return buffer;
    }

    /// <summary>
    /// Write <paramref name="message"/> as a bare frame and return the bytes written.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured.</exception>
    public static int WriteMessage(IBufferWriter<byte> output, ushort opcode, IWireStruct message)
    {
        int bodyBytes = BodyBytes(message);
        Span<byte> frame = BeginMessage(output, opcode, bodyBytes);
        WriteBody(frame[FrameReader.HeaderBytes..], message, bodyBytes);
        output.Advance(frame.Length);
        return frame.Length;
    }

    /// <summary>
    /// Write an already serialized <paramref name="body"/> as a bare frame and return the bytes written.
    /// </summary>
    public static int WriteMessage(IBufferWriter<byte> output, ushort opcode, ReadOnlySpan<byte> body)
    {
        Span<byte> frame = BeginMessage(output, opcode, body.Length);
        body.CopyTo(frame[FrameReader.HeaderBytes..]);
        output.Advance(frame.Length);
        return frame.Length;
    }

    /// <summary>
    /// Write <paramref name="message"/> inside an envelope frame and return the bytes written. The envelope's body is the inner frame:
    /// its length in clear, then its opcode and body encrypted with <paramref name="cipher"/>.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured.</exception>
    public static int WriteEnvelope(IBufferWriter<byte> output, ushort envelopeOpcode, ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        int bodyBytes = BodyBytes(message);
        Span<byte> frame = BeginEnvelope(output, envelopeOpcode, opcode, bodyBytes, out Span<byte> inner);
        WriteBody(inner[FrameReader.HeaderBytes..], message, bodyBytes);
        cipher.Encrypt(inner[4..]);
        output.Advance(frame.Length);
        return frame.Length;
    }

    /// <summary>
    /// Write an already serialized <paramref name="body"/> inside an envelope frame, encrypted as the overload that takes a message
    /// does, and return the bytes written.
    /// </summary>
    public static int WriteEnvelope(IBufferWriter<byte> output, ushort envelopeOpcode, ushort opcode, ReadOnlySpan<byte> body, PacketCipher cipher)
    {
        Span<byte> frame = BeginEnvelope(output, envelopeOpcode, opcode, body.Length, out Span<byte> inner);
        body.CopyTo(inner[FrameReader.HeaderBytes..]);
        cipher.Encrypt(inner[4..]);
        output.Advance(frame.Length);
        return frame.Length;
    }

    /// <summary>
    /// Write a transport ping or ping answer: a bare frame whose body is the one index byte.
    /// </summary>
    public static void WritePing(IBufferWriter<byte> output, ushort opcode, byte index)
    {
        const int total = FrameReader.HeaderBytes + 1;
        Span<byte> frame = output.GetSpan(total)[..total];
        WriteHeader(frame, total, opcode);
        frame[FrameReader.HeaderBytes] = index;
        output.Advance(total);
    }

    // The frame span for a bare frame of bodyBytes, header written; the caller fills the body and advances.
    private static Span<byte> BeginMessage(IBufferWriter<byte> output, ushort opcode, int bodyBytes)
    {
        int total = FrameReader.HeaderBytes + bodyBytes;
        Span<byte> frame = output.GetSpan(total)[..total];
        WriteHeader(frame, total, opcode);
        return frame;
    }

    // The frame span for an envelope around an inner frame of bodyBytes, both headers written; the caller fills the inner body,
    // encrypts inner[4..] and advances.
    private static Span<byte> BeginEnvelope(IBufferWriter<byte> output, ushort envelopeOpcode, ushort opcode, int bodyBytes, out Span<byte> inner)
    {
        int innerBytes = FrameReader.HeaderBytes + bodyBytes;
        Span<byte> frame = BeginMessage(output, envelopeOpcode, innerBytes);
        inner = frame[FrameReader.HeaderBytes..];
        WriteHeader(inner, innerBytes, opcode);
        return frame;
    }

    private static void WriteHeader(Span<byte> frame, int length, ushort opcode)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(frame, (uint)length);
        BinaryPrimitives.WriteUInt16LittleEndian(frame[4..], opcode);
    }

    /// <summary>
    /// The byte count of <paramref name="message"/>'s body.
    /// </summary>
    public static int BodyBytes(IWireStruct message) => (message.MeasureBits() + 7) >> 3;

    /// <summary>
    /// Serialize <paramref name="message"/>'s body into the first <paramref name="bodyBytes"/> of <paramref name="body"/>, where
    /// <paramref name="bodyBytes"/> is the message's <see cref="BodyBytes"/>.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured.</exception>
    public static void WriteBody(Span<byte> body, IWireStruct message, int bodyBytes)
    {
        var writer = new BitWriter(body[..bodyBytes]);
        message.Write(ref writer);
        if (writer.BytesWritten != bodyBytes)
            throw new CodecException($"A message measured {bodyBytes} bytes and wrote {writer.BytesWritten}.");
    }
}
