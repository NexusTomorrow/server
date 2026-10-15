using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.MailBox)]
public sealed partial class MailboxEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
}
