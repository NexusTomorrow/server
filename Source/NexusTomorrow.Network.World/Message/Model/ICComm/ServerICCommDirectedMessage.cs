using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[Message(GameMessageOpcode.ServerICCommDirectedMessage)]
[WireMessage(GameMessageOpcode.ServerICCommDirectedMessage, Direction.ServerToClient)]
public sealed partial class ServerICCommDirectedMessage : IWritable
{
    [UInt(64)] public ulong IccomId { get; set; }
    [NarrowString] public string Message { get; set; }
    [WideString] public string SenderName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
        writer.WriteString(Message);
        writer.WriteStringWide(SenderName);
    }
}

