using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingComplexCraft)]
[WireMessage(GameMessageOpcode.ClientCraftingComplexCraft, Direction.ClientToServer)]
public sealed partial class ClientCraftingComplexCraft : IReadable
{
    [UInt(32)] public uint ClientSpellcastUniqueId { get; private set; }
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; private set; }
    public CraftStats CraftStats { get; private set; } = new CraftStats();
    [UInt(18)] public uint PowerCoreItem2Id { get; private set; }
    [UInt(32)] public uint ApSpSplitDelta { get; private set; }
    [Array(CountBits = 3), UInt(32)] public int[] ChargeCounts { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ClientSpellcastUniqueId = reader.ReadUInt();
        CraftingStationUnitId = reader.ReadUInt();
        TradeskillSchematic2Id = reader.ReadUInt();
        CraftStats.Read(reader);
        PowerCoreItem2Id = reader.ReadUInt(18);
        ApSpSplitDelta = reader.ReadUInt();

        uint count = reader.ReadUInt(3);
        ChargeCounts = new int[count];
        for (int i = 0; i < count; i++)
        {
            ChargeCounts[i] = reader.ReadInt();
        }
    }
}
