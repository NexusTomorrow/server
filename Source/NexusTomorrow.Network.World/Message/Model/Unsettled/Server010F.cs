using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server010F, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server010F
{
    [WireStruct]
    public sealed partial class Server010FPart0
    {
        [UInt(7)] public byte Unknown0 { get; set; }
        [UInt(15)] public ushort Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server010FPart1
    {
        [UInt(7)] public byte Unknown0 { get; set; }
        [UInt(15)] public ushort Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(5)] public byte Unknown3 { get; set; }
    [UInt(5)] public byte Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [Array(CountBits = 32)] public List<Server010FPart0> Unknown7 { get; } = [];
    [Array(CountBits = 32)] public List<Server010FPart1> Unknown8 { get; } = [];
    [UInt(15)] public ushort Unknown9 { get; set; }
    [UInt(15)] public ushort Unknown10 { get; set; }
    [UInt(14)] public ushort Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(32)] public uint Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(32)] public uint Unknown15 { get; set; }
    [UInt(32)] public uint Unknown16 { get; set; }
    [UInt(3)] public byte Unknown17 { get; set; }
    [UInt(1)] public bool Unknown18 { get; set; }
    [UInt(1)] public bool Unknown19 { get; set; }
    [UInt(32)] public uint Unknown20 { get; set; }
    [UInt(4), Count] public byte Unknown21 { get; set; }
    [Array(CountFrom = nameof(Unknown21)), UInt(32)] public List<uint> Unknown22 { get; } = [];
    [Array(CountFrom = nameof(Unknown21)), UInt(32)] public List<uint> Unknown23 { get; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown24 { get; } = [];
    [UInt(32)] public uint Unknown25 { get; set; }
}
