using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingLeftQueue, Direction.ServerToClient)]
public sealed partial class ServerMatchingLeftQueue
{
}
