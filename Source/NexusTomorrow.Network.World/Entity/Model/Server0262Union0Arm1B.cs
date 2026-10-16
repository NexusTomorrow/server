using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(27)]
public sealed partial class Server0262Union0Arm1B : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; }
}
