using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(2)]
public sealed partial class Server0247Union0Arm02 : IServer0247Union0
{

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
}
