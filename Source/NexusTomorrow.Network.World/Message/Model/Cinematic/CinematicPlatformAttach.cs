using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Puts a unit on a platform unit at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0211, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicPlatformAttach
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

    /// <summary>
    /// The platform, by unit id.
    /// </summary>
    [UInt(32)] public uint PlatformUnitId { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
