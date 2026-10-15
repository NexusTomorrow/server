using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupMaxSizeChange)]
[WireMessage(GameMessageOpcode.ServerGroupMaxSizeChange, Direction.ServerToClient)]
public sealed partial class ServerGroupMaxSizeChange : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public GroupFlags NewFlags { get; set; }

    [UInt(32)] public uint NewMaxSize { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(NewFlags, 32u);
        writer.Write(NewMaxSize);
    }
}
