using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot
{
    // Sets CanLoot flag on the loot
    [Message(GameMessageOpcode.ServerLootCanLoot)]
    public class ServerLootCanLoot : IWritable
    {
        public uint LootUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(LootUnitId);
        }
    }
}
