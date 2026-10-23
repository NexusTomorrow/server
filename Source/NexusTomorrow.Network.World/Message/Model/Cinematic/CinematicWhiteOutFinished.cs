using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Client0721, Direction.ClientToServer)]
public sealed partial class CinematicWhiteOutFinished
{
}
