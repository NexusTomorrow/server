using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 24.
/// </summary>
[WireStruct]
[UnionCase(24)]
public sealed partial class Server0262Union0Arm18 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's owner, by unit id. Unit:GetUnitOwner finds the owner by it, and the client adds this unit to the owner's pets as it
    /// creates it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint OwnerUnitId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(15)] public ushort Unknown2 { get; set; }

    /// <summary>
    /// The unit's name, which the client gives the unit when its creature gives it none.
    /// </summary>
    [VerifiedMember, WideString] public string FallbackName { get; set; } = "";
}
