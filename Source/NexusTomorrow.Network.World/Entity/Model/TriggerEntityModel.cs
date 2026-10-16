using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(22)]
public sealed partial class Server0262Union0Arm16 : IServer0262Union0
{
    [WireStruct]
    public sealed partial class Server0262Part2
    {
        [UInt(2)] public byte Unknown0 { get; set; }
        public Client0096Part0 Unknown1 { get; set; }
        public Client0096Part0 Unknown2 { get; set; }
        [Float32] public float Unknown3 { get; set; }
    }

    [WideString] public string Unknown0 { get; set; }
    [Array(CountBits = 8)] public List<Server0262Part2> Unknown1 { get; } = [];
}
