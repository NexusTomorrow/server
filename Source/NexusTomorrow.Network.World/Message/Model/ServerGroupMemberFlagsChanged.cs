using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupMemberFlagsChanged, Direction.ServerToClient)]
public sealed partial class ServerGroupMemberFlagsChanged
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint MemberIndex { get; set; } //< Not sure
    public Identity TargetedPlayer { get; set; } = new();
    [UInt(32)] public GroupMemberInfoFlags ChangedFlags { get; set; }
    [UInt(1)] public bool IsFromPromotion { get; set; }
}
