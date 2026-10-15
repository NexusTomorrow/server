using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[Message(GameMessageOpcode.ServerICCommMessageResult)]
[WireMessage(GameMessageOpcode.ServerICCommMessageResult, Direction.ServerToClient)]
public sealed partial class ServerICCommMessageResult : IWritable
{
    [UInt(64)] public ulong IccomId { get; set; }
    [UInt(32)] public uint MessageId { get; set; }
    [UInt(4)] public ICCommMessageResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
        writer.Write(MessageId);
        writer.Write(Result, 4u);
    }
}
