using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 9.
/// </summary>
[WireStruct]
[UnionCase(9)]
public sealed partial class Server0262Union0Arm09 : IServer0262Union0
{
    /// <summary>
    /// A unit the client records this unit on, with two values.
    /// </summary>
    [WireStruct]
    public sealed partial class Server0262Part1
    {
        [UInt(2)] public byte Unknown0 { get; set; }
        [UInt(3)] public byte Unknown1 { get; set; }

        /// <summary>
        /// A unit's id. If the client has that unit, it records this unit on it, with the two values before this one.
        /// </summary>
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    /// <summary>
    /// An id in the client's UnitVehicle table.
    /// </summary>
    [UInt(14)] public ushort Unknown1 { get; set; }

    /// <summary>
    /// Other units: the client records this unit on each one it has.
    /// </summary>
    [Array(CountBits = 3)] public List<Server0262Part1> Unknown2 { get; set; } = [];
}
