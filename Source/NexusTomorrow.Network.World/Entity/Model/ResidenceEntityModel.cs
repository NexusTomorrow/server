using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class ResidenceEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
    }
}
