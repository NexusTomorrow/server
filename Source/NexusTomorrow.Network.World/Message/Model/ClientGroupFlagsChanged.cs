using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupFlagsChanged)]
[WireMessage(GameMessageOpcode.ClientGroupFlagsChanged, Direction.ClientToServer)]
public sealed partial class ClientGroupFlagsChanged : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public GroupFlags NewFlags { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        NewFlags = reader.ReadEnum<GroupFlags>(32u);
    }
}
