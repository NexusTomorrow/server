using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Lockbox)]
public sealed partial class LockboxEntityModel : IEntityModel
{
    [WireStruct]
    public sealed partial class UnknownLockboxStructure
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [UInt(18)] public uint CreatureId { get; set; }
    [Array(CountBits = 16)] public List<UnknownLockboxStructure> Unknown1 { get; } = [];
}
