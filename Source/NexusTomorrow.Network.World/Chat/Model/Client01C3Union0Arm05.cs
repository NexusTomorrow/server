using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(5)]
public sealed partial class Client01C3Union0Arm05 : IClient01C3Union0
{
    [UInt(15)] public ushort Unknown0 { get; set; }
}
