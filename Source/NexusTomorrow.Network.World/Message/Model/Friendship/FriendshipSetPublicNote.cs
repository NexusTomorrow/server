using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03AC, Direction.ClientToServer)]
public sealed partial class FriendshipSetPublicNote
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
