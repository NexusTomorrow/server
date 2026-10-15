using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerDatacubeUpdate)]
    public class ServerDatacubeUpdate : IWritable
    {
        public Datacube DatacubeData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            DatacubeData.Write(writer);
        }
    }
}
