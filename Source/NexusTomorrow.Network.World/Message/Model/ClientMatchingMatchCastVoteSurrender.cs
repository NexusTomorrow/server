using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0624, Direction.ClientToServer)]
public sealed partial class Client0624
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
