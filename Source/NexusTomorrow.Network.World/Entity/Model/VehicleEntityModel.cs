using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Vehicle)]
public sealed partial class VehicleEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(14)] public ushort UnitVehicleId { get; set; }
    [Array(CountBits = 3)] public List<VehiclePassenger> Passengers { get; set; } = [];
}
