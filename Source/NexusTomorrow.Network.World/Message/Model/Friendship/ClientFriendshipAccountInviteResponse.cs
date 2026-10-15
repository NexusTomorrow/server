using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountInviteResponse
{
    [UInt(64)] public ulong AccountFriendInviteId { get; private set; }
    [UInt(1)] public bool Response { get; private set; }
}
