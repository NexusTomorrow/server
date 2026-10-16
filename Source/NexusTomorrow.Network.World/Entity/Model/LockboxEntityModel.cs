using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(36)]
public sealed partial class Server0262Union0Arm24 : IServer0262Union0
{
    [WireStruct]
    public sealed partial class Server0262Part3
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [UInt(18)] public uint Unknown0 { get; set; }
    [Array(CountBits = 16)] public List<Server0262Part3> Unknown1 { get; } = [];
}
