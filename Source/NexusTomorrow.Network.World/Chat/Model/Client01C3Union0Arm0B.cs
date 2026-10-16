using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(11)]
public sealed partial class Client01C3Union0Arm0B : IClient01C3Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
