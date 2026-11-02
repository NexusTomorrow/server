using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Attaches a unit to a piece of a residence's decor.
/// </summary>
[WireMessage(GameMessageOpcode.Server053A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingAttachUnitToDecor
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    [VerifiedMember] public RealmScopedId ResidenceId { get; set; } = new();

    /// <summary>
    /// The unit being attached, by unit id.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong UnitId { get; set; }

    /// <summary>
    /// The decor item the unit is attached to, by item id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint DecorItemId { get; set; }
}
