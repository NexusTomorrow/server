using System.Buffers;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests;

internal static class ClientFrames
{
    public const ushort ClientEnvelope = 0x0244;
    public const ushort Nested = 0x025C;

    // The body of a 0x0244 frame holding one message, encrypted with the connection's cipher.
    public static byte[] EnvelopeBody(ushort opcode, IWireStruct message, PacketCipher cipher)
    {
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WriteEnvelope(output, ClientEnvelope, opcode, message, cipher);
        return output.WrittenSpan[FrameReader.HeaderBytes..].ToArray();
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
