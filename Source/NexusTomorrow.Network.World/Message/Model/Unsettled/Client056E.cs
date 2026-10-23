using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client056E, Direction.ClientToServer)]
[Dormant]
public sealed partial class Client056E
{
}
