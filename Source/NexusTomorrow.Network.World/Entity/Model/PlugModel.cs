using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Plug)]
public sealed partial class PlugModel : IEntityModel
{
    [UInt(14)] public ushort SocketId { get; set; }
    [UInt(15)] public ushort PlugId { get; set; }
    [UInt(6)] public byte PlugFlags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SocketId, 14u);
        writer.Write(PlugId, 15u);
        writer.Write(PlugFlags, 6u);
    }
}
