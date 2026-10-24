using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 27.
/// </summary>
[WireStruct]
[UnionCase(27)]
public sealed partial class Server0262Union0Arm1B : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's owner, by unit id. When the owner is a player, or a unit of type 23, the client takes the owner as this unit's controller
    /// for its disposition, which Unit:GetDispositionTo reports.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint OwnerUnitId { get; set; }

    /// <summary>
    /// The unit's name.
    /// </summary>
    [VerifiedMember, WideString] public string Name { get; set; } = "";
}
