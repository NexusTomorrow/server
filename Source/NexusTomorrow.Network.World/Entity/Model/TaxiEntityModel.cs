using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Taxi)]
public sealed partial class TaxiEntityModel : IEntityModel
{
    [WireStruct]
    public sealed partial class Passenger : IWritable
    {
        [UInt(2)] public byte SeatType { get; set; }
        [UInt(3)] public byte SeatPosition { get; set; }
        [UInt(32)] public uint UnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(SeatType, 2);
            writer.Write(SeatPosition, 3);
            writer.Write(UnitId);
        }
    }

    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(14)] public ushort UnitVehicleId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }
    [Array(CountBits = 3)] public List<Passenger> Passengers { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18);
        writer.Write(UnitVehicleId, 14);
        writer.Write(OwnerId);

        writer.Write((byte)Passengers.Count, 3);
        Passengers.ForEach(s => s.Write(writer));
    }
}
