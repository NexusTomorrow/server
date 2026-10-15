using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ClientLootItem)]
[WireMessage(GameMessageOpcode.ClientLootItem, Direction.ClientToServer)]
public sealed partial class ClientLootItem : IReadable
{
    [UInt(32)] public uint OwnerUnitId { get; private set; }
    [UInt(32)] public uint LootUnitId { get; private set; }
    [UInt(1)] public bool Request { get; private set; } // 1 = request, 0 = collect

    public void Read(GamePacketReader reader)
    {
        OwnerUnitId = reader.ReadUInt();
        LootUnitId = reader.ReadUInt();
        Request = reader.ReadBit();
    }
}
