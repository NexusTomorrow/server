using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipInviteMarkSeen, Direction.ClientToServer)]
public sealed partial class ClientFriendshipInviteMarkSeen
{
    [Array(CountBits = 16), UInt(64)] public List<ulong> FriendInviteIds { get; private set; } = [];// Array of InviteIds
}
