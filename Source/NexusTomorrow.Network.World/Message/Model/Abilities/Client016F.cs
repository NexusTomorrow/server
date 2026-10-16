using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client016F, Direction.ClientToServer)]
public sealed partial class Client016F
{
    [UInt(8)] public byte Unknown0 { get; private set; }
}
