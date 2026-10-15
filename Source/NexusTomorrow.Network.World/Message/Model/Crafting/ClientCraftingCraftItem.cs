using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingCraftItem, Direction.ClientToServer)]
public sealed partial class ClientCraftingCraftItem
{
    [UInt(32)] public uint ClientSpellcastUniqueId { get; private set; }
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; private set; }
    [UInt(18)] public uint SchematicCount { get; private set; }
}
