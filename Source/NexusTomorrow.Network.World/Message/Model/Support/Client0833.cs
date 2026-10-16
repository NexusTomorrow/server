using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client0833, Direction.ClientToServer)]
public sealed partial class Client0833
{
    [WideString] public string Unknown0 { get; private set; }
}
