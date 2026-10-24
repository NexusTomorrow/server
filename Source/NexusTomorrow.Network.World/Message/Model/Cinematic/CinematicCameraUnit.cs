using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Puts the cinematic camera on a unit, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0217, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraUnit
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

    [UInt(1)] public bool Unknown2 { get; set; }
}
