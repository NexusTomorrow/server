using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.NonPlayer)]
public sealed partial class NonPlayerEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(8)] public byte QuestChecklistIdx { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18);
        writer.Write(QuestChecklistIdx);
    }
}
