using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(4)]
public sealed partial class Client07DCUnion0Arm04 : IClient07DCUnion0
{
    [UInt(5)] public byte Unknown0 { get; private set; }
    [UInt(8)] public byte Unknown1 { get; private set; }
}
