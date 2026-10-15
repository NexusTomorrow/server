using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Mount)]
public sealed partial class MountEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(14)] public ushort UnitVehicleId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }
    [Array(CountBits = 3)] public List<VehiclePassenger> Passengers { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(UnitVehicleId, 14u);
        writer.Write(OwnerId);

        writer.Write((byte)Passengers.Count, 3u);
        Passengers.ForEach(p => p.Write(writer));
    }
}
