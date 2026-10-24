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
    /// A unit that takes this unit as its vehicle, with its seat type and one other value.
    /// </summary>
    [WireStruct]
    public sealed partial class Server0262Part1
    {
        /// <summary>
        /// The kind of seat the unit takes on this vehicle: the client picks the unit's attachment point by it, and plays the passenger
        /// animations only for seat types 0 to 2.
        /// </summary>
        [VerifiedMember, UInt(2)] public byte SeatType { get; set; }

        [UInt(3)] public byte Unknown1 { get; set; }

        /// <summary>
        /// A unit, by unit id. If the client has that unit, that unit takes this one as its vehicle, with the two values before this one.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint UnitId { get; set; }
    }

    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The vehicle the unit is, as an id in the client's UnitVehicle table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort UnitVehicleId { get; set; }

    /// <summary>
    /// Other units: each one the client has takes this unit as its vehicle.
    /// </summary>
    [VerifiedMember, Array(CountBits = 3)] public List<Server0262Part1> Unknown2 { get; set; } = [];
}
