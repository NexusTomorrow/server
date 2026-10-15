using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingCraftItemAutoCraft)]
[WireMessage(GameMessageOpcode.ClientCraftingCraftItemAutoCraft, Direction.ClientToServer)]
public sealed partial class ClientCraftingCraftItemAutoCraft : IReadable
{
    [UInt(32)] public uint ClientSpellcastUniqueId { get; private set; } // creates a spell cast when the craft is Runecrafting
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; private set; }
    [UInt(32)] public uint SchematicCount { get; private set; }
    [UInt(18)] public uint CatalystItem2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ClientSpellcastUniqueId = reader.ReadUInt();
        CraftingStationUnitId = reader.ReadUInt();
        TradeskillSchematic2Id = reader.ReadUInt();
        SchematicCount = reader.ReadUInt();
        CatalystItem2Id = reader.ReadUInt(18);
    }
}
