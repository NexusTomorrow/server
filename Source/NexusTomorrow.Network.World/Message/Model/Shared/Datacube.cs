using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class Datacube : IWritable
    {
        public ushort DatacubeId { get; set; }
        public uint Progress { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(DatacubeId, 14u);
            writer.Write(Progress);
        }
    }
}
