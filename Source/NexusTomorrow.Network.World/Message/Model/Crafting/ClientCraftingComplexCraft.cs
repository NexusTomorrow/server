using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingComplexCraft, Direction.ClientToServer)]
public sealed partial class ClientCraftingComplexCraft
{
    [UInt(32)] public uint ClientSpellcastUniqueId { get; private set; }
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; private set; }
    public CraftStats CraftStats { get; private set; } = new CraftStats();
    [UInt(18)] public uint PowerCoreItem2Id { get; private set; }
    [UInt(32)] public uint ApSpSplitDelta { get; private set; }
    [Array(CountBits = 3), UInt(32)] public int[] ChargeCounts { get; private set; }
}
