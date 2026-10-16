using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client039E, Direction.ClientToServer)]
public sealed partial class Client039E
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
