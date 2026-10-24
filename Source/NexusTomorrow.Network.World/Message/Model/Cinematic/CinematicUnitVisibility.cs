using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets which units are drawn during the cinematic, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0220, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicUnitVisibility
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id; 0 for every unit.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// For a unit, set keeps the unit visible during the cinematic. With <see cref="UnitId"/> 0, it sets whether every unit not kept
    /// visible is hidden.
    /// </summary>
    [UInt(1)] public bool Visible { get; set; }

    /// <summary>
    /// With <see cref="UnitId"/> 0, limits <see cref="Visible"/> to players.
    /// </summary>
    [UInt(1)] public bool PlayersOnly { get; set; }
}
