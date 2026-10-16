using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server067B, Direction.ServerToClient)]
public sealed partial class Server067B
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server067BPart0> Unknown1 { get; set; } = [];
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [Float32] public float Unknown7 { get; set; }
    [Float32] public float Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
}
