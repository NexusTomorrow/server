using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountAddByEmail, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountAddByEmail
{
    [WideString] public string Email { get; private set; }
    [WideString] public string Note { get; private set; }
}
