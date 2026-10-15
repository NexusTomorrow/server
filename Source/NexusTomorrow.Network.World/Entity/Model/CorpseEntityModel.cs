using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.CorpseUnit)]
public sealed partial class CorpseEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
