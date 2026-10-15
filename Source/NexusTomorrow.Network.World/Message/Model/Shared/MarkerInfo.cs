using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Group;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class MarkerInfo : IWritable
    {
        public uint UnitId { get; set; }

        public GroupMarker Marker { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Marker, 32u);
            writer.Write(UnitId);
        }
    }
}
