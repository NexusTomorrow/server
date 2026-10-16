using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(0)]
public sealed partial class Client01C3Union0Arm00 : IClient01C3Union0
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
