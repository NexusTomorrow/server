using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Item
{
    [WireStruct]
    public sealed partial class UnknownStructure
    {
        [UInt(3)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
    }

    [WireStruct]
    public sealed partial class UnknownStructure2
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown8 { get; set; }
    }

    [UInt(64)] public ulong Guid { get; set; }
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(18)] public uint ItemId { get; set; }
    public ItemLocation LocationData { get; set; }
    [UInt(32)] public uint StackCount { get; set; }
    [UInt(32)] public uint Charges { get; set; }
    [UInt(64)] public ulong RandomCircuitData { get; set; }
    [UInt(32)] public uint RandomGlyphData { get; set; }
    [UInt(64)] public ulong ThresholdData { get; set; }
    [Float32] public float Durability { get; set; }
    [UInt(32)] public uint Unknown44 { get; set; }
    [UInt(8)] public byte Unknown48 { get; set; }
    [UInt(32)] public uint DyeData { get; set; }
    [UInt(32)] public uint DynamicFlags { get; set; }
    [UInt(32)] public uint ExpirationTimeLeft { get; set; }
    [FixedArray(2)] public UnknownStructure[] Unknown58 { get; set; } = [new(), new()];
    [UInt(18)] public uint Unknown70 { get; set; }
    [Array(CountBits = 3), UInt(32)] public List<uint> Microchips { get; } = [];
    [Array(CountBits = 4), UInt(32)] public List<uint> Glyphs { get; } = [];
    [Array(CountBits = 6)] public List<UnknownStructure2> Unknown88 { get; } = [];
    [UInt(32)] public uint EffectiveItemLevel { get; set; }
}
