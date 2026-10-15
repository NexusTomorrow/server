using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Trigger BindOnPickup dialog when looting an item
[WireMessage(GameMessageOpcode.ServerLootBindOnPickup, Direction.ServerToClient)]
public sealed partial class ServerLootBindOnPickup
{
    [UInt(32)] public uint Unused { get; set; }
    [UInt(32)] public uint LootUnitId { get; set; }
}
