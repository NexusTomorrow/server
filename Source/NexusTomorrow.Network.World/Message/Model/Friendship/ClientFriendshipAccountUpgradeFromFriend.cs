using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Takes an existing friend and uses the identity of that character to
// send an account friend invite.
[WireMessage(GameMessageOpcode.ClientFriendshipAccountUpgradeFromFriend, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountUpgradeFromFriend
{
    public Identity ExisitingFriend { get; private set; } = new();
    [WideString] public string Note { get; private set; }
}
