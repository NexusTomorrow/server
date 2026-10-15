using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

// Clears the player's destination arrow regardless of how it was set
[WireMessage(GameMessageOpcode.ServerDestinationArrowClear, Direction.ServerToClient)]
public sealed partial class ServerDestinationArrowClear
{
}
