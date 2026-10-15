using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Item : IWritable
{
    [WireStruct]
    public sealed partial class UnknownStructure : IWritable
    {
        [UInt(3)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown0, 3u);
            writer.Write(Unknown4);
            writer.Write(Unknown8);
        }
    }

    [WireStruct]
    public sealed partial class UnknownStructure2 : IWritable
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown8 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown0, 14u);
            writer.Write(Unknown8);
        }
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

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Unknown0);
        writer.Write(ItemId, 18u);
        LocationData.Write(writer);
        writer.Write(StackCount);
        writer.Write(Charges);
        writer.Write(RandomCircuitData);
        writer.Write(RandomGlyphData);
        writer.Write(ThresholdData);
        writer.Write(Durability);
        writer.Write(Unknown44);
        writer.Write(Unknown48);
        writer.Write(DyeData);
        writer.Write(DynamicFlags);
        writer.Write(ExpirationTimeLeft);

        for (uint i = 0u; i < Unknown58.Length; i++)
            Unknown58[i].Write(writer);

        writer.Write(Unknown70, 18u);

        writer.Write(Microchips.Count, 3u);
        Microchips.ForEach(m => writer.Write(m));
        writer.Write(Glyphs.Count, 4u);
        Glyphs.ForEach(g => writer.Write(g));
        writer.Write(Glyphs.Count, 6u);
        Unknown88.ForEach(u => u.Write(writer));

        writer.Write(EffectiveItemLevel);
    }
}
