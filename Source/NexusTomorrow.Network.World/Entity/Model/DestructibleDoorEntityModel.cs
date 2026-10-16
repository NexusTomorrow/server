using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(30)]
public sealed partial class Server0262Union0Arm1E : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
}
