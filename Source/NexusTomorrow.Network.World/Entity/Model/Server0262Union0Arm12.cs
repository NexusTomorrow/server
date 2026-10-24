using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 18.
/// </summary>
[WireStruct]
[UnionCase(18)]
public sealed partial class Server0262Union0Arm12 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The looted object, as an id in one of the client's Item2, VirtualItem, LootSpell, AccountItem and AccountCurrencyType tables: the
    /// client names the unit from the row it finds. For one kind of loot it gives the unit a fixed name instead and does not read this id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint LootObjectId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown2 { get; set; }

    [UInt(4)] public byte Unknown3 { get; set; }
}
