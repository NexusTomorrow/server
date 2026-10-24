using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 16.
/// </summary>
[WireStruct]
[UnionCase(16)]
public sealed partial class Server0262Union0Arm10 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client ignores an id it has no creature for.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }
}
