using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(7)]
public sealed partial class Client01C3Union0Arm07 : IClient01C3Union0
{
    [UInt(14)] public uint Unknown0 { get; set; }
}
