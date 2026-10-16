using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class Server0262Part10
{
    [WireStruct]
    public sealed partial class Server0262Part11
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(3)] public byte Unknown7 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(4)] public byte Unknown8 { get; set; }
    [UInt(3)] public byte Unknown9 { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part11> Unknown10 { get; set; } = [];
}
