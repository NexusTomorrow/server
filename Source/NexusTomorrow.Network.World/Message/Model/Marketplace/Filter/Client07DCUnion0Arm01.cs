using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(1)]
public sealed partial class Client07DCUnion0Arm01 : IClient07DCUnion0
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [Float32] public float Unknown1 { get; private set; }
}
