using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingStopLookingForReplacements, Direction.ClientToServer)]
public sealed partial class ClientMatchingStopLookingForReplacements
{
}
