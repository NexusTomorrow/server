using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client00FC, Direction.ClientToServer)]
public sealed partial class Client00FC
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
}
