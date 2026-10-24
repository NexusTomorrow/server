using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0348, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server0348
{
    [Float32] public float Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [Float32] public float Unknown5 { get; set; }
    [Float32] public float Unknown6 { get; set; }
    [Float32] public float Unknown7 { get; set; }
    [Float32] public float Unknown8 { get; set; }
    [Float32] public float Unknown9 { get; set; }
    [Float32] public float Unknown10 { get; set; }
    [Float32] public float Unknown11 { get; set; }
}
