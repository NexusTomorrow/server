using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class GroupMarkerInfo : IWritable
    {
        public MarkerInfo[] Markers { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Markers.Length);
            foreach (MarkerInfo markerInfo in Markers)
                markerInfo.Write(writer);
        }
    }
}
