using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class MapRegion
{
    [UInt(14)] public uint WorldSocketId { get; set; }
    [UInt(17)] public uint WorldLocation2Id { get; set; }
}
