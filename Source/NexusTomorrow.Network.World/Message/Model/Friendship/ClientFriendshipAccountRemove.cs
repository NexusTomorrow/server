using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountRemove, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountRemove
{
    [UInt(64)] public ulong AccountFriendId { get; private set; }
}
