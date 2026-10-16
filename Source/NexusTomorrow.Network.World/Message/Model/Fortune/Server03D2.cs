using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.Server03D2, Direction.ServerToClient)]
public sealed partial class Server03D2
{
    [WireStruct]
    public sealed partial class Server03D2Part0
    {
        [UInt(5)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server03D2Part0> Unknown1 { get; set; } = [];
    [Array(CountBits = 32), Float32] public List<float> Unknown2 { get; set; } = [];
    [Array(CountBits = 32), Float32] public List<float> Unknown3 { get; set; } = [];
}
