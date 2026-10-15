using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireStruct]
public sealed partial class LootItem : IWritable
{
    [UInt(32)] public uint LootUnitId { get; set; }
    [UInt(32)] public LootItemType Type { get; set; }
    [UInt(32)] public uint ItemId { get; set; }
    [UInt(32)] public uint Amount { get; set; }
    [UInt(1)] public bool CanLoot { get; set; }
    [UInt(1)] public bool RequiresRoll { get; set; }
    [UInt(1)] public bool OnlyMasterLootable { get; set; }
    [UInt(1)] public bool Explosion { get; set; }
    [UInt(32)] public uint RollTime { get; set; }
    [UInt(64)] public ulong RandomCircuitData { get; set; }
    [UInt(32)] public uint RandomGlyphData { get; set; }
    [UInt(32)] public uint ItemQuality2Id { get; set; }
    [Array(CountBits = 32)] public List<Identity> MasterList { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LootUnitId);
        writer.Write(Type, 32u);
        writer.Write(ItemId); // Can be Item2Id, LootSpellId, VirtualItemId, AccountItemId
        writer.Write(Amount);
        writer.Write(CanLoot);
        writer.Write(RequiresRoll);
        writer.Write(OnlyMasterLootable);
        writer.Write(Explosion);
        writer.Write(RollTime);
        writer.Write(RandomCircuitData);
        writer.Write(RandomGlyphData);
        writer.Write(ItemQuality2Id);
        writer.Write(MasterList.Count);
        MasterList.ForEach(m => m.Write(writer));
    }
}
