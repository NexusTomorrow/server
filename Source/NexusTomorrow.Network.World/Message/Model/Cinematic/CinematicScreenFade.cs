using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Fades the screen out, holds the cinematic's commands during the fade, and fades playback back in.
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

    [UInt(16)] public ushort Unknown2 { get; set; }

    /// <summary>
    /// How long playback takes to fade back in, in milliseconds.
    /// </summary>
    [UInt(16)] public ushort FadeInMs { get; set; }
}
