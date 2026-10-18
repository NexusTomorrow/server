using System.Buffers;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests;

internal static class ClientFrames
{
    public const ushort ClientEnvelope = 0x0244;
    public const ushort Nested = 0x025C;
    public const ushort Tagged = 0x038C;

    // A ClientMovement body that is malformed if read: a clock of 1000, then a count of 10,000 commands with no bits left for them.
    public static byte[] OverclaimingMovement() => [0xE8, 0x03, 0x00, 0x00, 0x10, 0x27, 0x00, 0x00];

    // The body of a 0x0244 frame holding one message, encrypted with the connection's cipher.
    public static byte[] EnvelopeBody(ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WriteEnvelope(output, ClientEnvelope, opcode, message, cipher);
        return output.WrittenSpan[FrameReader.HeaderBytes..].ToArray();
    }

    // The body of a 0x0244 frame holding 0x038C holding one message under a tag: the tag byte, then the message's whole frame.
    public static byte[] TaggedEnvelopeBody(byte tag, ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        var inner = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(inner, opcode, message);
        return EnvelopeBody(Tagged, new RawBody([(byte)(tag & 0x1F), .. inner.WrittenSpan]), cipher);
    }

    // The body of a 0x0244 frame holding 0x025C holding one message.
    public static byte[] NestedEnvelopeBody(ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        var inner = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(inner, opcode, message);
        var nested = new Network.Message.Model.NestedMessage { Payload = inner.WrittenSpan[4..].ToArray() };
        return EnvelopeBody(Nested, nested, cipher);
    }
}
