using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerDatacubeUpdateList, Direction.ServerToClient)]
public sealed partial class ServerDatacubeUpdateList
{
    [Array(CountBits = 32)] public List<Datacube> DatacubeData { get; set; } = [];
    [Array(CountBits = 32)] public List<Datacube> DatacubeVolumeData { get; set; } = [];
}
