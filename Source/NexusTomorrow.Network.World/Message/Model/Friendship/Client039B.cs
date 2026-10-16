using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client039B, Direction.ClientToServer)]
public sealed partial class Client039B
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
