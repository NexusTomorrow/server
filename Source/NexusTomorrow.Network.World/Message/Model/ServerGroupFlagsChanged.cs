using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupFlagsChanged)]
[WireMessage(GameMessageOpcode.ServerGroupFlagsChanged, Direction.ServerToClient)]
public sealed partial class ServerGroupFlagsChanged : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint Unk0 { get; set; }

    [UInt(32)] public GroupFlags Flags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(Unk0);
        writer.Write(Flags, 32u);
    }
}
