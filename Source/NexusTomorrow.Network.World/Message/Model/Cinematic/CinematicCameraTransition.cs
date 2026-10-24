using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Runs a transition of the cinematic camera at the command's time, after reporting the camera's subject to the server.
/// </summary>
[WireMessage(GameMessageOpcode.Server0216, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraTransition
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// How the camera moves to its new view.
    /// </summary>
    [UInt(32)] public ScaleTransitionType Kind { get; set; }

    /// <summary>
    /// The transition's duration, in milliseconds: the blend's, or the fade-out's.
    /// </summary>
    [UInt(16)] public ushort DurationMs { get; set; }

    [UInt(16)] public ushort Unknown3 { get; set; }

    /// <summary>
    /// How long a fade takes to fade back in, in milliseconds.
    /// </summary>
    [UInt(16)] public ushort FadeInMs { get; set; }
}
