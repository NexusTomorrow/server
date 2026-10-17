using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 25.
/// </summary>
[WireStruct]
[UnionCase(25)]
public sealed partial class Server0262Union0Arm19 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    /// <summary>
    /// A unit's id, which the client looks up when it creates this unit.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(15)] public ushort Unknown2 { get; set; }
}
