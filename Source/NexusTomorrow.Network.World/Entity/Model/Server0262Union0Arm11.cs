using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 17.
/// </summary>
[WireStruct]
[UnionCase(17)]
public sealed partial class Server0262Union0Arm11 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's warplot tier, counting from 0. Unit:GetCurrentWarplotTier returns it plus 1, Unit:CanUpgradeWarplotStructure reads
    /// it, and Unit:GetCurrentWarplotUpgradeCost picks the upgrade cost of <see cref="WarplotPlugInfoId"/>'s row by it, giving none above
    /// 1. <c>0x08F6</c> changes it.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte WarplotTier { get; set; }

    /// <summary>
    /// The unit's warplot plug, as an id in the client's HousingWarplotPlugInfo table, whose row gives the upgrade costs
    /// Unit:GetCurrentWarplotUpgradeCost reads.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort WarplotPlugInfoId { get; set; }
}
