using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Sets CanLoot flag on the loot
[Message(GameMessageOpcode.ServerLootCanLoot)]
[WireMessage(GameMessageOpcode.ServerLootCanLoot, Direction.ServerToClient)]
public sealed partial class ServerLootCanLoot : IWritable
{
    [UInt(32)] public uint LootUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LootUnitId);
    }
}
