using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Residence)]
public sealed partial class ResidenceEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
