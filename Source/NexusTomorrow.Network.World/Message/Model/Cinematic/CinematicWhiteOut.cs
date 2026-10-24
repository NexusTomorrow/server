using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Fades the screen out to white; the client answers <see cref="CinematicWhiteOutFinished"/> once the fade is done.
/// </summary>
/// <remarks>
/// The client answers at once when the cinematic camera already holds after a <see cref="ScaleTransitionType.FadeColor"/> or
/// <see cref="ScaleTransitionType.FadeWhite"/> transition.
/// </remarks>
[WireMessage(GameMessageOpcode.Server022D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicWhiteOut
{
}
