using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

// Not seen in sniffs so not sure when these would be sent
[Message(GameMessageOpcode.ServerICCommOrderedMessage)]
[WireMessage(GameMessageOpcode.ServerICCommOrderedMessage, Direction.ServerToClient)]
public sealed partial class ServerICCommOrderedMessage : IWritable
{
    [UInt(64)] public ulong IccomId { get; set; }
    [UInt(32)] public uint MessageId { get; set; }
    [WideString] public string Message { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
        writer.Write(MessageId);
        writer.WriteStringWide(Message);
    }
}
