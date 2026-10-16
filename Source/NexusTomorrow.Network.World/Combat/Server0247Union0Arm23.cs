using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(35)]
public sealed partial class Server0247Union0Arm23 : IServer0247Union0
{

    [UInt(32)] public uint Unknown0 { get; set; }
}
