using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerCraftingFinish, NexusTomorrow.Network.Codec.Direction.ServerToClient)]
public sealed partial class ServerCraftingFinish
{
    [UInt(1)] public bool Pass { get; set; }
    [UInt(15)] public uint TradeskillSchematic2IdCrafted { get; set; }
    [UInt(18)] public uint Item2IdCrafted { get; set; }
    [UInt(32)] public uint Unused { get; set; }
    [UInt(32)] public CraftingDiscovery HotOrCold { get; set; }
    [UInt(4)] public CraftingDirection Direction { get; set; }
    [UInt(32)] public uint EarnedXp { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> MaterialReturnedIds { get; set; } = [];
}
