using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMarkerInfo
{
    [Array(CountBits = 32)] public MarkerInfo[] Markers { get; set; }
}
