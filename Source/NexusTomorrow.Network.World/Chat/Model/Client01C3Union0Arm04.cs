using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(4)]
public sealed partial class Client01C3Union0Arm04 : IClient01C3Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
}
