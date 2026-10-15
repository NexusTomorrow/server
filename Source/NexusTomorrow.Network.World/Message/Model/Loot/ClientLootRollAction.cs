using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ClientLootRollAction)]
[WireMessage(GameMessageOpcode.ClientLootRollAction, Direction.ClientToServer)]
public sealed partial class ClientLootRollAction : IReadable
{
    [UInt(32)] public uint OwnerUnitId { get; private set; }
    [UInt(32)] public uint LootUnitId { get; private set; }
    [UInt(2)] public LootRollAction Action { get; private set; }

    public void Read(GamePacketReader reader)
    {
        OwnerUnitId = reader.ReadUInt();
        LootUnitId = reader.ReadUInt();
        Action = reader.ReadEnum<LootRollAction>(2u);
    }
}
