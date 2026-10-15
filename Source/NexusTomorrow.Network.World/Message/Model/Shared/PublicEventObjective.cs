using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventObjective : IWritable
{
    [UInt(15)] public uint ObjectiveId { get; set; }
    public PublicEventObjectiveStatus ObjectiveStatus { get; set; }
    [UInt(1)] public bool Busy { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
    [UInt(32)] public uint NotificationMode { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Locations { get; set; } = []; // array of worldLocation2Id
    [Array(CountBits = 32)] public List<MapRegion> MapRegions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 15);
        ObjectiveStatus.Write(writer);
        writer.Write(Busy);
        writer.Write(ElapsedTimeMs);
        writer.Write(NotificationMode);

        writer.Write(Locations.Count);
        Locations.ForEach(location => writer.Write(location));

        writer.Write(MapRegions.Count);
        MapRegions.ForEach(region => region.Write(writer));
    }
}
