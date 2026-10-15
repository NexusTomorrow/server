using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot
{
    [Message(GameMessageOpcode.ServerLootGrant)]
    public class ServerLootGrant : IWritable
    {
        public uint OwnerUnitId { get; set; }
        public uint LooterUnitId { get; set; }
        public LootItem LootItem { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(OwnerUnitId);
            writer.Write(LooterUnitId);
            LootItem.Write(writer);
        }
    }
}
