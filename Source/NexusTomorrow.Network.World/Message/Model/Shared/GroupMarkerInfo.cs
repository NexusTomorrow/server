using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMarkerInfo : IWritable
{
    [Array(CountBits = 32)] public MarkerInfo[] Markers { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Markers.Length);
        foreach (MarkerInfo markerInfo in Markers)
            markerInfo.Write(writer);
    }
}
