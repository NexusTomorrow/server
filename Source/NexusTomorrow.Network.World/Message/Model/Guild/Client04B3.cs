using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client04B3, Direction.ClientToServer)]
public sealed partial class Client04B3 : Client04B1
{
}
