using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(1)]
public sealed partial class Server0262Union2Arm01 : IServer0262Union2
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
