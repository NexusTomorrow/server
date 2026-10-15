using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipRemoveByIdentity, Direction.ClientToServer)]
public sealed partial class ClientFriendshipRemoveByIdentity
{
    public Identity PlayerIdentity { get; private set; } = new();
    [UInt(4)] public FriendshipType Type { get; private set; }
}
