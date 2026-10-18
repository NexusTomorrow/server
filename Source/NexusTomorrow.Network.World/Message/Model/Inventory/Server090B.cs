using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server090B, Direction.ServerToClient)]
public sealed partial class Server090B
{
    [WireStruct]
    public sealed partial class Server090BPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server090BPart1
    {
        [WireStruct]
        public sealed partial class Server090BPart2
        {
            [UInt(3)] public byte Unknown0 { get; set; }
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(32)] public uint Unknown2 { get; set; }
        }

        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(17)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(32)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(64)] public ulong Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        public Server090BPart2 Unknown11 { get; set; } = new();
        public Server090BPart2 Unknown12 { get; set; } = new();
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server090BPart0> Unknown1 { get; } = [];
    [Array(CountBits = 32)] public List<Server090BPart1> Unknown2 { get; } = [];
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
}
