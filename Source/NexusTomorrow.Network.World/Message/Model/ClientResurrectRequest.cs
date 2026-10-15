using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientResurrectRequest, Direction.ClientToServer)]
public sealed partial class ClientResurrectRequest
{
}
