using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(7)]
public sealed partial class Server0262Union0Arm07 : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 3)] public List<Server0262Part0> Unknown3 { get; set; } = [];
}
