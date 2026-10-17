using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Gives the client control of a unit's movement.
/// </summary>
/// <remarks>
/// Taken only after <see cref="PlayerUnit"/>; before it the client ignores it. The client answers at once with
/// <see cref="MovementControlReceipt"/>. Unless it already moves the unit, it then takes the unit over and starts sending its movement
/// (<see cref="ClientMovement"/>) and its zone changes (<see cref="ZoneChange"/>); when it first takes over the player's own unit, and when
/// control of that unit comes back after it was taken away, the client also sends a <see cref="ZoneChange"/> at once. The client registers
/// a writer for it as well.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0636, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MovementControl
{
    /// <summary>
    /// A value the client sends straight back in <see cref="MovementControlReceipt"/>.
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
