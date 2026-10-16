using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.Client0170, Direction.ClientToServer)]
public sealed partial class Client0170
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
