using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Launches a missile from one unit to another at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server021B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicMissileAdd
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The handle the client keeps the missile by, which <see cref="CinematicMissileRemove"/> names it with.
    /// </summary>
    [UInt(32)] public uint MissileHandle { get; set; }

    /// <summary>
    /// The unit the missile starts from, by unit id; its position is the missile's start.
    /// </summary>
    [UInt(32)] public uint CasterUnitId { get; set; }

    /// <summary>
    /// The unit the missile flies to, by unit id; its position is the missile's end.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    /// <summary>
    /// The missile, a Spell4ClientMissile id.
    /// </summary>
    [UInt(32)] public uint ClientMissileId { get; set; }
}
