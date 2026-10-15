using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Camera)]
public sealed partial class CameraEntityModel : IEntityModel
{
    [UInt(1)] public bool Unknown0 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown0);
    }
}
