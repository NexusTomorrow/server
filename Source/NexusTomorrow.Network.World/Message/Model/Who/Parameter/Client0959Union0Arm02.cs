using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(2)]
public sealed partial class Client0959Union0Arm02 : IClient0959Union0
{
    [UInt(3)] public byte Unknown0 { get; private set; }
}
