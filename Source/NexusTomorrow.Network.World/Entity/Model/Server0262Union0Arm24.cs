using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 36.
/// </summary>
[WireStruct]
[UnionCase(36)]
public sealed partial class Server0262Union0Arm24 : IServer0262Union0
{
    /// <summary>
    /// One unit that tagged this one.
    /// </summary>
    [WireStruct]
    public sealed partial class Server0262Part3
    {
        /// <summary>
        /// The tagger, by unit id. The client compares it with the player's unit.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint UnitId { get; set; }
    }

    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The units that tagged this one. While the client has the player's unit, it sets this unit's tagged-by-me flag when the player's unit
    /// is among them and clears it otherwise; Unit:IsTaggedByMe and the unit's nameplate colour read the flag.
    /// </summary>
    [VerifiedMember, Array(CountBits = 16)] public List<Server0262Part3> TaggerUnitIds { get; } = [];
}
