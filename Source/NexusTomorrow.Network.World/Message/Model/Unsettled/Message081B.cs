using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message081B, Direction.Both)]
public sealed partial class Message081B
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown5 { get; } = [];
}
