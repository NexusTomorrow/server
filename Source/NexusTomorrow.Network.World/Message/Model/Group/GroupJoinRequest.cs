using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0424, Direction.ServerToClient)]
public sealed partial class GroupJoinRequest
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public GroupMemberRecord Unknown1 { get; set; } = new();
}
