using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupSetRole)]
[WireMessage(GameMessageOpcode.ClientGroupSetRole, Direction.ClientToServer)]
public sealed partial class ClientGroupSetRole : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    public Identity TargetedPlayer { get; set; } = new();

    [UInt(32)] public GroupMemberInfoFlags CurrentFlags { get; set; }

    [UInt(32)] public GroupMemberInfoFlags ChangedFlag { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        TargetedPlayer.Read(reader);
        CurrentFlags = reader.ReadEnum<GroupMemberInfoFlags>(32u);
        ChangedFlag = reader.ReadEnum<GroupMemberInfoFlags>(32u);
    }
}
