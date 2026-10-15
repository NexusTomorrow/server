using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerDatacubeVolumeUpdate)]
    public class ServerDatacubeVolumeUpdate : IWritable
    {
        public Datacube DatacubeVolumeData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            DatacubeVolumeData.Write(writer);
        }
    }
}
