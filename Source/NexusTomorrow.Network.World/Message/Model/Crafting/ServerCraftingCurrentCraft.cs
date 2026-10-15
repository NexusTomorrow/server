using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerCraftingCurrentCraft, Direction.ServerToClient)]
public sealed partial class ServerCraftingCurrentCraft
{
    [UInt(15)] public uint TradeskillSchematic2Id { get; set; }
    public CraftStats CraftStats { get; set; } = new CraftStats();
    [UInt(32)] public uint GlyphData { get; set; } // is of type RuneSlots in "Item / Inventory message" pull request
    [UInt(32)] public uint SchematicCount { get; set; }
    [UInt(32)] public uint AdditiveCount { get; set; }
    [UInt(32)] public uint Unused { get; set; } = 0;
    [FixedArray(5), UInt(32)] public uint[] Item2IdModifiers { get; set; } = new uint[5];
    [UInt(18)] public uint Item2Id { get; set; }
    [Float32] public Vector2 DiscoveryCoordinates { get; set; }
    [Float32] public Vector2 DiscoveryVectorMultiplier { get; set; }
    [Float32] public float DiscoveryRadiusMultiplier { get; set; }
}
