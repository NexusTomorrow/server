using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountInviteRemove, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountInviteRemove
{
    [UInt(32)] public uint AccountId { get; set; }
    [UInt(64)] public ulong AccountFriendInviteId { get; set; }
}
