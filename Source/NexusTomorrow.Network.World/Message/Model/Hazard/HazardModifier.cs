using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireStruct]
public sealed partial class Message010BPart0
{
    [Float32] public float Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
