using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Lockbox)]
public sealed partial class LockboxEntityModel : IEntityModel
{
    [WireStruct]
    public sealed partial class UnknownLockboxStructure : IWritable
    {
        [UInt(32)] public uint Unknown0 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown0);
        }
    }

    [UInt(18)] public uint CreatureId { get; set; }
    [Array(CountBits = 16)] public List<UnknownLockboxStructure> Unknown1 { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18);
        writer.Write((ushort)Unknown1.Count, 16u);
        Unknown1.ForEach(s => s.Write(writer));
    }
}
