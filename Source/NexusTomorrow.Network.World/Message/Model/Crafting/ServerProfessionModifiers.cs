using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerProfessionModifiers)]
[WireMessage(GameMessageOpcode.ServerProfessionModifiers, Direction.ServerToClient)]
public sealed partial class ServerProfessionModifiers : IWritable
{
    [WireStruct]
    public sealed partial class CraftingModifier : IWritable
    {
        [UInt(32)] public CraftingModifierType Type { get; set; }
        [UInt(32)] public TradeskillType TradeskillId { get; set; }
        [Pad(32)] public Pad TradeskillIdHigh { get; }
        [UInt(32)] public uint Item2TypeId { get; set; }
        [UInt(32)] public uint Item2MaterialId { get; set; }
        [Float32] public float Coefficient { get; set; } // Might be used additive or multiplier, depends on Type
        [UInt(32)] public uint FixedValue { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Type, 32u);
            writer.Write(TradeskillId);
            writer.Write(Item2TypeId);
            writer.Write(Item2MaterialId);
            writer.Write(Coefficient);
            writer.Write(FixedValue);
        }
    }

    [Array(CountBits = 32)] public List<CraftingModifier> Modifiers { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Modifiers.Count);
        foreach (CraftingModifier modifier in Modifiers)
        {
            modifier.Write(writer);
        }
    }
}
