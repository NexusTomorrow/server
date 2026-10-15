using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipSetAutoResponseMessage, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetAutoResponseMessage
{
    [WideString] public string AwayMessage { get; private set; }
    [WideString] public string BusyMessage { get; private set; }
}
