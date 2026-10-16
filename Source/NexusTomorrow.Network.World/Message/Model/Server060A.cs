using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server060A, Direction.ServerToClient)]
public sealed partial class Server060A
{
    [WireStruct]
    public sealed partial class Server060APart1
    {
        [UInt(16)] public ushort Unknown0 { get; set; }
        [UInt(8)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server060APart2
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server060APart0
    {
        public Client00B5Part0 Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(14)] public uint Unknown3 { get; set; }
        [UInt(14)] public uint Unknown4 { get; set; }
        [UInt(14)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(3)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(1)] public bool Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }
        [FixedArray(5)] public Server060APart1[] Unknown12 = new Server060APart1[5];
        [Array(CountBits = 32)] public List<Server060APart2> Unknown13 { get; set; } = [];

    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server060APart0> Unknown1 { get; set; } = [];
}
