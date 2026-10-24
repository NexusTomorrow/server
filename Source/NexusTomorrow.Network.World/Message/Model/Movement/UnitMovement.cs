using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Moves a unit the client does not move, by commands stamped with the sender's clock.
/// </summary>
/// <remarks>
/// The client ignores the whole message for the unit it moves.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0638, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
[LayoutConfirmed]
public sealed partial class UnitMovement
{
    /// <summary>
    /// The unit, by id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The time of the message on the sender's clock. For each unit the client fits the sender's clock to its own over up to 16 of these
    /// times, taking a new one only once it has moved on by at least half of <see cref="WorldSettings.MovementInterval"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Time { get; set; }

    /// <summary>
    /// When set, the client restarts the unit's movement from its current state at the current time, and drops its fit of the sender's
    /// clock.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Reset { get; set; }

    /// <summary>
    /// A mark the client stores on the unit, the same one <see cref="MovementControlEnd"/> sets and taking the unit over clears: while the
    /// unit the client moves, or last moved, carries it, a <see cref="MovementControl"/> for the player's own unit makes the client send a
    /// <see cref="ZoneChange"/> at once. Every unit the client creates starts with it set.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool ServerMoved { get; set; }

    /// <summary>
    /// The commands the client applies to the unit.
    /// </summary>
    [VerifiedMember, Array(CountBits = 5)] public List<Server0262Part5> Commands { get; set; } = [];
}
