using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipBlock, Direction.ClientToServer)]
public sealed partial class ClientFriendshipBlock
{
    [UInt(1)] public bool BlockFriendRequests { get; private set; }
}
