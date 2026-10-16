using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0935Part0
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
