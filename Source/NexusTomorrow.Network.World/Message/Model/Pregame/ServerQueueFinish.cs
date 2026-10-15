using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[WireMessage(GameMessageOpcode.ServerQueueFinish, Direction.ServerToClient)]
public sealed partial class ServerQueueFinish
{
}
