using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

[WireMessage(GameMessageOpcode.Server0988, Direction.ServerToClient)]
public sealed partial class Server0988
{
    [WireStruct]
    public sealed partial class Server0988Part0
    {
        [WideString] public string Unknown0 { get; set; } = "";
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(1)] public bool Unknown5 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0988Part1
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(32)] public uint Unknown2 { get; set; }
        [Float32] public float Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0988Part0> Unknown0 { get; set; } = [];
    [UInt(3)] public byte Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<Server0988Part1> Unknown2 { get; set; } = [];
}
