using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client076F, Direction.ClientToServer)]
public sealed partial class Client076F
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
