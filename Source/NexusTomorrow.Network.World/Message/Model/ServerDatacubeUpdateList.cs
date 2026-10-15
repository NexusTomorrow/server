using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerDatacubeUpdateList)]
[WireMessage(GameMessageOpcode.ServerDatacubeUpdateList, Direction.ServerToClient)]
public sealed partial class ServerDatacubeUpdateList : IWritable
{
    [Array(CountBits = 32)] public List<Datacube> DatacubeData { get; set; } = [];
    [Array(CountBits = 32)] public List<Datacube> DatacubeVolumeData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DatacubeData.Count);
        DatacubeData.ForEach(d => d.Write(writer));

        writer.Write(DatacubeVolumeData.Count);
        DatacubeVolumeData.ForEach(d => d.Write(writer));
    }
}
