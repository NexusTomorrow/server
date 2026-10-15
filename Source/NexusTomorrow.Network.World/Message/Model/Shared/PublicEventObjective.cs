using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventObjective
{
    [UInt(15)] public uint ObjectiveId { get; set; }
    public PublicEventObjectiveStatus ObjectiveStatus { get; set; }
    [UInt(1)] public bool Busy { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
    [UInt(32)] public uint NotificationMode { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Locations { get; set; } = []; // array of worldLocation2Id
    [Array(CountBits = 32)] public List<MapRegion> MapRegions { get; set; } = [];
}
