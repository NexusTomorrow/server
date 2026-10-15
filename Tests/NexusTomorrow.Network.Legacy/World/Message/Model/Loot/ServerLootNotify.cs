using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ServerLootNotify)]
public class ServerLootNotify : IWritable
{
    public uint OwnerUnitId { get; set; }
    public uint ParentUnitId { get; set; } // to be confirmed
    public bool Explosion { get; set; }
    public List<LootItem> LootItems { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(OwnerUnitId);
        writer.Write(ParentUnitId);
        writer.Write(Explosion);
        writer.Write(LootItems.Count);
        LootItems.ForEach(i => i.Write(writer));
    }
}
