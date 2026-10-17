using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 7.
/// </summary>
[WireStruct]
[UnionCase(7)]
public sealed partial class Server0262Union0Arm07 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    /// <summary>
    /// An id in the client's UnitVehicle table.
    /// </summary>
    [UInt(14)] public ushort Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// Other units: the client records this unit on each one it has.
    /// </summary>
    [Array(CountBits = 3)] public List<Server0262Part0> Unknown3 { get; set; } = [];
}
