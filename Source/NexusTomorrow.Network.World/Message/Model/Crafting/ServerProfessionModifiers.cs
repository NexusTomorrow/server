using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerProfessionModifiers, Direction.ServerToClient)]
public sealed partial class ServerProfessionModifiers
{
    [WireStruct]
    public sealed partial class CraftingModifier
    {
        [UInt(32)] public CraftingModifierType Type { get; set; }
        [UInt(32)] public TradeskillType TradeskillId { get; set; }
        [Pad(32)] public Pad TradeskillIdHigh { get; }
        [UInt(32)] public uint Item2TypeId { get; set; }
        [UInt(32)] public uint Item2MaterialId { get; set; }
        [Float32] public float Coefficient { get; set; } // Might be used additive or multiplier, depends on Type
        [UInt(32)] public uint FixedValue { get; set; }
    }

    [Array(CountBits = 32)] public List<CraftingModifier> Modifiers { get; set; } = [];
}
