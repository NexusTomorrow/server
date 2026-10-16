using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(6)]
public sealed partial class Client01C3Union0Arm06 : IClient01C3Union0
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
