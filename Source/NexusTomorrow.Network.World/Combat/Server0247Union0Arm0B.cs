using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(11)]
public sealed partial class Server0247Union0Arm0B : IServer0247Union0
{

    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
}
