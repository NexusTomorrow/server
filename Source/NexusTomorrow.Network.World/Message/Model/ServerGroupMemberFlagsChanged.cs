using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupMemberFlagsChanged)]
[WireMessage(GameMessageOpcode.ServerGroupMemberFlagsChanged, Direction.ServerToClient)]
public sealed partial class ServerGroupMemberFlagsChanged : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint MemberIndex { get; set; } //< Not sure
    public Identity TargetedPlayer { get; set; } = new();
    [UInt(32)] public GroupMemberInfoFlags ChangedFlags { get; set; }
    [UInt(1)] public bool IsFromPromotion { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(MemberIndex);
        TargetedPlayer.Write(writer);
        writer.Write(ChangedFlags, 32);
        writer.Write(IsFromPromotion);
    }
}
