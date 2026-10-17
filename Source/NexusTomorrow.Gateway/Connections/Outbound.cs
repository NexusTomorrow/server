using NexusTomorrow.Network.Codec;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Connections;

internal enum OutboundKind
{
    Bare,
    Enveloped,
    Key,
    PingAnswer,
}

// One item of a session's outbound queue. A key change is an item too, so it takes effect exactly between the messages queued before
// and after it.
internal readonly record struct Outbound(OutboundKind Kind, ushort Opcode, IWireStruct? Message, PacketCipher? Cipher, byte PingIndex)
{
    public static Outbound Bare(ushort opcode, IWireStruct message) => new(OutboundKind.Bare, opcode, message, null, 0);

    public static Outbound Enveloped(ushort opcode, IWireStruct message) => new(OutboundKind.Enveloped, opcode, message, null, 0);

    public static Outbound Key(PacketCipher cipher) => new(OutboundKind.Key, 0, null, cipher, 0);

    public static Outbound Ping(byte index) => new(OutboundKind.PingAnswer, 0, null, null, index);
}
