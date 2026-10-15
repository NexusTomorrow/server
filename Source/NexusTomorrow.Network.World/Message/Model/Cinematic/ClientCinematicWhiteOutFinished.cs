using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ClientCinematicWhiteOutFinished, Direction.ClientToServer)]
public sealed partial class ClientCinematicWhiteOutFinished
{
}
