using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerDatacubeUpdateList)]
public class ServerDatacubeUpdateList : IWritable
{
    public List<Datacube> DatacubeData { get; set; } = [];
    public List<Datacube> DatacubeVolumeData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DatacubeData.Count);
        DatacubeData.ForEach(d => d.Write(writer));

        writer.Write(DatacubeVolumeData.Count);
        DatacubeVolumeData.ForEach(d => d.Write(writer));
    }
}
