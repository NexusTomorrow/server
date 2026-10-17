using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Gives the client control of a unit's movement.
/// </summary>
/// <remarks>
/// Taken only after <see cref="PlayerUnit"/>; before it the client ignores it. The client answers at once with <c>0x0635</c>. Unless it
/// already moves the unit, it then takes the unit over and starts sending its movement (<c>0x0637</c>) and its zone changes
/// (<c>0x063A</c>); when control comes back to the player's own unit after it was taken away, the client also sends <c>0x063A</c> at
/// once. The client registers a writer for it as well.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0636, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MovementControl
{
    /// <summary>
    /// A value the client sends straight back in <c>0x0635</c>.
    /// </summary>
    [UInt(32)] public uint Ticket { get; set; }

    /// <summary>
    /// When set, the client rebuilds the movement state of the unit it moves from that unit's current state, at the current time.
    /// </summary>
    [UInt(1)] public bool RebuildMovement { get; set; }

    /// <summary>
    /// The unit to move. If it is not the unit the client moves, the client switches to it, or waits for that unit's <see cref="UnitCreate"/>.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
