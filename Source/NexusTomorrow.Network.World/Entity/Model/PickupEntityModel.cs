using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class PickupEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }
    [UInt(32)] public uint Unknown0 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(OwnerId);
        writer.Write(Unknown0);
    }
}
