using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Trigger BindOnPickup dialog when looting an item
[Message(GameMessageOpcode.ServerLootBindOnPickup)]
[WireMessage(GameMessageOpcode.ServerLootBindOnPickup, Direction.ServerToClient)]
public sealed partial class ServerLootBindOnPickup : IWritable
{
    [UInt(32)] public uint Unused { get; set; }
    [UInt(32)] public uint LootUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
        writer.Write(LootUnitId);
    }
}
