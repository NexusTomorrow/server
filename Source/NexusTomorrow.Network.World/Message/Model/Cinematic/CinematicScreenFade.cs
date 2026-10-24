using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Fades the screen out, holds the cinematic's commands until the fade-out and the hold have passed, and fades playback back in.
/// </summary>
[WireMessage(GameMessageOpcode.Server0222, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicScreenFade
{
    /// <summary>
    /// The fade: <see cref="ScaleTransitionType.FadeColor"/> or <see cref="ScaleTransitionType.FadeWhite"/> picks its color.
    /// </summary>
    [UInt(32)] public ScaleTransitionType Kind { get; set; }

    /// <summary>
    /// How long the screen takes to fade out, in milliseconds.
    /// </summary>
    [UInt(16)] public ushort FadeOutMs { get; set; }

    /// <summary>
    /// Added to <see cref="FadeOutMs"/>, the time in milliseconds after which the client runs the commands it held during the fade.
    /// </summary>
    [UInt(16)] public ushort HoldMs { get; set; }

    /// <summary>
    /// How long playback takes to fade back in, in milliseconds.
    /// </summary>
    [UInt(16)] public ushort FadeInMs { get; set; }
}
