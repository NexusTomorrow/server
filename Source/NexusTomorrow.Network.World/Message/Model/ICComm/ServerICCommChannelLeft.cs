using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[Message(GameMessageOpcode.ServerICCommChannelLeft)]
[WireMessage(GameMessageOpcode.ServerICCommChannelLeft, Direction.ServerToClient)]
public sealed partial class ServerICCommChannelLeft : IWritable
{
    [UInt(64)] public ulong IccomId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
    }
}
