using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupLeave)]
[WireMessage(GameMessageOpcode.ServerGroupLeave, Direction.ServerToClient)]
public sealed partial class ServerGroupLeave : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(4)] public RemoveReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(Reason, 4u);
    }
}
