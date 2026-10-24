using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 13.
/// </summary>
[WireStruct]
[UnionCase(13)]
public sealed partial class Server0262Union0Arm0D : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The objective the unit stands for, as an index: the client reads it for the unit's quest activation state, for whether an item used
    /// on the unit serves a quest objective, for path mission objectives, and for Unit:GetRewardInfo.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte ObjectiveIndex { get; set; }
}
