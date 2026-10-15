using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ServerLootNotify)]
[WireMessage(GameMessageOpcode.ServerLootNotify, Direction.ServerToClient)]
public sealed partial class ServerLootNotify : IWritable
{
    [UInt(32)] public uint OwnerUnitId { get; set; }
    [UInt(32)] public uint ParentUnitId { get; set; } // to be confirmed
    [UInt(1)] public bool Explosion { get; set; }
    [Array(CountBits = 32)] public List<LootItem> LootItems { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(OwnerUnitId);
        writer.Write(ParentUnitId);
        writer.Write(Explosion);
        writer.Write(LootItems.Count);
        LootItems.ForEach(i => i.Write(writer));
    }
}
