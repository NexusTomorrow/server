using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.PinataLoot)]
public sealed partial class PinataLootEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(32)] public uint ItemId { get; set; }
    [UInt(32)] public uint ItemCount { get; set; }
    [UInt(4)] public byte LootType { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(ItemId);
        writer.Write(ItemCount);
        writer.Write(LootType, 4);
    }
}
