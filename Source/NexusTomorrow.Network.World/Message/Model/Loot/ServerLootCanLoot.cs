using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Sets CanLoot flag on the loot
[WireMessage(GameMessageOpcode.ServerLootCanLoot, Direction.ServerToClient)]
public sealed partial class ServerLootCanLoot
{
    [UInt(32)] public uint LootUnitId { get; set; }
}
