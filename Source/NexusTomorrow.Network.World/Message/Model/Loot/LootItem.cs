using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireStruct]
public sealed partial class LootItem
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
}
