using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPlayerEnteredWorld, Direction.ServerToClient)]
public sealed partial class ServerPlayerEnteredWorld
{
}
