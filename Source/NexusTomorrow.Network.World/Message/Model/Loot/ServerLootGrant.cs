using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ServerLootGrant)]
[WireMessage(GameMessageOpcode.ServerLootGrant, Direction.ServerToClient)]
public sealed partial class ServerLootGrant : IWritable
{
    [UInt(32)] public uint OwnerUnitId { get; set; }
    [UInt(32)] public uint LooterUnitId { get; set; }
    public LootItem LootItem { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(OwnerUnitId);
        writer.Write(LooterUnitId);
        LootItem.Write(writer);
    }
}
