using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ClientLootItem)]
public class ClientLootItem : IReadable
{
    public uint OwnerUnitId { get; private set; }
    public uint LootUnitId { get; private set; }
    public bool Request { get; private set; } // 1 = request, 0 = collect

    public void Read(GamePacketReader reader)
    {
        OwnerUnitId = reader.ReadUInt();
        LootUnitId = reader.ReadUInt();
        Request = reader.ReadBit();
    }
}
