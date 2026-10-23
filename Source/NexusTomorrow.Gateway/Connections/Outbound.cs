using System.Buffers;
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

// One item of a session's outbound queue. A message item holds the message's serialized body in a rented buffer and never the message
// itself, so the sender may change or reuse the message at once. The item is the buffer's single owner: the writer returns it after
// writing the item, Enqueue returns it when the queue refuses the item, and the writer's close path returns every item still queued.
// A key change is an item too, so it takes effect exactly between the messages queued before and after it.
internal readonly record struct Outbound(OutboundKind Kind, ushort Opcode, byte[]? Body, int BodyLength, Type? MessageType, PacketCipher? Cipher, byte PingIndex)
{
    public static Outbound Bare(ushort opcode, IWireStruct message) => Message(OutboundKind.Bare, opcode, message);

    public static Outbound Enveloped(ushort opcode, IWireStruct message) => Message(OutboundKind.Enveloped, opcode, message);

    public static Outbound Key(PacketCipher cipher) => new(OutboundKind.Key, 0, null, 0, null, cipher, 0);

    public static Outbound Ping(byte index) => new(OutboundKind.PingAnswer, 0, null, 0, null, null, index);

    // Returns the item's buffer to the pool; the one place a message item's buffer goes back. Key and ping items hold none. The buffer
    // is cleared on every return, whatever the message, so a returned buffer holds no message bytes (a body can carry a session key).
    public void ReturnBody()
    {
        if (Body is not null)
            ArrayPool<byte>.Shared.Return(Body, clearArray: true);
    }

    private static Outbound Message(OutboundKind kind, ushort opcode, IWireStruct message)
    {
        byte[] body = FrameWriter.RentBody(message, out int length);
        return new Outbound(kind, opcode, body, length, message.GetType(), null, 0);
    }
}
