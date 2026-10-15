using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.HousingMannequin)]
public sealed partial class HousingMannequinEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
