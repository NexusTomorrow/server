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
    [VerifiedMember, UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The vehicle the unit is, as an id in the client's UnitVehicle table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort UnitVehicleId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// Other units: each one the client has takes this unit as its vehicle.
    /// </summary>
    [VerifiedMember, Array(CountBits = 3)] public List<Server0262Part0> Unknown3 { get; set; } = [];
}
