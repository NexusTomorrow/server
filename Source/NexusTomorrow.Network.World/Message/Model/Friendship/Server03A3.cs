using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03A3, Direction.ServerToClient)]
public sealed partial class Server03A3
{
    [WireStruct]
    public sealed partial class Server03A3Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; } = "";
        [Float32] public float Unknown3 { get; set; }
        [WideString] public string Unknown4 { get; set; } = "";
        [WideString] public string Unknown5 { get; set; } = "";
        [UInt(3)] public byte Unknown6 { get; set; }
        [Array(CountBits = 32)] public List<Server0399Part0> Unknown7 { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<Server03A3Part0> Unknown0 { get; set; } = [];
}
