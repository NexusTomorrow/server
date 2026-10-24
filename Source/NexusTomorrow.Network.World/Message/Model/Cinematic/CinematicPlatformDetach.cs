using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Takes a unit off the platform <see cref="CinematicPlatformAttach"/> put it on, at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server021E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicPlatformDetach
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
