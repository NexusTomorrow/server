using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Shows or hides units' text bubbles during the cinematic camera, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server022B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicTextBubbles
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// Whether units' text bubbles show while the cinematic camera runs.
    /// </summary>
    [UInt(1)] public bool Visible { get; set; }
}
