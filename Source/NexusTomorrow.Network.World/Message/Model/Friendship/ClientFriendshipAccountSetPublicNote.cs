using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPublicNote, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPublicNote
{
    [WideString] public string PublicNote { get; private set; }
}
