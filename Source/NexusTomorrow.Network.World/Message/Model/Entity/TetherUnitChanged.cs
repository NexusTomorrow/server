using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the tether of the unit the client moves: the unit it is tethered to.
/// </summary>
/// <remarks>
/// The client keeps the id on the unit it moves, and ignores the message while it moves none. That unit's controller looks
/// the tethered unit up by this id and uses the unit it finds. When MovementControl hands control to another unit, the
/// client carries the id over to it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0881, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[LayoutConfirmed]
public sealed partial class TetherUnitChanged
{
    /// <summary>
    /// The unit tethered to, by unit id.
    /// </summary>
    [UInt(32)] public uint TetherUnitId { get; set; }
}
