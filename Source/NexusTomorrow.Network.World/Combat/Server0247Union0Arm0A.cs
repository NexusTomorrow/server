using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(10)]
public sealed partial class Server0247Union0Arm0A : IServer0247Union0
{

    public Server0247Part0 Unknown0 { get; set; } = new();
}
