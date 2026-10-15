using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Trigger)]
public sealed partial class TriggerEntityModel : IEntityModel
{
    [WireStruct]
    public sealed partial class Bound
    {
        [UInt(2)] public byte Type { get; set; }
        public Position Min { get; set; }
        public Position Lim { get; set; }
        [Float32] public float Radius { get; set; }
    }

    [WideString] public string Name { get; set; }
    [Array(CountBits = 8)] public List<Bound> Bounds { get; } = [];
}
