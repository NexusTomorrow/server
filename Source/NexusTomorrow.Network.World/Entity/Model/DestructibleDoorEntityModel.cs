using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.DestructibleDoor)]
public sealed partial class DestructibleDoorEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
