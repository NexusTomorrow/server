using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMemberInfo
{
    [WireStruct]
    public sealed partial class Server0405Part2
    {
        [UInt(16)] public ushort Unknown0 { get; set; }
        [UInt(8)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0405Part3
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    [WideString] public string Unknown0 { get; set; } = "";
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(6)] public byte Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(6)] public byte Unknown5 { get; set; }
    [UInt(2)] public byte Unknown6 { get; set; }
    [UInt(7)] public byte Unknown7 { get; set; }
    [UInt(7)] public byte Unknown8 { get; set; }
    [UInt(3)] public byte Unknown9 { get; set; }
    [UInt(17)] public uint Unknown10 { get; set; }
    [UInt(16)] public ushort Unknown11 { get; set; }

    [FixedArray(5)] public Server0405Part2[] Unknown12 { get; set; } = [new(), new(), new(), new(), new()];

    public RealmScopedId Unknown13 { get; set; } = new();

    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(16)] public ushort Unknown15 { get; set; }
    [UInt(16)] public ushort Unknown16 { get; set; }
    [UInt(16)] public ushort Unknown17 { get; set; }
    [UInt(16)] public ushort Unknown18 { get; set; }
    [UInt(16)] public ushort Unknown19 { get; set; }
    [UInt(16)] public ushort Unknown20 { get; set; }
    [UInt(16)] public ushort Unknown21 { get; set; }
    [UInt(16)] public ushort Unknown22 { get; set; }
    [UInt(16)] public ushort Unknown23 { get; set; }
    [UInt(16)] public ushort Unknown24 { get; set; }
    [UInt(16)] public ushort Unknown25 { get; set; }
    [UInt(16)] public ushort Unknown26 { get; set; }

    [UInt(14)] public ushort Unknown27 { get; set; }
    [UInt(15)] public ushort Unknown28 { get; set; }
    [UInt(32)] public uint Unknown29 { get; set; }
    [UInt(32)] public uint Unknown30 { get; set; }
    [UInt(1)] public bool Unknown31 { get; set; }

    [UInt(32)] public uint Unknown32 { get; set; }
    [UInt(32)] public uint Unknown33 { get; set; }

    [Array(CountBits = 32)] public List<Server0405Part3> Unknown34 { get; set; } = [];
}
