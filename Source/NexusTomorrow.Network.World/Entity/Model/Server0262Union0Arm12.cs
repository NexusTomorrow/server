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
    /// The loot, as an id the client looks up in the table the arm's loot type picks. The record it finds gives the unit its name.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint LootId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown2 { get; set; }

    [UInt(4)] public byte Unknown3 { get; set; }
}
