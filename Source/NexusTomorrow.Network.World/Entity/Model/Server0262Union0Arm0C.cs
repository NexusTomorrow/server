using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 12.
/// </summary>
[WireStruct]
[UnionCase(12)]
public sealed partial class Server0262Union0Arm0C : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }
}
