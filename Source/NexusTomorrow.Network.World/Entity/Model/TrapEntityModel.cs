using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class TrapEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(OwnerId);
    }
}
