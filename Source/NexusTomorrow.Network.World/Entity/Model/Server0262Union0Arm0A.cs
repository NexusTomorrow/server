using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 10.
/// </summary>
[WireStruct]
[UnionCase(10)]
public sealed partial class Server0262Union0Arm0A : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client ignores an id it has no creature for.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    [UInt(8)] public byte Unknown1 { get; set; }
}
