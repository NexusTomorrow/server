using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.ServerLootNotify, Direction.ServerToClient)]
public sealed partial class ServerLootNotify
{
    [UInt(32)] public uint OwnerUnitId { get; set; }
    [UInt(32)] public uint ParentUnitId { get; set; } // to be confirmed
    [UInt(1)] public bool Explosion { get; set; }
    [Array(CountBits = 32)] public List<LootItem> LootItems { get; set; } = [];
}
