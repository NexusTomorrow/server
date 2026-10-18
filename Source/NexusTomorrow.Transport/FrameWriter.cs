using System.Buffers;
using System.Buffers.Binary;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Transport;

/// <summary>
/// Writes frames: bare messages, enveloped messages and transport pings. The frame length is always written at its full 32-bit width.
/// </summary>
public static class FrameWriter
{
    /// <summary>
    /// Write <paramref name="message"/> as a bare frame and return the bytes written.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured.</exception>
    public static int WriteMessage(IBufferWriter<byte> output, ushort opcode, IWireStruct message)
    {
        int bodyBytes = BodyBytes(message);
        int total = FrameReader.HeaderBytes + bodyBytes;
        Span<byte> frame = output.GetSpan(total)[..total];
        WriteHeader(frame, total, opcode);
        WriteBody(frame[FrameReader.HeaderBytes..], message, bodyBytes);
        output.Advance(total);
        return total;
    }

    /// <summary>
    /// Write <paramref name="message"/> inside an envelope frame and return the bytes written. The envelope's body is the inner frame:
    /// its length in clear, then its opcode and body encrypted with <paramref name="cipher"/>.
    /// </summary>
    /// <exception cref="CodecException">The message wrote a different number of bytes than it measured.</exception>
    public static int WriteEnvelope(IBufferWriter<byte> output, ushort envelopeOpcode, ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        int bodyBytes = BodyBytes(message);
        int innerBytes = FrameReader.HeaderBytes + bodyBytes;
        int total = FrameReader.HeaderBytes + innerBytes;
        Span<byte> frame = output.GetSpan(total)[..total];
        WriteHeader(frame, total, envelopeOpcode);

        Span<byte> inner = frame[FrameReader.HeaderBytes..];
        WriteHeader(inner, innerBytes, opcode);
        WriteBody(inner[FrameReader.HeaderBytes..], message, bodyBytes);
        cipher.Encrypt(inner[4..]);

        output.Advance(total);
        return total;
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

    private static int BodyBytes(IWireStruct message) => (message.MeasureBits() + 7) >> 3;

    private static void WriteHeader(Span<byte> frame, int length, ushort opcode)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(frame, (uint)length);
        BinaryPrimitives.WriteUInt16LittleEndian(frame[4..], opcode);
    }

    private static void WriteBody(Span<byte> body, IWireStruct message, int bodyBytes)
    {
        var writer = new BitWriter(body[..bodyBytes]);
        message.Write(ref writer);
        if (writer.BytesWritten != bodyBytes)
            throw new CodecException($"A message measured {bodyBytes} bytes and wrote {writer.BytesWritten}.");
    }
}
