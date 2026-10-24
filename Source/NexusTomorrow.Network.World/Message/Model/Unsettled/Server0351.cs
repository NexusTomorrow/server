using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0351, Direction.ServerToClient)]
public sealed partial class Server0351
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [Float32] public float Unknown5 { get; set; }
    [Float32] public float Unknown6 { get; set; }
    [Float32] public float Unknown7 { get; set; }
}
