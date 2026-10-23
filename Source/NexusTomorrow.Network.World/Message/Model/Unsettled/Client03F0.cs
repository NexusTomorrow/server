using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client03F0, Direction.ClientToServer)]
[Dormant]
public sealed partial class Client03F0
{
}
