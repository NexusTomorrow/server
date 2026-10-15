using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountAddByIdentity, Direction.Both)]
public sealed partial class ClientFriendshipAddByIdentity
{
    public Identity Target { get; set; } = new(); // Match the account from the target's identity
    [UInt(4)] public FriendshipType Type { get; set; }
    [WideString] public string Note { get; set; } // Optional note sent with invite
}
