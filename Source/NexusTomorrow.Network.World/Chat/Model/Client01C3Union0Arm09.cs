using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(9)]
public sealed partial class Client01C3Union0Arm09 : IClient01C3Union0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
