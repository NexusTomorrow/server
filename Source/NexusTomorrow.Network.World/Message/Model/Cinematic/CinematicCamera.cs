using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Begins or ends the cinematic camera at the command's time, with the transition it ends through.
/// </summary>
/// <remarks>
/// The client reports <see cref="CinematicState.CameraStarted"/> when the camera begins and <see cref="CinematicState.CameraEnded"/> when it
/// ends, in <see cref="CinematicProgress"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0218, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCamera
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// <see cref="CameraAddFlags.AddCamera"/> begins the camera, and without it the command ends the camera.
    /// <see cref="CameraAddFlags.AwaitContinue"/> holds the camera after a fade-out, reporting <see cref="CinematicState.AwaitingContinue"/>,
    /// until <see cref="CinematicContinue"/>.
    /// </summary>
    [UInt(32)] public CameraAddFlags Flags { get; set; }

    /// <summary>
    /// The transition the camera ends through; the client ends the camera at once on <see cref="ScaleTransitionType.Cut"/>.
    /// </summary>
    [UInt(32)] public ScaleTransitionType EndKind { get; set; }

    /// <summary>
    /// The end transition's duration, in milliseconds: the blend's, or the fade-out's.
    /// </summary>
    [UInt(16)] public ushort EndDurationMs { get; set; }

    [UInt(16)] public ushort Unknown4 { get; set; }

    /// <summary>
    /// How long the end transition takes to fade back in, in milliseconds.
    /// </summary>
    [UInt(16)] public ushort EndFadeInMs { get; set; }
}
