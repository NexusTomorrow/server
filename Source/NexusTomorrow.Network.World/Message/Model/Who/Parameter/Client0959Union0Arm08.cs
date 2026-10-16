using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(8)]
public sealed partial class Client0959Union0Arm08 : IClient0959Union0
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
