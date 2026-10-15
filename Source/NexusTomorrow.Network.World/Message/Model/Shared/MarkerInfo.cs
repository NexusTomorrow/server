using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class MarkerInfo
{

    [UInt(32)] public GroupMarker Marker { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
