using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(3)]
public sealed partial class Client0959Union0Arm03 : IClient0959Union0
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [UInt(6)] public byte Unknown1 { get; set; }
}
