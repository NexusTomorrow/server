using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Gives the player movement control during the cinematic, or takes it away, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0219, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicPlayerControl
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// Whether the player keeps control of movement while the cinematic plays.
    /// </summary>
    [UInt(1)] public bool Enabled { get; set; }
}
