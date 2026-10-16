using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(2)]
public sealed partial class Client01C3Union0Arm02 : IClient01C3Union0
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
