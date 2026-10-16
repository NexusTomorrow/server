using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(21)]
public sealed partial class Server0247Union0Arm15 : IServer0247Union0
{

    public Server0247Part0 Unknown0 { get; set; }
}
