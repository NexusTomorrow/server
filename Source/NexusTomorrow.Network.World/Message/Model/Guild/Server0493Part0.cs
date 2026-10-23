using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Server0493Part0
{
    [WireStruct]
    public sealed partial class Server0493Part3
    {
        [WireStruct]
        public sealed partial class Server0493Part4
        {
            [UInt(32)] public uint Unknown0 { get; set; }
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(32)] public uint Unknown2 { get; set; }
            [UInt(32)] public uint Unknown3 { get; set; }
            [UInt(32)] public uint Unknown4 { get; set; }
            [UInt(32)] public uint Unknown5 { get; set; }
        }

        [WideString] public string Unknown0 { get; set; } = "";
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(3)] public byte Unknown2 { get; set; }
        public Server0493Part4 Unknown3 { get; set; } = new();
        [Float32] public float Unknown4 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0493Part2
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [Float32] public float Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }

    [FixedArray(10)] public GuildRank[] Unknown4 { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];

    public GuildHolomark Unknown5 { get; set; } = new();

    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }

    [UInt(64)] public ulong Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [FixedArray(10), WideString] public string[] Unknown13 { get; set; } = [.. Enumerable.Repeat(string.Empty, 10)];

    [FixedArray(16), UInt(8)] public byte[] Unknown14 { get; set; } = new byte[16];

    [Array(CountBits = 32)] public List<Server0493Part2> Unknown15 { get; set; } = [];

    public Server0493Part3 Unknown16 { get; set; } = new();
}
