using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client0397, Direction.ClientToServer)]
public sealed partial class FriendshipAccountAddByEmail
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";
}
