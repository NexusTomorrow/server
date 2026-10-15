using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent
{
    [Message(GameMessageOpcode.ServerMapTrackedUnitDisable)]
    public class ServerMapTrackedUnitDisable : IWritable
    {
        public uint TrackedUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TrackedUnitId);
        }
    }
}
