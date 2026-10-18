using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

[WireMessage(GameMessageOpcode.Server098B, Direction.ServerToClient)]
public sealed partial class Server098B
{
    [WireStruct]
    public sealed partial class Server098BPart0
    {
        [WireStruct]
        public sealed partial class Server098BPart1
        {
            [WireStruct]
            public sealed partial class Server098BPart2
            {
                [UInt(5)] public byte Unknown0 { get; set; }
                [Float32] public float Unknown1 { get; set; }
                [UInt(2)] public byte Unknown2 { get; set; }
                [Float32] public float Unknown3 { get; set; }
                [UInt(64)] public ulong Unknown4 { get; set; }
                [UInt(64)] public ulong Unknown5 { get; set; }
            }

            [WireStruct]
            public sealed partial class Server098BPart3
            {
                public interface IServer098BUnion0 : IWireStruct
                {
                }

                [WireStruct]
                [UnionCase(0)]
                public sealed partial class Server098BUnion0Arm00 : IServer098BUnion0
                {
                    [UInt(15)] public ushort Unknown0 { get; set; }
                    [UInt(32)] public uint Unknown1 { get; set; }
                }

                [WireStruct]
                [UnionCase(1)]
                public sealed partial class Server098BUnion0Arm01 : IServer098BUnion0
                {
                    [UInt(32)] public uint Unknown0 { get; set; }
                    [UInt(32)] public uint Unknown1 { get; set; }
                }

                [WireStruct]
                [UnionCase(2)]
                public sealed partial class Server098BUnion0Arm02 : IServer098BUnion0
                {
                    [UInt(32)] public uint Unknown0 { get; set; }
                }

                [UInt(32)] public uint Unknown0 { get; set; }
                [Union(nameof(Unknown0))] public IServer098BUnion0 Unknown1 { get; set; } = new Server098BUnion0Arm00();
            }

            [UInt(32)] public uint Unknown0 { get; set; }
            [WideString] public string Unknown1 { get; set; } = "";
            [WideString] public string Unknown2 { get; set; } = "";
            [Float32] public float Unknown3 { get; set; }
            [Float32] public float Unknown4 { get; set; }
            [UInt(32)] public uint Unknown5 { get; set; }
            [UInt(64)] public ulong Unknown6 { get; set; }
            [UInt(8)] public byte Unknown7 { get; set; }
            [Array(CountBits = 32)] public List<Server098BPart2> Unknown8 { get; set; } = [];
            [Array(CountBits = 32)] public List<Server098BPart3> Unknown9 { get; set; } = [];
        }

        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; } = "";
        [WideString] public string Unknown3 { get; set; } = "";
        [UInt(14)] public ushort Unknown4 { get; set; }
        [Array(CountBits = 32)] public List<Server098BPart1> Unknown5 { get; set; } = [];

        [UInt(32), Count] public uint Unknown6 { get; set; }
        [Array(CountFrom = nameof(Unknown6)), UInt(32)] public List<uint> Unknown7 { get; set; } = [];
        [Array(CountFrom = nameof(Unknown6)), UInt(32)] public List<uint> Unknown8 { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<Server098BPart0> Unknown0 { get; set; } = [];
}
