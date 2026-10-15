using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Hidden)]
public sealed partial class HiddenEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
