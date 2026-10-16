using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(4)]
public sealed partial class Client0959Union0Arm04 : IClient0959Union0
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
}
