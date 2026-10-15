using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class MapRegion : IWritable
{
    [UInt(15)] public uint WorldSocketId { get; set; }
    [UInt(17)] public uint WorldLocation2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WorldSocketId, 15u);
        writer.Write(WorldLocation2Id, 17u);
    }
}
