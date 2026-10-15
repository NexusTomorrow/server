using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.HousingHarvestPlug)]
public sealed partial class HousingHarvestPlugEntityModel : IEntityModel
{
    [UInt(15)] public ushort PlugId { get; set; }
    [UInt(14)] public ushort SocketId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PlugId, 15u);
        writer.Write(SocketId, 14u);
    }
}
