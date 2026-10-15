using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Chest)]
public sealed partial class ChestEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
