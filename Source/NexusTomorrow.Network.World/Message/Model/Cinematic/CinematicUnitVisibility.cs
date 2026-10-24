using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets which units are drawn during the cinematic, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0220, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicUnitVisibility
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The unit, by unit id; while <see cref="Unknown3"/> is clear, 0 names every unit.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Whether the units it names are shown during the cinematic. With <see cref="UnitId"/> 0 and <see cref="Unknown3"/> clear, set shows
    /// every unit, and clear hides every unit not kept visible. With a unit, set keeps that unit visible and clear stops keeping it; for a
    /// cinematic actor, it shows or hides the actor.
    /// </summary>
    [UInt(1)] public bool Visible { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }
}
