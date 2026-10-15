using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingGroupMemberRoleSelection, Direction.ServerToClient)]
public sealed partial class ServerMatchingGroupMemberRoleSelection
{
    public Identity Identity { get; set; } = new();
    [UInt(32)] public Role Role { get; set; }
}
