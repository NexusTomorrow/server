using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountInviteMarkSeen, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountInviteMarkSeen
{
    [UInt(64)] public ulong AccountFriendInviteId { get; private set; }
}
