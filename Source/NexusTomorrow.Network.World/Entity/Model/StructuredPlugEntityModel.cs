using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.StructuredPlug)]
public sealed partial class StructuredPlugEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(2)] public byte CurrentTier { get; set; }
    [UInt(14)] public ushort SocketId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(CurrentTier, 2u);
        writer.Write(SocketId, 14u);
    }
}
