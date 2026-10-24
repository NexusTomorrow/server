using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Tells the server the screen has faded out after <see cref="CinematicWhiteOut"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Client0721, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CinematicWhiteOutFinished
{
}
