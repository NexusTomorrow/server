using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPublicDisplayName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPublicDisplayName
{
    [WideString] public string PublicDisplayName { get; private set; }
}
